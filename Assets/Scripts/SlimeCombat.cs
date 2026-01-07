using UnityEngine;
using System.Collections;

public class SlimeCombat : MonoBehaviour
{
    public GameObject balaPrefab;
    private Transform boss;
    private SlimeStats stats;
    private bool activo = false;

    void Awake() => stats = GetComponent<SlimeStats>();

    public void ActivarAtaque(Transform target)
    {
        boss = target;
        if (stats == null) stats = GetComponent<SlimeStats>();
        activo = true;
        StartCoroutine(RutinaDisparo());
    }

    IEnumerator RutinaDisparo()
    {
        while (activo && boss != null)
        {
            int cantidadBalas = 0;
            float tiempoEspera = 0;

            // Configuramos según tus reglas
            switch (stats.tipoSeleccionado)
            {
                case TipoSlime.Normal: cantidadBalas = 2; tiempoEspera = 3f; break;
                case TipoSlime.Rapido:  cantidadBalas = 3; tiempoEspera = 2f; break;
                case TipoSlime.Pesado:  cantidadBalas = 5; tiempoEspera = 3.5f; break;
                case TipoSlime.Tanque:  cantidadBalas = 6; tiempoEspera = 3f; break;
            }

            // Disparamos la ráfaga
            for (int i = 0; i < cantidadBalas; i++)
            {
                DispararConDispersion();
                yield return new WaitForSeconds(0.1f); // Pequeño retraso entre balas de la misma ráfaga
            }

            yield return new WaitForSeconds(tiempoEspera);
        }
    }

    void DispararConDispersion()
    {
        if (balaPrefab == null || boss == null) return;

        GameObject b = Instantiate(balaPrefab, transform.position, Quaternion.identity);
        
        // Calculamos dirección básica
        Vector2 dirBase = (boss.position - transform.position).normalized;

        // AÑADIMOS DISPERSIÓN (Ángulo aleatorio entre -15 y 15 grados)
        float anguloDispersion = Random.Range(-15f, 15f);
        Vector2 dirFinal = Quaternion.Euler(0, 0, anguloDispersion) * dirBase;

        float vel = (stats.tipoSeleccionado == TipoSlime.Rapido) ? 14f : 9f;
        if (stats.tipoSeleccionado == TipoSlime.Tanque) b.transform.localScale *= 2f;

        Rigidbody2D rb = b.GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = dirFinal * vel;
    }
}