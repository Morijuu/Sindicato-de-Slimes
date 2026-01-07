using UnityEngine;
using System.Collections.Generic;

public class SlimeFollow : MonoBehaviour
{
    // Esta es la lista que BossManager y RoomTransition buscan
    public static List<GameObject> fila = new List<GameObject>();

    [Header("Estado")]
    public bool siguiendo = false; // Esta es la variable que daba error
    
    [Header("Ajustes")]
    public float velocidad = 6f;
    private Transform objetivo;
    private bool jugadorCerca = false;

    void Start()
    {
        // Inicializar la fila con el Player si es el primer slime
        if (fila.Count == 0)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) fila.Add(p);
        }
    }

    void Update()
    {
        // RECLUTAR con la E
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E) && !siguiendo)
        {
            Reclutar();
        }

        // MOVIMIENTO
        if (siguiendo && objetivo != null)
        {
            float dist = Vector2.Distance(transform.position, objetivo.position);

            // TP si el jugador cambia de sala (distancia grande)
            if (dist > 12f) transform.position = objetivo.position;

            // Seguir al objetivo
            if (dist > 1.2f)
            {
                transform.position = Vector2.MoveTowards(transform.position, objetivo.position, velocidad * Time.deltaTime);
            }
        }
    }

    void Reclutar()
    {
        if (fila.Count > 0)
        {
            objetivo = fila[fila.Count - 1].transform;
            fila.Add(this.gameObject);
            siguiendo = true;

            // Desactivar movimiento aleatorio si existe
            if (GetComponent<SlimeWanderFlee>() != null) GetComponent<SlimeWanderFlee>().enabled = false;
            
            Debug.Log("Slime reclutado. Siguiendo a: " + objetivo.name);
        }
    }

    public void CambiarObjetivo(Transform nuevo) 
    { 
        objetivo = nuevo; 
        siguiendo = true;
    }

    // Detección del jugador (Asegúrate de tener un CircleCollider2D como Trigger)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) jugadorCerca = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) jugadorCerca = false;
    }
}