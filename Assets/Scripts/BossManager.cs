using UnityEngine;

public class BossManager : MonoBehaviour
{
    public GameObject bossPrefab;
    public GameObject balaPrefab;
    public Transform spawnPoint;
    
    [Header("Rango de la Room_5")]
    public float distanciaParaPoderInvocar = 12f; 

    private bool bossInvocado = false;
    private Transform jugador;

    void Start()
    {
        GameObject p = GameObject.FindWithTag("Player");
        if (p != null) jugador = p.transform;
    }

    void Update()
    {
        if (bossInvocado || jugador == null) return;

        // Calculamos distancia entre el jugador y el centro de esta sala
        float dist = Vector2.Distance(transform.position, jugador.position);

        // SOLO SI ESTÁ CERCA DE LA ROOM_5 Y PULSA B
        if (dist <= distanciaParaPoderInvocar && Input.GetKeyDown(KeyCode.B))
        {
            InvocarTodo();
        }
    }

    void InvocarTodo()
    {
        bossInvocado = true;
        GameManager.instance.ActivarVidaBossUI();

        GameObject boss = Instantiate(bossPrefab, spawnPoint.position, Quaternion.identity);
        boss.tag = "Boss";
        
        BossAI ai = boss.GetComponent<BossAI>();
        if (ai != null) {
            ai.ActivarBoss();
            GameManager.instance.ActualizarVidaBoss(ai.vida);
        }

        // Convertir Slimes a Torretas usando TAG
        GameObject[] slimes = GameObject.FindGameObjectsWithTag("Slime");
        foreach (GameObject s in slimes)
        {
            SlimeFollow sf = s.GetComponent<SlimeFollow>();
            if (sf != null) sf.enabled = false;

            SlimeCombat sc = s.GetComponent<SlimeCombat>() ?? s.AddComponent<SlimeCombat>();
            sc.enabled = true;
            sc.balaPrefab = balaPrefab;
            sc.ActivarAtaque(boss.transform);
        }
    }
}