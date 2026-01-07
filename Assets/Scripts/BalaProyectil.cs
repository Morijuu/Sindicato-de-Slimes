using UnityEngine;

public class BalaProyectil : MonoBehaviour
{
    public float dano = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Importante: El Boss debe tener el Tag "Boss"
        if (collision.CompareTag("Boss"))
        {
            BossAI boss = collision.GetComponent<BossAI>();
            if (boss != null)
            {
                boss.RecibirDano(dano);
            }
            Destroy(gameObject); // La bala desaparece al chocar
        }
        // Si choca con una pared (opcional)
        else if (collision.CompareTag("Wall")) 
        {
            Destroy(gameObject);
        }
    }
}
          