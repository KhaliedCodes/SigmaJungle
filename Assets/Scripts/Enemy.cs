using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;

    bool isMoving = false;
    public float speed = 10;

    private float lastExecutionTime;
    public bool IsDying = false;

    enum Direction
    {
        Left,
        Right,
        Idle
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
        if (direction == Direction.Left)
        {
            rb.linearVelocityX = -speed;
            isMoving = true;
        }
        else
        if (direction == Direction.Right)
        {
            rb.linearVelocityX = speed;
            isMoving = true;
        }
        else
        {
            rb.linearVelocityX = 0;
            isMoving = false;
        }
        if (Time.time - lastExecutionTime >= 5f)
        {
            lastExecutionTime = Time.time;
            if (direction == Direction.Left)
            {
                direction = Direction.Right;
            }
            else
            {
                direction = Direction.Left;
            }
            if (direction == Direction.Idle)
            {
                isMoving = false;
            }
        }
        transform.localScale = direction == Direction.Left ? new Vector3(-8, 8, 8) : new Vector3(8, 8, 8);
        GetComponent<Animator>().SetBool("IsMoving", isMoving);

        if (IsDying)
        {
            StartCoroutine(Die());
        }

    }



    public IEnumerator Die()
    {
        direction = Direction.Idle;
        GetComponent<Animator>().SetTrigger("Die");
        yield return null;
        do
        {
            IsDying = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Death");

            if (!IsDying)
            {
                Destroy(gameObject);
                break;
            }

            yield return null;
        } while (true);

    }
}
