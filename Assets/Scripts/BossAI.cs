using UnityEngine;

public class BossAI : MonoBehaviour
{
    public float velocidad = 3.5f;
    public float vida = 100f;
    private bool activo = false;
    private Transform jugador;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) jugador = p.transform;
    }

    public void ActivarBoss() => activo = true;

    void FixedUpdate()
    {
        if (activo && jugador != null && rb != null)
        {
            Vector2 pos = Vector2.MoveTowards(rb.position, jugador.position, velocidad * Time.fixedDeltaTime);
            rb.MovePosition(pos);
        }
    }

    public void RecibirDano(float cant)
    {
        vida -= cant;
        GameManager.instance.ActualizarVidaBoss(vida);
        if (vida <= 0) {
            GameManager.instance.FinalizarJuego(true);
            Destroy(gameObject);
        }
    }
}