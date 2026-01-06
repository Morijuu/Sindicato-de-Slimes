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

        // mover player
        other.transform.position = spawnPos;
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;

        // mover slimes seguidores
        for (int i = 1; i < SlimeFollow.followers.Count; i++)
        {
            Transform slime = SlimeFollow.followers[i];
            slime.position = spawnPos;

            Rigidbody2D slimeRb = slime.GetComponent<Rigidbody2D>();
            if (slimeRb != null) slimeRb.linearVelocity = Vector2.zero;
        }

        RoomManager.instance.ChangeRoom(targetRoom);
    }
}
