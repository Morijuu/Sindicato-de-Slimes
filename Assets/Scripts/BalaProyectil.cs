using UnityEngine;

public class BalaProyectil : MonoBehaviour
{
    public float dano = 10f;
    private bool haChocado = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (haChocado) return;

        if (collision.CompareTag("Boss"))
        {
            haChocado = true;
            
            BossAI boss = collision.GetComponent<BossAI>();
            if (boss != null)
            {
                // Llamamos a la función de daño y usamos "vida"
                boss.RecibirDano(dano);
            }

            Destroy(gameObject);
        }
    }
}