using UnityEngine;

public class Parrallax : MonoBehaviour
{
    public Transform cameraTransform; // Reference to the camera
    public float parallaxFactor;      // Speed of the parallax effect
    public float textureUnitSizeX;
    private Vector3 lastCameraPosition;

    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform; // Default to the main camera
        }
        if (textureUnitSizeX == 0)
        {
            Sprite sprite = GetComponent<SpriteRenderer>().sprite;
            textureUnitSizeX = sprite.bounds.size.x / 2 / 2 * transform.localScale.x;
        }
        lastCameraPosition = cameraTransform.position;
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    void Update()
    {
        // Calculate the difference in camera movement
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;

        // Apply the parallax effect
        transform.position += new Vector3(deltaMovement.x * parallaxFactor, deltaMovement.y, 0);
        transform.position.Set(transform.position.x, cameraTransform.position.y, 0);
        float cameraOffsetX = cameraTransform.position.x - transform.position.x;
        if (Mathf.Abs(cameraOffsetX) >= textureUnitSizeX)
        {
            // Reposition the background
            float offsetX = (cameraOffsetX > 0 ? 1 : -1) * textureUnitSizeX;
            transform.position += new Vector3(offsetX, 0, 0);
        }
        // Update the last camera position
        lastCameraPosition = cameraTransform.position;
    }
}
