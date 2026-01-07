using UnityEngine;

public class BossAI : MonoBehaviour
{
    [Header("Configuración")]
    public float velocidad = 3.5f;
    public float vida = 100f;
    private bool activo = false;
    private Transform jugador;

    void Start()
    {
        // Buscamos al jugador por su Tag
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) jugador = p.transform;
    }

    // ESTA ES LA FUNCIÓN QUE TE DA ERROR EN EL MANAGER
    public void ActivarBoss()
    {
        activo = true;
        Debug.Log("IA del Boss Activada");
    }

    void Update()
    {
        if (activo && jugador != null)
        {
            // Persecución directa
            transform.position = Vector2.MoveTowards(transform.position, jugador.position, velocidad * Time.deltaTime);
        }
    }

    public void RecibirDano(float cantidad)
    {
        vida -= cantidad;
        if (vida <= 0)
        {
            Debug.Log("Boss muerto");
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si el Boss toca al jugador
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth ph = collision.gameObject.GetComponent<PlayerHealth>();
            if (ph != null) ph.QuitarVida(1);
        }
    }
}