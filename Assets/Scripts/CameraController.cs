using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float zOffset = -10f;
    private Transform playerTransform;

    private void Awake()
    {
        PlayerController player = FindFirstObjectByType<PlayerController>();
        playerTransform = player?.transform;
    }

    private void Update()
    {
        if (playerTransform == null)
        {
            return;
        }

        transform.position = playerTransform.position + new Vector3(0f, 0f, zOffset);
    }
}
