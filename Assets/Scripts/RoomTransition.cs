using UnityEngine;

public class RoomTransition : MonoBehaviour
{
    [Header("Salas conectadas")]
    [SerializeField] private Room roomA;
    [SerializeField] private Room roomB;

    [Header("Spawn positions")]
    [SerializeField] private Vector2 spawnInRoomA;
    [SerializeField] private Vector2 spawnInRoomB;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Room currentRoom = RoomManager.instance.GetCurrentRoom();
        Room targetRoom;
        Vector2 spawnPos;

        if (currentRoom == roomA)
        {
            targetRoom = roomB;
            spawnPos = spawnInRoomB;
        }
        else
        {
            targetRoom = roomA;
            spawnPos = spawnInRoomA;
        }

        // 1. MOVER AL JUGADOR
        other.transform.position = spawnPos;
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;

        // 2. TELETRANSPORTAR SLIMES (Cambiado 'followers' por 'fila')
        // Empezamos en 1 porque el 0 es el Jugador
        for (int i = 1; i < SlimeFollow.fila.Count; i++)
        {
            GameObject slimeObj = SlimeFollow.fila[i];
            
            if (slimeObj != null)
            {
                slimeObj.transform.position = spawnPos;

                Rigidbody2D slimeRb = slimeObj.GetComponent<Rigidbody2D>();
                if (slimeRb != null) 
                {
                    slimeRb.linearVelocity = Vector2.zero;
                }
            }
        }

        // 3. CAMBIAR DE SALA Y CÁMARA
        RoomManager.instance.ChangeRoom(targetRoom);
    }
}