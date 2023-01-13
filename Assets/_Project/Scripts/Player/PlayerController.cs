using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRigidBody2D;
    private Animator playerAnimator;
    private GameManager gameManager;

    private float horizontal;
    private float speedY;

    [SerializeField] private bool isOnTheGround;

    [Range(0, 1000)] public float jumpForce;
    [Range(0, 50)] public float speed;
    [Range(0, 50)] public float shootSpeed;

    public Vector2 parabolForce;

    public Transform groundCheck;
    public Transform weapon;
    public LayerMask groundLayer;

    private int speedX;
    private int extraJump;

    public int newJump;

    public bool isLeft;
    public bool isShoot;
    public bool isParabol;

    public GameObject[] weaponPrefabs;

    public float fireRate;

    private void Initialization()
    {
        gameManager = FindObjectOfType(typeof(GameManager)) as GameManager;
        playerRigidBody2D = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        extraJump = newJump;

    }

    // Start is called before the first frame update
    void Start()
    {
        Initialization();
    }
    private void FixedUpdate()
    {
        GroundCheck();
    }

    // Update is called once per frame
    void Update()
    {
        MovementControl();
        Jump();
        Shoot();
    }

    private void LateUpdate()
    {
        AnimationsControl();
    }

    private void MovementControl()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (horizontal != 0)
        {
            speedX = 1;
        }
        else
        {
            speedX = 0;
        }

        if (isLeft && horizontal > 0)
        {
            Flip();
        }

        if (!isLeft && horizontal < 0)
        {
            Flip();
        }

        speedY = playerRigidBody2D.velocity.y;
        playerRigidBody2D.velocity = new(horizontal * speed, speedY);
    }

    private void Flip()
    {
        isLeft = !isLeft;
        float scaleX = transform.localScale.x;
        scaleX *= -1f;
        transform.localScale = new(scaleX, transform.localScale.y, transform.localScale.z);
        shootSpeed *= -1f;
        parabolForce.x *= -1f;
    }

    private void Shoot()
    {
        if (Input.GetButtonDown("Fire1") && !isShoot && gameManager.munition > 0)
        {
            StartCoroutine(DelayShoot());
            isShoot = true;
            gameManager.ManagerMunition(-1);
            GameObject temp;
           
            if (!isParabol)
            {
                temp = Instantiate(weaponPrefabs[0]);
                temp.GetComponent<Rigidbody2D>().velocity = new Vector2(shootSpeed, 0);
            }
            else
            {
                temp = Instantiate(weaponPrefabs[1]);
                temp.GetComponent<Rigidbody2D>().AddForce(parabolForce);
            }

            temp.transform.position = weapon.position;
            Destroy(temp, 2f);
        }
    }

    private IEnumerator DelayShoot()
    {
        yield return new WaitForSeconds(fireRate);
        isShoot = false;
    }

    private void GroundCheck()
    {
        isOnTheGround = Physics2D.OverlapCircle(groundCheck.position, 0.02f, groundLayer);

        if (isOnTheGround)
        {
            extraJump = newJump;
        }
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && extraJump > 0)
        {
            playerRigidBody2D.AddForce(new Vector2(0, jumpForce));
            extraJump--;
        }
        else if (Input.GetButtonDown("Jump") && isOnTheGround && extraJump == 0)
        {
            playerRigidBody2D.AddForce(new Vector2(0, jumpForce));
        }
    }

    private void AnimationsControl()
    {
        playerAnimator.SetInteger("SpeedX", speedX);
        playerAnimator.SetBool("Grounded", isOnTheGround);
        playerAnimator.SetFloat("SpeedY", speedY);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, 0.02f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Collectibles":

                IDItem id = collision.gameObject.GetComponent<IDItem>();
                switch (id.name)
                {
                    case "Munition":
                        gameManager.ManagerMunition(id.amount);
                        break;
                }

                Destroy(collision.gameObject);
                break;
            case "Obstacles":
                Debug.LogError("PUFT");
                break;
        }
    }
}
