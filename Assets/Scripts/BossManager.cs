using UnityEngine;
using System.Collections;

public class BossManager : MonoBehaviour
{
    public float tiempoPreparacion = 60f;
    public GameObject bossPrefab;
    public Transform spawnPoint;
    public GameObject balaPrefab;

    IEnumerator Start()
    {
        Debug.Log("Preparación: 60 segundos...");
        yield return new WaitForSeconds(tiempoPreparacion);

        GameObject bossObj = Instantiate(bossPrefab, spawnPoint.position, Quaternion.identity);

        // 1. Activar los slimes que ya soltaste manualmente
        SlimeCombat[] sueltos = Object.FindObjectsByType<SlimeCombat>(FindObjectsSortMode.None);
        foreach (SlimeCombat s in sueltos) 
        {
            s.ActivarAtaque(bossObj.transform);
        }

        // 2. Soltar automáticamente a los que sigan en la fila
        // Recorremos desde el final hacia atrás para no romper el índice
        for (int i = SlimeFollow.fila.Count - 1; i >= 1; i--)
        {
            GameObject t = SlimeFollow.fila[i];
            if (t != null)
            {
                SlimeFollow sf = t.GetComponent<SlimeFollow>();
                if(sf != null) sf.siguiendo = false;

                SlimeCombat sc = t.GetComponent<SlimeCombat>();
                if (sc == null) sc = t.AddComponent<SlimeCombat>();
                
                sc.balaPrefab = balaPrefab;
                sc.ActivarAtaque(bossObj.transform);
                
                SlimeFollow.fila.RemoveAt(i);
            }
        }
    }
}