using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int vidaActual = 8; 

    void Start()
    {
        vidaActual = 8; 
        if (GameManager.instance != null) GameManager.instance.ActualizarVidaJugador(vidaActual);
    }

    public void QuitarVida(int cantidad)
    {
        vidaActual -= cantidad;
        if (GameManager.instance != null) GameManager.instance.ActualizarVidaJugador(vidaActual);

        if (vidaActual <= 0) {
            GameManager.instance.FinalizarJuego(false);
            gameObject.SetActive(false); 
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boss")) {
            QuitarVida(1);
        }
    }
}