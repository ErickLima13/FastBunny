using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRigidBody2D;

    private GameManager gameManager;

    [SerializeField] private bool isOnTheGround;

    [Range(0, 500)] public float jumpForce;

    public Transform groundCheck;

    private void Initialization()
    {
        gameManager = FindObjectOfType(typeof(GameManager)) as GameManager;
        playerRigidBody2D = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialization();
    }
    private void FixedUpdate()
    {
        isOnTheGround = Physics2D.OverlapCircle(groundCheck.position, 0.02f);
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && isOnTheGround)
        {
            playerRigidBody2D.AddForce(new Vector2(0, jumpForce));
        }
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
