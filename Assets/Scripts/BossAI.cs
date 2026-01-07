using UnityEngine;

public class BossAI : MonoBehaviour
{
    public float velocidad = 2.5f;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player != null)
            transform.position = Vector2.MoveTowards(transform.position, player.position, velocidad * Time.deltaTime);
    }
}