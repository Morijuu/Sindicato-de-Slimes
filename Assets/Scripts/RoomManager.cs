using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager instance;
    private Room currentRoom;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // 1. Buscamos todas las salas de la escena (incluso las desactivadas)
        Room[] allRooms = Resources.FindObjectsOfTypeAll<Room>();
        
        Room firstRoom = null;

        foreach (Room r in allRooms)
        {
            // Solo actuamos sobre objetos que están realmente en la jerarquía (no en los assets)
            if (r.gameObject.scene.name == null) continue; 

            // 2. Si es la Room_1, la guardamos. Si es otra, la desactivamos.
            if (r.gameObject.name == "Room_1" || r.gameObject.CompareTag("Room_1"))
            {
                firstRoom = r;
                r.gameObject.SetActive(true); 
            }
            else
            {
                r.gameObject.SetActive(false);
            }
        }

        // 3. Forzamos que el juego empiece en la Room_1
        if (firstRoom != null)
        {
            currentRoom = firstRoom;
            
            // Posicionamos la cámara inmediatamente (sin suavizado) al empezar
            CameraFollowRoom cam = FindAnyObjectByType<CameraFollowRoom>();
            if (cam != null && firstRoom.cameraPoint != null)
            {
                // Teletransporte inicial de la cámara para que no haya lag
                Vector3 startPos = firstRoom.cameraPoint.position;
                cam.transform.position = new Vector3(startPos.x, startPos.y, cam.transform.position.z);
                cam.MoveToNewRoom(startPos);
            }
        }
    }

    public void ChangeRoom(Room newRoom)
    {
        if (currentRoom != null)
            currentRoom.gameObject.SetActive(false);

        currentRoom = newRoom;
        currentRoom.gameObject.SetActive(true);

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