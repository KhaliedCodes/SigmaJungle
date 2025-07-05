using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Rigidbody2D rb;

    bool isMoving = false;
    public bool isJumping = false;
    public float speed = 15;

    public bool JumpUp = false;
    public bool JumpDown = false;
    public bool Slashing = false;

    private bool isOnRope = false;
    public int Health = 3;
    public int Score = 0;
    public bool IsDying = false;
    private Vector2 RespawnPosition;
    public Vector2 LastSafePosition;

    public Vector2 RopePosition;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI HealthText;
    enum Direction
    {
        Left,
        Right
    }
    Direction direction = Direction.Right;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if (IsDying) return;
        if (Input.GetKeyDown(KeyCode.Z) && !isJumping && !Slashing)
        {
            rb.linearVelocityY = 15;
            // rb.linearVelocityY = 10;
            isJumping = true;
        }

        if (Input.GetKey(KeyCode.LeftArrow) && !Slashing)
        {
            rb.linearVelocityX = -speed;
            isMoving = true;
            direction = Direction.Left;
        }
        else
        if (Input.GetKey(KeyCode.RightArrow) && !Slashing)
        {
            rb.linearVelocityX = speed;
            isMoving = true;
            direction = Direction.Right;
        }
        else
        {
            rb.linearVelocityX = 0;
            isMoving = false;
        }

        if (Input.GetKeyDown(KeyCode.X) && !isJumping && !Slashing)
        {
            GetComponent<Animator>().SetTrigger("Slash1");
            Slashing = true;
        }

        Slashing = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Slash1");

        if (isJumping && rb.linearVelocityY == 0)
        {
            isJumping = false;
        }
        if (rb.linearVelocityY == 0)
        {
            JumpUp = false;
            JumpDown = false;
            // rb.gravityScale = 1;
        }
        if (rb.linearVelocityY > 0)
        {
            // rb.linearVelocityY += Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            JumpUp = true;
            JumpDown = false;
        }
        else if (rb.linearVelocityY < 0)
        {
            rb.gravityScale = 2;
            // rb.linearVelocityY += Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            JumpUp = false;
            JumpDown = true;
        }


        if (rb.linearVelocityY == 0)
        {
            LastSafePosition = transform.position;
        }

        if (isOnRope && isJumping)
        {

            DetachFromRope();
            // transform.position = RopePosition;
        }


        transform.localScale = direction == Direction.Left ? new Vector3(-8, 8, 8) : new Vector3(8, 8, 8);
        GetComponent<Animator>().SetBool("IsMoving", isMoving);

        GetComponent<Animator>().SetBool("JumpUp", JumpUp);
        GetComponent<Animator>().SetBool("JumpDown", JumpDown);
    }
    public void PushPlayer(Vector3 direction)
    {
        GetComponent<Animator>().SetTrigger("Hit");
        // Optional: Ensure the player has a Rigidbody for force-based movement


        rb.AddForce(direction * 15, ForceMode2D.Impulse);

    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Spike")
        {
            transform.position = LastSafePosition;
            Health--;
            HealthText.SetText(Health.ToString());
            if (Health == 0)
            {
                StartCoroutine(Die());
            }
        }
        if (other.gameObject.tag == "Collectable")
        {
            Debug.Log("Collected");
            Score++;
            ScoreText.SetText(Score.ToString());
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Rope")
        {
            AttachToRope(other);
        }
        if (other.gameObject.tag == "Finish")
        {
            SceneManager.LoadScene("UI");
        }
        if (other.gameObject.tag == "Enemy" && !other.gameObject.GetComponent<Enemy>().IsDying)
        {
            Health--;
            HealthText.SetText(Health.ToString());
            var directionVector = other.gameObject.transform.position - transform.position;
            PushPlayer(directionVector.normalized);
            if (Health == 0)
            {
                StartCoroutine(Die());
            }
        }
    }

    void AttachToRope(Collider2D other)
    {
        isOnRope = true;
        rb.gravityScale = 0; // Disable gravity
        rb.linearVelocity = Vector2.zero; // Stop the player from falling
        rb.bodyType = RigidbodyType2D.Kinematic; // Optional: Makes the player easier to position on the rope
        // gameObject.transform.position = other.gameObject.transform.position;
        gameObject.transform.SetParent(other.gameObject.transform);


        // RopePosition = other.gameObject.transform.position;
    }

    void DetachFromRope()
    {
        isOnRope = false;
        rb.gravityScale = 1; // Restore gravity
        transform.rotation = Quaternion.identity; // Optional: Reset rotation
        rb.bodyType = RigidbodyType2D.Dynamic;
        gameObject.transform.SetParent(null);
    }
    public IEnumerator Die()
    {
        GetComponent<Animator>().SetTrigger("Die");
        IsDying = true;
        yield return null;
        do
        {
            IsDying = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Die") || GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Hit");
            // Debug.Log(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Hit"));

            if (!IsDying)
            {
                transform.position = RespawnPosition;
                Health = 3;
                HealthText.SetText(Health.ToString());
                break;
            }

            yield return null;
        } while (true);

    }
}
