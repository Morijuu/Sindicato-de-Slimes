using UnityEngine;

public class SlimeFlee : MonoBehaviour
{
    public float baseSpeed = 3f;              // Velocidad base media
    public float fearMultiplier = 1.5f;       // Cuánto influye el miedo
    public float stdDev = 0.4f;                // Desviación típica (ruido normal)

    [HideInInspector] public int spawnChance;  // Porcentaje con el que spawnea este slime
    [HideInInspector] public int maxChance;    // Mayor porcentaje posible (ej. 50)

    private float speed;                       // Velocidad final del slime
    private Transform player;                  // Referencia al jugador

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Busca al jugador


        //Validaciones defensivas

        if (maxChance <= 0)
        {
            Debug.LogError("maxChance inválido, se corrige a 1");
            maxChance = 1;
        }

        if (spawnChance < 0)
        {
            Debug.LogError("spawnChance inválido, se corrige a 0");
            spawnChance = 0;
        }

        float rareFactor = 1f - ((float)spawnChance / maxChance);      // Menos probabilidad = más raro
        float fear = 1f + rareFactor * fearMultiplier;                 // Calcula el miedo

        float noise = RandomNormal() * stdDev;                          // Ruido normal (gaussiano)
        speed = baseSpeed * fear + noise;                               // Velocidad final

        speed = Mathf.Max(1f, speed);                                   // Evita velocidades negativas
    }

    void Update()
    {
        Vector3 dirAway = transform.position - player.position;        // Dirección opuesta al jugador
        dirAway = dirAway.normalized;                                   // Normaliza la dirección

        transform.position += dirAway * speed * Time.deltaTime;        // Mueve el slime alejándose
    }

    float RandomNormal()
    {
        float u1 = Random.value;                                        // Valor aleatorio 0..1
        float u2 = Random.value;                                        // Valor aleatorio 0..1

        return Mathf.Sqrt(-2f * Mathf.Log(u1)) *
               Mathf.Cos(2f * Mathf.PI * u2);                           // Distribución normal estándar
    }
}
