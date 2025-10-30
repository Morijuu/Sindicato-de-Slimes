using UnityEngine;

public class SlimeFollow : MonoBehaviour
{
    public float followMovement = 5f;
    private Transform player;
    public bool isFollowing = false;
    void Start()
    {

    }

    void OnTriggerStay2D(Collider2D x)
    {
        // Detecta si el jugador está dentro del rango
        if (x.CompareTag("Player"))
        {
            // Si presiona E, empieza a seguirlo
            if (Input.GetKeyDown(KeyCode.E))
            {
                player = x.transform;
                isFollowing = true;
            }
        }
    }

    void Update()
    {
        // Si está en modo seguimiento, se mueve hacia el jugador
        if (isFollowing && player != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, followMovement * Time.deltaTime);

        }
    }
}