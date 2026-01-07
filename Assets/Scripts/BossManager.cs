using UnityEngine;
using System.Collections;

public class BossManager : MonoBehaviour
{
    public GameObject bossPrefab;
    public Transform spawnPoint;
    public GameObject balaPrefab;
    
    private bool bossYaSalio = false;

    void Update()
    {
        // Detecta si pulsas la B y si no ha salido ya
        if (Input.GetKeyDown(KeyCode.B) && !bossYaSalio)
        {
            // Opcional: Verificar si el jugador está en la Room 5
            // Si tu RoomManager es accesible, podrías poner: 
            // if (RoomManager.instance.GetCurrentRoom().name == "Room_5")
            
            EmpezarCombate();
        }
    }

    void EmpezarCombate()
    {
        bossYaSalio = true;
        Debug.Log("¡Tecla B pulsada! Spawneando Boss inmediatamente...");

        if (bossPrefab == null || spawnPoint == null)
        {
            Debug.LogError("Faltan referencias en el BossManager (Prefab o SpawnPoint)");
            return;
        }

        // 1. Spawnea al Boss
        GameObject bossObj = Instantiate(bossPrefab, spawnPoint.position, Quaternion.identity);
        bossObj.tag = "Boss";

        // 2. Activa su IA
        BossAI ai = bossObj.GetComponent<BossAI>();
        if (ai != null) ai.ActivarBoss();

        // 3. Ordena a los Slimes que ataquen
        ActivarAtaqueSlimes(bossObj.transform);
    }

    void ActivarAtaqueSlimes(Transform bossTransform)
    {
        // Slimes sueltos en el escenario
        SlimeCombat[] todos = Object.FindObjectsByType<SlimeCombat>(FindObjectsSortMode.None);
        foreach (SlimeCombat s in todos)
        {
            s.ActivarAtaque(bossTransform);
        }

        // Slimes que todavía están en la fila (se sueltan y atacan)
        for (int i = SlimeFollow.fila.Count - 1; i >= 1; i--)
        {
            GameObject sObj = SlimeFollow.fila[i];
            if (sObj != null)
            {
                sObj.GetComponent<SlimeFollow>().siguiendo = false;
                SlimeCombat sc = sObj.GetComponent<SlimeCombat>() ?? sObj.AddComponent<SlimeCombat>();
                sc.balaPrefab = balaPrefab;
                sc.ActivarAtaque(bossTransform);
                SlimeFollow.fila.RemoveAt(i);
            }
        }
    }
}