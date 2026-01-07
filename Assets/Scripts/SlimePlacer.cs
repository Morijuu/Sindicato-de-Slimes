using UnityEngine;

public class SlimePlacer : MonoBehaviour
{
    public GameObject balaPrefab;

    void Update()
    {
        // Detectar teclas 1 a 5
        if (Input.GetKeyDown(KeyCode.Alpha1)) SoltarSlime(1);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SoltarSlime(2);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SoltarSlime(3);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SoltarSlime(4);
        if (Input.GetKeyDown(KeyCode.Alpha5)) SoltarSlime(5);
    }

    void SoltarSlime(int indice)
    {
        // Usamos 'fila' en lugar de 'followers'
        if (SlimeFollow.fila.Count > indice)
        {
            GameObject slimeObj = SlimeFollow.fila[indice];
            SlimeFollow scriptFollow = slimeObj.GetComponent<SlimeFollow>();

            // 1. Usamos 'siguiendo' en lugar de 'isFollowing'
            scriptFollow.siguiendo = false;
            scriptFollow.enabled = false;

            // 2. Preparar combate
            SlimeCombat combat = slimeObj.GetComponent<SlimeCombat>();
            if (combat == null) combat = slimeObj.AddComponent<SlimeCombat>();
            combat.balaPrefab = balaPrefab;

            // 3. Sacar de la lista
            SlimeFollow.fila.RemoveAt(indice);

            // 4. Reorganizar la fila para que los de atrás sigan al nuevo objetivo
            for (int i = 1; i < SlimeFollow.fila.Count; i++)
            {
                SlimeFollow.fila[i].GetComponent<SlimeFollow>().CambiarObjetivo(SlimeFollow.fila[i-1].transform);
            }
            
            Debug.Log("Slime colocado. Quedan en fila: " + (SlimeFollow.fila.Count - 1));
        }
    }
}