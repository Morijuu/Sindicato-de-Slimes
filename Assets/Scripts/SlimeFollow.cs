using UnityEngine;
using System.Collections.Generic;

public class SlimeFollow : MonoBehaviour
{
    public static List<Transform> followers = new List<Transform>(); 
    
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private float stopDistance = 1.5f;
    [SerializeField] private float trailingDistance = 1.0f;
    
    private Transform player;
    private Transform followTarget;
    private bool playerInRange = false;
    public bool isFollowing = false;
    
    [SerializeField] private int maxSlimes = 5;
    
    void Start()
    {
        if (followers.Count == 0 && GameObject.FindGameObjectWithTag("Player") != null)
        {
            followers.Add(GameObject.FindGameObjectWithTag("Player").transform);
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!isFollowing && followers.Count < maxSlimes + 1)
            {
                isFollowing = true;
                
                followTarget = followers[followers.Count - 1];
                
                followers.Add(this.transform);
                
                playerInRange = false;
            }
        }

        if (isFollowing && followTarget != null)
        {
            float requiredDistance = (followTarget == followers[0]) ? stopDistance : trailingDistance;
            
            float distance = Vector2.Distance(transform.position, followTarget.position);

            if (distance > requiredDistance)
            {
                transform.position = Vector2.MoveTowards(
                    transform.position,
                    followTarget.position,
                    followSpeed * Time.deltaTime
                );
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isFollowing)
        {
            player = other.transform;
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}