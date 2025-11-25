using UnityEngine;
using System.Collections.Generic;

public class RoomTransition : MonoBehaviour
{
    [SerializeField] private Vector2 targetPosition; 
    
    private CameraFollowRoom cameraFollow; 

    void Start()
    {
        cameraFollow = FindAnyObjectByType<CameraFollowRoom>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // MOVER JUGADOR
            other.transform.position = targetPosition;
            
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.linearVelocity = Vector2.zero;
            }

            // MOVER SLIMES
            for (int i = 1; i < SlimeFollow.followers.Count; i++)
            {
                Transform slimeTransform = SlimeFollow.followers[i];
                
                slimeTransform.position = targetPosition; 
                
                Rigidbody2D slimeRb = slimeTransform.GetComponent<Rigidbody2D>();
                if (slimeRb != null)
                {
                    slimeRb.linearVelocity = Vector2.zero;
                }
            }

            // MOVER CÁMARA
            if (cameraFollow != null)
            {
                Vector3 cameraTarget = new Vector3(targetPosition.x, targetPosition.y, 0f);
                cameraFollow.MoveToNewRoom(cameraTarget);
            }
        }
    }
}