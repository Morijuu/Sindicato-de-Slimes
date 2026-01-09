using UnityEngine;

public class SlimeWanderFlee : MonoBehaviour
{
    public SpriteRenderer spawnArea;      // Área donde NO puede salirse
    public Transform fearCenter;          // Centro del radio (si es null, usa el propio slime)
    public float fearRadius = 3f;         // Radio para activar miedo

    [Header("Fear hysteresis (para que no se apague al instante)")]
    public float fearExitMultiplier = 1.4f;   // el miedo se apaga cuando esté bastante más lejos
    private bool fearState = false;           // estado real de miedo (con memoria)


    public float wanderSpeed = 1.5f;      // Velocidad paseando
    public float baseFleeSpeed = 3f;      // Base al huir

    public float moveTimeMin = 0.6f;      // Tiempo mínimo moviéndose (wander)
    public float moveTimeMax = 1.4f;      // Tiempo máximo moviéndose (wander)
    public float idleTimeMin = 0.4f;      // Tiempo mínimo parado (wander)
    public float idleTimeMax = 1.2f;      // Tiempo máximo parado (wander)

    public float fearMultiplier = 1.5f;   // Cuánto afecta la rareza (miedo)
    public float stdDev = 0.4f;           // Ruido normal para velocidad (variación)

    [Header("Fear stop/go (para poder atraparlos)")]
    public float fleeMoveMin = 0.25f;     // tiempo mínimo huyendo (fear)
    public float fleeMoveMax = 0.70f;     // tiempo máximo huyendo (fear)
    public float fleeStopMin = 0.10f;     // tiempo mínimo parado (fear)
    public float fleeStopMax = 0.35f;     // tiempo máximo parado (fear)

    [HideInInspector] public int spawnChance; // % con el que spawnea ESTE slime
    [HideInInspector] public int maxChance;   // % máximo entre todos (se lo pasas desde el spawner)

    Transform player;
    Vector3 targetPos;
    float timer;
    bool isMoving;                        // para “no movimiento continuo” (wander)
    float fleeSpeed;

    // Estado fear stop/go
    float fleeTimer = 0f;
    bool fleeMoving = true;
    bool wasFearActive = false;

    void Start()
    {
        GameObject pObj = GameObject.FindGameObjectWithTag("Player");
        if (pObj != null) player = pObj.transform;

        if (fearCenter == null) fearCenter = transform;

        PickNewWanderTarget();
        SetIdle();

        // Seguridad: evita división por 0 si te llega mal desde el spawner
        if (maxChance <= 0) maxChance = 1;
        if (spawnChance < 0) spawnChance = 0;

        // Calcula fleeSpeed según rareza (menos spawnChance = más miedo)
        float rareFactor = 1f - ((float)spawnChance / maxChance);      // 0..1 aprox
        rareFactor = Mathf.Clamp01(rareFactor);

        float fear = 1f + rareFactor * fearMultiplier;                 // 1..(1+mult)

        float noise = RandomNormal() * stdDev;                         // variación normal
        fleeSpeed = baseFleeSpeed * fear + noise;                      // raros => más rápido

        if (fleeSpeed < 1f) fleeSpeed = 1f;
    }

    void Update()
    {
        if (spawnArea == null) return;
        if (player == null) return;

        bool fearActive = IsPlayerInsideFear();

        // Si el estado fear ha cambiado, reseteamos timers para que no se quede “atascado”
        if (fearActive && !wasFearActive)
        {
            // Entrando en fear: empieza huyendo y crea primer tramo aleatorio
            fleeMoving = true;
            fleeTimer = Random.Range(fleeMoveMin, fleeMoveMax);
        }
        else if (!fearActive && wasFearActive)
        {
            // Saliendo de fear: resetea el patrón fear
            fleeTimer = 0f;
            fleeMoving = true;
        }

        wasFearActive = fearActive;

        if (fearActive)
            Flee();
        else
            Wander();
    }

    bool IsPlayerInsideFear()
    {
        float d = Vector2.Distance(player.position, fearCenter.position);

        float enterR = fearRadius;
        float exitR = fearRadius * fearExitMultiplier;

        // Si NO estaba asustado, solo se activa al entrar en el radio normal
        if (!fearState)
        {
            if (d <= enterR) fearState = true;
        }
        else
        {
            // Si YA estaba asustado, no se apaga hasta salir de un radio más grande
            if (d >= exitR) fearState = false;
        }

        return fearState;
    }


    void Wander()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            if (isMoving) SetIdle();
            else
            {
                PickNewWanderTarget();
                SetMoving();
            }
        }

        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPos,
                wanderSpeed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, targetPos) < 0.05f)
            {
                SetIdle();
            }
        }
    }

    void Flee()
    {
        // 1) Alterna run/stop aleatorio
        fleeTimer -= Time.deltaTime;

        if (fleeTimer <= 0f)
        {
            fleeMoving = !fleeMoving;

            if (fleeMoving)
                fleeTimer = Random.Range(fleeMoveMin, fleeMoveMax);
            else
                fleeTimer = Random.Range(fleeStopMin, fleeStopMax);
        }

        // 2) Si toca “stop”, no se mueve (pero sigue en fear)
        if (!fleeMoving) return;

        // 3) Huida normal
        Vector3 dirAway = (transform.position - player.position).normalized;

        // Si por algún motivo dirAway da (0,0,0), evita NaN
        if (dirAway.sqrMagnitude < 0.0001f) dirAway = Vector3.right;

        Vector3 newPos = transform.position + dirAway * fleeSpeed * Time.deltaTime;

        newPos = ClampToBounds(newPos);
        transform.position = newPos;
    }

    void PickNewWanderTarget()
    {
        Bounds b = spawnArea.bounds;

        float x = Random.Range(b.min.x, b.max.x);
        float y = Random.Range(b.min.y, b.max.y);

        targetPos = new Vector3(x, y, 0f);
    }

    Vector3 ClampToBounds(Vector3 p)
    {
        Bounds b = spawnArea.bounds;

        float x = Mathf.Clamp(p.x, b.min.x, b.max.x);
        float y = Mathf.Clamp(p.y, b.min.y, b.max.y);

        return new Vector3(x, y, 0f);
    }

    void SetMoving()
    {
        isMoving = true;
        timer = Random.Range(moveTimeMin, moveTimeMax);
    }

    void SetIdle()
    {
        isMoving = false;
        timer = Random.Range(idleTimeMin, idleTimeMax);
    }

    float RandomNormal()
    {
        float u1 = Random.value;
        float u2 = Random.value;

        // Evita Log(0)
        if (u1 < 0.0001f) u1 = 0.0001f;

        return Mathf.Sqrt(-2f * Mathf.Log(u1)) * Mathf.Cos(2f * Mathf.PI * u2);
    }
}
