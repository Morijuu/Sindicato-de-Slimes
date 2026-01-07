using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public float vida = 100f;

    public void RecibirDano(float cantidad)
    {
        vida -= cantidad;
        if (vida <= 0)
        {
            Debug.Log("¡Boss Derrotado!");
            Destroy(gameObject);
        }
    }
}