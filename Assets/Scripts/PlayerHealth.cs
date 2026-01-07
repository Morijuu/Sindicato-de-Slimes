using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int vida = 8;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si el Boss toca al jugador
        if (collision.gameObject.CompareTag("Boss"))
        {
            QuitarVida(1);
        }
    }

    public void QuitarVida(int cantidad)
    {
        vida -= cantidad;
        Debug.Log("Vida Jugador: " + vida);
        if (vida <= 0)
        {
            Debug.Log("Jugador muerto");
            Destroy(gameObject);
        }
    }
}