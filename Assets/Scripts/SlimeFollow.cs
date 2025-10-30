using UnityEngine;

public class SlimeFollow : MonoBehaviour
{
    [SerializeField] private float followSpeed = 5f;       // Velocidad de movimiento
    [SerializeField] private float stopDistance = 1.5f;    // Distancia mínima al jugador
    private Transform player;
    private bool playerInRange = false;
    public bool isFollowing = false;

    private void Update()
    {
        // Si el jugador está en rango y presiona E, el slime empieza a seguir
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            isFollowing = true;
        }

        // Si está siguiendo y hay un jugador asignado
        if (isFollowing && player != null)
        {
            // Calculamos la distancia actual
            float distance = Vector2.Distance(transform.position, player.position);

            // Si la distancia es mayor al rango de parada, seguir moviéndose
            if (distance > stopDistance)
            {
                transform.position = Vector2.MoveTowards(
                    transform.position,
                    player.position,
                    followSpeed * Time.deltaTime
                );
            }
            // Si está muy cerca, no se mueve (queda quieto)
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Cuando el jugador entra al rango del trigger
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Cuando el jugador sale del rango del trigger
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
