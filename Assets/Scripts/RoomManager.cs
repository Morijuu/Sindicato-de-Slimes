using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager instance;
    private Room currentRoom;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void ChangeRoom(Room newRoom)
    {
        if (currentRoom != null)
            currentRoom.gameObject.SetActive(false);

        currentRoom = newRoom;
        currentRoom.gameObject.SetActive(true);

        CameraFollowRoom cam = FindAnyObjectByType<CameraFollowRoom>();
        cam.MoveToNewRoom(currentRoom.cameraPoint.position);
    }

    public Room GetCurrentRoom()
    {
        return currentRoom;
    }
}
