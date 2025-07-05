using UnityEngine;

public class SlashCollision : MonoBehaviour
{



    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            // StartCoroutine(other.gameObject.GetComponent<Enemy>().Die());
            other.gameObject.GetComponent<Enemy>().IsDying = true;
            // other.gameObject.GetComponent<Enemy>().Die();
        }
    }
}
