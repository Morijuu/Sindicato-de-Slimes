using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager instance;

    [Header("Starting room (manual)")]
    public Room startingRoom;

    private Room currentRoom;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Buscar todas las salas (incluye desactivadas)
        Room[] rooms = FindObjectsByType<Room>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        if (rooms == null || rooms.Length == 0)
        {
            Debug.LogError("RoomManager: No hay Rooms en la escena.");
            return;
        }

        // Si no asignas startingRoom, usa la primera encontrada
        if (startingRoom == null)
        {
            startingRoom = rooms[0];
            Debug.LogWarning("RoomManager: startingRoom no asignada. Usando la primera Room encontrada.");
        }

        // Apagar todas
        foreach (Room r in rooms)
            r.gameObject.SetActive(false);

        // Encender solo la inicial
        startingRoom.gameObject.SetActive(true);
        currentRoom = startingRoom;

        // Colocar cámara en la sala inicial
        CameraFollowRoom cam = FindAnyObjectByType<CameraFollowRoom>();
        if (cam != null && currentRoom.cameraPoint != null)
        {
            Vector3 p = currentRoom.cameraPoint.position;
            cam.transform.position = new Vector3(p.x, p.y, cam.transform.position.z);
            cam.MoveToNewRoom(p);
        }
    }

    public void ChangeRoom(Room newRoom)
    {

        Debug.Log("ChangeRoom -> " + newRoom.name);
        Debug.Log("cameraPoint -> " + (newRoom.cameraPoint == null ? "NULL" : newRoom.cameraPoint.name));

        if (newRoom == null)
        {
            Debug.LogError("RoomManager.ChangeRoom: newRoom es NULL.");
            return;
        }

        if (currentRoom == newRoom) return;

        if (currentRoom != null)
            currentRoom.gameObject.SetActive(false);

        currentRoom = newRoom;
        currentRoom.gameObject.SetActive(true);

        // mover cámara al cameraPoint de la nueva sala
        CameraFollowRoom cam = FindAnyObjectByType<CameraFollowRoom>();
        if (cam != null && currentRoom.cameraPoint != null)
        {
            cam.MoveToNewRoom(currentRoom.cameraPoint.position);
        }
    }

    public Room GetCurrentRoom()
    {
        return currentRoom;
    }
}
