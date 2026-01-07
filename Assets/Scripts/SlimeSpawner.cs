using UnityEngine;

public class SlimeSpawner : MonoBehaviour
{
    public SpriteRenderer spawnArea;   // �rea visible de spawn

    public GameObject slimeNormalPrefab;
    public GameObject slimeRapidoPrefab;
    public GameObject slimePesadoPrefab;
    public GameObject slimeTanquePrefab;

    public Transform fearCenter; // arrastra aquí tu Empty del “radio”
    public float fearRadius = 3f;

    public int pNormal = 50;
    public int pRapido = 20;
    public int pPesado = 20;
    public int pTanque = 10;



    void Start()
    {
        // Genera 5 slimes al empezar
        for (int i = 0; i < 5; i++)
        {
            SpawnOne();
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


    void SpawnOne()
    {
        // 1) Calcular posición
        Bounds b = spawnArea.bounds;

        float x = Random.Range(b.min.x, b.max.x);
        float y = Random.Range(b.min.y, b.max.y);

        Vector3 pos = new Vector3(x, y, 0f);

        // 2) Elegir prefab por probabilidad
        int r = Random.Range(0, 100);
        GameObject prefab;

        if (r < pNormal)
            prefab = slimeNormalPrefab;
        else if (r < pNormal + pRapido)
            prefab = slimeRapidoPrefab;
        else if (r < pNormal + pRapido + pPesado)
            prefab = slimePesadoPrefab;
        else
            prefab = slimeTanquePrefab;

        GameObject slime = Instantiate(prefab, pos, Quaternion.identity);

        SlimeWanderFlee ai = slime.GetComponent<SlimeWanderFlee>();
        if (ai != null)
        {
            ai.spawnArea = spawnArea;
            ai.spawnChance = GetChanceForPrefab(prefab);
            ai.maxChance = GetMaxChance();
            ai.fearCenter = fearCenter;
            ai.fearRadius = fearRadius;
        }
    }

    int GetChanceForPrefab(GameObject prefab)
    {
        if (prefab == slimeNormalPrefab) return pNormal;                // Normal
        if (prefab == slimeRapidoPrefab) return pRapido;                // Rápido
        if (prefab == slimePesadoPrefab) return pPesado;                // Pesado
        return pTanque;                                                  // Tanque
    }

}
