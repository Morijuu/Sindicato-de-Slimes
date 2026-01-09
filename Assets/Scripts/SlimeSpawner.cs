using UnityEngine;

public class SlimeSpawner : MonoBehaviour
{
    public GameObject slimeNormalPrefab;
    public GameObject slimeRapidoPrefab;
    public GameObject slimePesadoPrefab;
    public GameObject slimeTanquePrefab;

    public int pNormal = 50;
    public int pRapido = 20;
    public int pPesado = 20;
    public int pTanque = 10;

    public int cantidadPorArea = 5;

    private bool spawned = false;

    void Start()
    {
        if (spawned) return;
        spawned = true;

        SpawnAreaRef[] areas = FindObjectsByType<SpawnAreaRef>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
        );

        foreach (SpawnAreaRef a in areas)
        {
            if (a.area == null) continue;

            for (int i = 0; i < cantidadPorArea; i++)
            {
                SpawnOne(a.area);
            }
        }
    }

    void SpawnOne(SpriteRenderer area)
    {
        Bounds b = area.bounds;

        float x = Random.Range(b.min.x, b.max.x);
        float y = Random.Range(b.min.y, b.max.y);
        Vector3 pos = new Vector3(x, y, 0f);

        int r = Random.Range(0, 100);
        GameObject prefab;

        int spawnChance;

        if (r < pNormal)
        {
            prefab = slimeNormalPrefab;
            spawnChance = pNormal;
        }
        else if (r < pNormal + pRapido)
        {
            prefab = slimeRapidoPrefab;
            spawnChance = pRapido;
        }
        else if (r < pNormal + pRapido + pPesado)
        {
            prefab = slimePesadoPrefab;
            spawnChance = pPesado;
        }
        else
        {
            prefab = slimeTanquePrefab;
            spawnChance = pTanque;
        }

        GameObject slime = Instantiate(prefab, pos, Quaternion.identity);

        // 🔑 INICIALIZAR IA
        SlimeWanderFlee ai = slime.GetComponent<SlimeWanderFlee>();
        if (ai != null)
        {
            ai.spawnArea = area;
            ai.spawnChance = spawnChance;
            ai.maxChance = GetMaxChance();
            // fearCenter y fearRadius se quedan como los tengas en el prefab
        }
    }
    int GetMaxChance()
    {
        int max = pNormal;
        if (pRapido > max) max = pRapido;
        if (pPesado > max) max = pPesado;
        if (pTanque > max) max = pTanque;
        return max;
    }


}
