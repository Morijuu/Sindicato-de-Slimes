using UnityEngine;

public class CameraFollowRoom : MonoBehaviour
{
    public float transitionSpeed = 6f;
    private Vector3 targetPosition;

    private void Start()
    {
        targetPosition = transform.position;
    }

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            Time.deltaTime * transitionSpeed
        );
    }

    public void MoveToNewRoom(Vector3 newPosition)
    {
        targetPosition = new Vector3(
            newPosition.x,
            newPosition.y,
            transform.position.z
        );
    }
}
