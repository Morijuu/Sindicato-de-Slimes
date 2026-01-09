using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class RoomTransition : MonoBehaviour
{
    [Header("Salas conectadas por ESTA puerta")]
    public Room roomA;
    public Room roomB;

    [Header("Teleport")]
    public float exitOffset = 0.6f;
    public float insideMargin = 0.1f;

    private Collider2D doorCol;

    private void Awake()
    {
        doorCol = GetComponent<Collider2D>();
        doorCol.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (RoomManager.instance == null)
        {
            Debug.LogError("RoomTransition: No existe RoomManager en la escena.");
            return;
        }

        if (roomA == null || roomB == null)
        {
            Debug.LogError("RoomTransition: Asigna roomA y roomB en el Inspector.");
            return;
        }

        // 1) Elegir sala destino según sala actual
        Room current = RoomManager.instance.GetCurrentRoom();
        Room targetRoom = (current == roomA) ? roomB : roomA;

        // 2) Cambiar sala (esto mueve la cámara)
        RoomManager.instance.ChangeRoom(targetRoom);

        // 3) Teleport al lado opuesto del rectángulo de la puerta
        Bounds b = doorCol.bounds;
        Vector3 c = b.center;
        Vector3 p = other.transform.position;

        float playerHalfX = 0f, playerHalfY = 0f;
        Collider2D playerCol = other.GetComponent<Collider2D>();
        if (playerCol != null)
        {
            playerHalfX = playerCol.bounds.extents.x;
            playerHalfY = playerCol.bounds.extents.y;
        }

        float nx = (b.extents.x > 0f) ? (p.x - c.x) / b.extents.x : 0f;
        float ny = (b.extents.y > 0f) ? (p.y - c.y) / b.extents.y : 0f;

        Vector3 target = p;

        if (Mathf.Abs(nx) > Mathf.Abs(ny))
        {
            float side = Mathf.Sign(p.x - c.x);
            if (side == 0f) side = 1f;

            float push = b.extents.x + exitOffset + playerHalfX;
            target.x = c.x - side * push;

            float minY = b.min.y + insideMargin + playerHalfY;
            float maxY = b.max.y - insideMargin - playerHalfY;
            target.y = Mathf.Clamp(p.y, minY, maxY);
        }
        else
        {
            float side = Mathf.Sign(p.y - c.y);
            if (side == 0f) side = 1f;

            float push = b.extents.y + exitOffset + playerHalfY;
            target.y = c.y - side * push;

            float minX = b.min.x + insideMargin + playerHalfX;
            float maxX = b.max.x - insideMargin - playerHalfX;
            target.x = Mathf.Clamp(p.x, minX, maxX);
        }

        other.transform.position = new Vector3(target.x, target.y, other.transform.position.z);

        Rigidbody2D rb = other.attachedRigidbody;
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }
}
