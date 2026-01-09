using UnityEngine;

public class BossManager : MonoBehaviour
{
    public GameObject bossPrefab;
    public Transform spawnPoint;

    private GameObject bossInstance;
    private bool bossYaSalio = false;

    private int slimesPorSacrificar = 0;
    private int slimesSacrificados = 0;

    void Update()
    {
        // Spawn boss con B (solo una vez)
        if (Input.GetKeyDown(KeyCode.B) && !bossYaSalio)
        {
            SpawnBoss();
            LanzarSlimesAlBoss();
        }
    }

    void SpawnBoss()
    {
        if (bossPrefab == null || spawnPoint == null)
        {
            Debug.LogError("BossManager: Asigna bossPrefab y spawnPoint.");
            return;
        }

        bossInstance = Instantiate(bossPrefab, spawnPoint.position, Quaternion.identity);
        bossYaSalio = true;
        Debug.Log("Boss apareció.");
    }

    void LanzarSlimesAlBoss()
    {
        if (bossInstance == null)
        {
            Debug.LogError("BossManager: No existe bossInstance.");
            return;
        }

        // cantidad de slimes que te siguen (fila[0] es el Player)
        slimesPorSacrificar = Mathf.Max(0, SlimeFollow.fila.Count - 1);
        slimesSacrificados = 0;

        if (slimesPorSacrificar == 0)
        {
            Debug.Log("No tienes slimes. Boss muere igualmente (según regla).");
            MatarBoss();
            return;
        }

        // De atrás hacia delante, soltar y lanzar
        for (int i = SlimeFollow.fila.Count - 1; i >= 1; i--)
        {
            GameObject slimeObj = SlimeFollow.fila[i];
            if (slimeObj == null) continue;

            // 1) quitar de la fila
            SlimeFollow.fila.RemoveAt(i);

            // 2) desactivar scripts que puedan interferir
            SlimeFollow sf = slimeObj.GetComponent<SlimeFollow>();
            if (sf != null) sf.enabled = false;

            SlimeWanderFlee wf = slimeObj.GetComponent<SlimeWanderFlee>();
            if (wf != null) wf.enabled = false;

            SlimeFlee ff = slimeObj.GetComponent<SlimeFlee>();
            if (ff != null) ff.enabled = false;

            SlimeCombat sc = slimeObj.GetComponent<SlimeCombat>();
            if (sc != null) sc.enabled = false;

            // 3) añadir carga al boss
            SlimeChargeToBoss charge = slimeObj.GetComponent<SlimeChargeToBoss>();
            if (charge == null) charge = slimeObj.AddComponent<SlimeChargeToBoss>();

            // velocidad opcional basada en tipo (si no hay stats, usa 8)
            float speed = 8f; // todos cargan igual


            charge.Init(bossInstance.transform, this, speed);

        }

        Debug.Log("Slimes lanzados al boss: " + slimesPorSacrificar);
    }

    public void OnSlimeSacrificed()
    {
        slimesSacrificados++;

        // Cuando se hayan sacrificado todos, muere el boss
        if (slimesSacrificados >= slimesPorSacrificar)
        {
            Debug.Log("Todos los slimes se han sacrificado. Boss muere.");
            MatarBoss();
        }
    }

    void MatarBoss()
    {
        if (bossInstance != null)
        {
            Destroy(bossInstance);
            bossInstance = null;
        }
    }
}
