using UnityEngine;

public class SlimePlacer : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SoltarSlime(1);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SoltarSlime(2);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SoltarSlime(3);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SoltarSlime(4);
        if (Input.GetKeyDown(KeyCode.Alpha5)) SoltarSlime(5);
    }

    void SoltarSlime(int indice)
    {
        // fila[0] es el Player, así que el primer slime es indice 1
        if (SlimeFollow.fila == null) return;
        if (indice <= 0 || indice >= SlimeFollow.fila.Count) return;

        GameObject sObj = SlimeFollow.fila[indice];
        if (sObj == null) return;

        // Desactiva follow del slime soltado
        SlimeFollow sf = sObj.GetComponent<SlimeFollow>();
        if (sf != null) sf.siguiendo = false;

        // Saca de la fila
        SlimeFollow.fila.RemoveAt(indice);

        // Reorganiza objetivos del resto
        for (int i = 1; i < SlimeFollow.fila.Count; i++)
        {
            SlimeFollow.fila[i].GetComponent<SlimeFollow>()
                .CambiarObjetivo(SlimeFollow.fila[i - 1].transform);
        }
    }
}
