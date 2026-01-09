using UnityEngine;
using System.Collections;

public class SlimeCombat : MonoBehaviour
{
    public GameObject balaPrefab;
    public float rangoDeteccion = 15f;
    private Transform bossActual;
    private bool disparando = false;

    public void ActivarAtaque(Transform target) => bossActual = target;

    void Update()
    {
        if (bossActual == null)
        {
            GameObject b = GameObject.FindWithTag("Boss");
            if (b != null) bossActual = b.transform;
        }
        
        if (bossActual != null && !disparando)
        {
            if (Vector2.Distance(transform.position, bossActual.position) <= rangoDeteccion)
            {
                StartCoroutine(BucleDisparo());
            }
        }
    }

    IEnumerator BucleDisparo()
    {
        disparando = true;
        while (bossActual != null)
        {
            if (Vector2.Distance(transform.position, bossActual.position) <= rangoDeteccion)
            {
                Disparar();
                yield return new WaitForSeconds(1.2f);
            }
            else { break; }
        }
        disparando = false;
    }

    void Disparar()
    {
        if (balaPrefab == null || bossActual == null) return;

        GameObject b = Instantiate(balaPrefab, transform.position, Quaternion.identity);
        Vector2 direccion = (bossActual.position - transform.position).normalized;

        // IMPORTANTE: Para que detecte colisión sin Rigidbody, 
        // al menos UNO de los dos debe ser cinemático o tener un RB básico.
        // Vamos a asegurarnos de que la bala se mueva.
        BalaMovimiento mov = b.GetComponent<BalaMovimiento>() ?? b.AddComponent<BalaMovimiento>();
        mov.configurar(direccion, 14f); 

        BalaProyectil bp = b.GetComponent<BalaProyectil>() ?? b.AddComponent<BalaProyectil>();
        bp.dano = 5f; 
    }
}