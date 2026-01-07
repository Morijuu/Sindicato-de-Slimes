using UnityEngine;
using System.Collections;

public class SlimeCombat : MonoBehaviour
{
    public GameObject balaPrefab;
    private Transform bossActual;
    private SlimeStats stats;
    private bool disparando = false;
    public float rangoDeteccion = 7f; // Rango mediano para que tengas que acercarlo

    void Awake() => stats = GetComponent<SlimeStats>();

    public void ActivarAtaque(Transform target)
    {
        bossActual = target;
        if (!disparando) StartCoroutine(BucleCombate());
    }

    IEnumerator BucleCombate()
    {
        disparando = true;
        while (bossActual != null)
        {
            float distancia = Vector2.Distance(transform.position, bossActual.position);

            // Solo dispara si el boss está en rango
            if (distancia <= rangoDeteccion)
            {
                int balas = 2; float cd = 3f;
                switch (stats.tipoSeleccionado)
                {
                    case TipoSlime.Rapido: balas = 3; cd = 2f; break;
                    case TipoSlime.Pesado: balas = 5; cd = 3.5f; break;
                    case TipoSlime.Tanque: balas = 6; cd = 3f; break;
                }

                for (int i = 0; i < balas; i++)
                {
                    if (bossActual == null) break;
                    Disparar();
                    yield return new WaitForSeconds(0.15f);
                }
                yield return new WaitForSeconds(cd);
            }
            else
            {
                yield return new WaitForSeconds(0.5f); // Esperar un poco antes de volver a chequear distancia
            }
        }
        disparando = false;
    }

    void Disparar()
    {
        GameObject b = Instantiate(balaPrefab, transform.position, Quaternion.identity);
        Vector2 dir = (bossActual.position - transform.position).normalized;
        float disp = Random.Range(-15f, 15f);
        Vector2 dirFinal = Quaternion.Euler(0, 0, disp) * dir;
        
        // Configurar la bala
        BalaProyectil scriptBala = b.GetComponent<BalaProyectil>() ?? b.AddComponent<BalaProyectil>();
        scriptBala.dano = 1f;
        
        Rigidbody2D rb = b.GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = dirFinal * 12f;
    }
}