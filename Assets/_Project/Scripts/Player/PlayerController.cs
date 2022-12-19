using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRigidBody2D;
    private Animator playerAnimator;

    private GameManager gameManager;

    private float horizontal;

    [SerializeField] private bool isOnTheGround;

    [Range(0, 500)] public float jumpForce;
    [Range(0, 50)] public float speed;
    public float speedY;
    
    public int speedX;

    public Transform groundCheck;

    private void Initialization()
    {
        gameManager = FindObjectOfType(typeof(GameManager)) as GameManager;
        playerRigidBody2D = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
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
    }

    private void LateUpdate()
    {
        AnimationsControl();
    }

    private void MovementControl()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if(horizontal != 0)
        {
            speedX = 1;
        }
        else
        {
            speedX = 0;
        }

        speedY = playerRigidBody2D.velocity.y;
        playerRigidBody2D.velocity = new(horizontal * speed, speedY);
    }

    private void GroundCheck()
    {
        isOnTheGround = Physics2D.OverlapCircle(groundCheck.position, 0.02f);
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && isOnTheGround)
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
                gameManager.AddScore(10);
                Destroy(collision.gameObject);
                break;
            case "Obstacles":
                Debug.LogError("PUFT");
                break;

        }
    }
}
