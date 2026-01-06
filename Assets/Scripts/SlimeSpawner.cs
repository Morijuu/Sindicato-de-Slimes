using UnityEngine;

public class SlimeSpawner : MonoBehaviour
{
    public SpriteRenderer spawnArea;   // Área visible de spawn

    public GameObject slimeNormalPrefab;
    public GameObject slimeRapidoPrefab;
    public GameObject slimePesadoPrefab;
    public GameObject slimeTanquePrefab;

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

    void SpawnOne()
    {
        // Posición aleatoria dentro del sprite
        Bounds b = spawnArea.bounds; //convierte el rectangulo de spawn en coordenadas
        float x = Random.Range(b.min.x, b.max.x);
        float y = Random.Range(b.min.y, b.max.y);
        Vector3 pos = new Vector3(x, y, 0f);

        // Elegir tipo por probabilidad
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

        // Crear slime
        Instantiate(prefab, pos, Quaternion.identity);
    }
}
