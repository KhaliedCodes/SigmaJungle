using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Set the camera's position to the player's position
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);

    }
}
