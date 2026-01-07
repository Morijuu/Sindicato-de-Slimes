using UnityEngine;

public class Bala : MonoBehaviour
{
    public float daño = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boss"))
        {
            // Aquí llamaríamos a una función de "RecibirDaño" del Boss
            Debug.Log("¡Impacto en el Boss!");
            Destroy(gameObject); // La bala desaparece al chocar
        }
        
        if (collision.CompareTag("Wall")) 
        {
            Destroy(gameObject);
        }
    }
}