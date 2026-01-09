using UnityEngine;

public class BalaMovimiento : MonoBehaviour
{
    private Vector2 dir;
    private float vel;
    private bool listo = false;

    public void configurar(Vector2 direccion, float velocidad)
    {
        dir = direccion;
        vel = velocidad;
        listo = true;
        Destroy(gameObject, 5f); // Se destruye sola a los 5 segundos
    }

    void Update()
    {
        if (listo)
        {
            // Movimiento por Transform: No necesita Rigidbodys ni nada, se mueve SÍ O SÍ
            transform.Translate(dir * vel * Time.deltaTime);
        }
    }
}