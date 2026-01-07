using UnityEngine;

public class SlimeWanderFlee : MonoBehaviour
{
    public SpriteRenderer spawnArea;      // Área donde NO puede salirse
    public Transform fearCenter;          // Centro del radio (si es null, usa el propio slime)
    public float fearRadius = 3f;         // Radio para activar miedo

    public float wanderSpeed = 1.5f;      // Velocidad paseando
    public float baseFleeSpeed = 3f;      // Base al huir

    public float moveTimeMin = 0.6f;      // Tiempo mínimo moviéndose
    public float moveTimeMax = 1.4f;      // Tiempo máximo moviéndose
    public float idleTimeMin = 0.4f;      // Tiempo mínimo parado
    public float idleTimeMax = 1.2f;      // Tiempo máximo parado

    public float fearMultiplier = 1.5f;   // Cuánto afecta la rareza (miedo)
    public float stdDev = 0.4f;           // Ruido normal para velocidad (variación)

    [HideInInspector] public int spawnChance; // % con el que spawnea ESTE slime
    [HideInInspector] public int maxChance;   // % máximo entre todos (se lo pasas desde el spawner)

    Transform player;
    Vector3 targetPos;
    float timer;
    bool isMoving;                        // para “no movimiento continuo”
    float fleeSpeed;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Player debe tener tag "Player"

        if (fearCenter == null) fearCenter = transform;                // si no asignas, usa el slime

        PickNewWanderTarget();                                         // primer objetivo
        SetIdle();                                                     // empieza parado (opcional)

        // Calcula fleeSpeed según rareza (menos spawnChance = más miedo)
        float rareFactor = 1f - ((float)spawnChance / maxChance);      // 0..1 aprox
        float fear = 1f + rareFactor * fearMultiplier;                 // 1..(1+mult)

        float noise = RandomNormal() * stdDev;                         // variación normal
        fleeSpeed = baseFleeSpeed * fear + noise;                      // raros => más rápido

        if (fleeSpeed < 1f) fleeSpeed = 1f;                            // mínimo para que no se quede quieto
    }

    void Update()
    {
        if (spawnArea == null) return;                                 // si no hay área, no hacemos nada

        bool fearActive = IsPlayerInsideFear();                        // ¿está el player dentro del radio?

        if (fearActive)
        {
            Flee();                                                    // huir
        }
        else
        {
            Wander();                                                  // pasear
        }
    }

    bool IsPlayerInsideFear()
    {
        float d = Vector2.Distance(player.position, fearCenter.position); // distancia player-centro
        return d <= fearRadius;                                           // true si está dentro del radio
    }

    void Wander()
    {
        timer -= Time.deltaTime;                                       // cuenta atrás del estado

        if (timer <= 0f)
        {
            if (isMoving)
            {
                SetIdle();                                             // pasa a “parado”
            }
            else
            {
                PickNewWanderTarget();                                 // nuevo punto aleatorio
                SetMoving();                                           // pasa a “moviendo”
            }
        }

        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(                  // mueve hacia el target
                transform.position,
                targetPos,
                wanderSpeed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, targetPos) < 0.05f) // si llegó, cambia a idle
            {
                SetIdle();
            }
        }
    }

    void Flee()
    {
        Vector3 dirAway = (transform.position - player.position).normalized; // dirección opuesta

        Vector3 newPos = transform.position + dirAway * fleeSpeed * Time.deltaTime; // se aleja

        newPos = ClampToBounds(newPos);                                  // evita salirse del área
        transform.position = newPos;
    }

    void PickNewWanderTarget()
    {
        Bounds b = spawnArea.bounds;                                    // límites del rectángulo

        float x = Random.Range(b.min.x, b.max.x);                       // uniforme en X
        float y = Random.Range(b.min.y, b.max.y);                       // uniforme en Y

        targetPos = new Vector3(x, y, 0f);
    }

    Vector3 ClampToBounds(Vector3 p)
    {
        Bounds b = spawnArea.bounds;

        float x = Mathf.Clamp(p.x, b.min.x, b.max.x);                   // recorta X dentro del área
        float y = Mathf.Clamp(p.y, b.min.y, b.max.y);                   // recorta Y dentro del área

        return new Vector3(x, y, 0f);
    }

    void SetMoving()
    {
        isMoving = true;
        timer = Random.Range(moveTimeMin, moveTimeMax);                 // durará moviéndose X tiempo
    }

    void SetIdle()
    {
        isMoving = false;
        timer = Random.Range(idleTimeMin, idleTimeMax);                 // durará parado X tiempo
    }

    float RandomNormal()
    {
        float u1 = Random.value;
        float u2 = Random.value;

        return Mathf.Sqrt(-2f * Mathf.Log(u1)) * Mathf.Cos(2f * Mathf.PI * u2); // normal estándar
    }
}
