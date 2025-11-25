using UnityEngine;

public class CameraFollowRoom : MonoBehaviour
{
    public float transitionSpeed = 5f; 
    private Vector3 targetPosition;
    
    void Start()
    {
        targetPosition = transform.position;
    }

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(
            transform.position, 
            targetPosition, 
            Time.deltaTime * transitionSpeed
        );
    }

    public void MoveToNewRoom(Vector3 newPosition)
    {
        newPosition.z = transform.position.z; 
        targetPosition = newPosition;
    }
}