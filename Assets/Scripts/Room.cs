using UnityEngine;

public class Room : MonoBehaviour
{
    public Transform cameraPoint;

    private void Awake()
    {
        gameObject.SetActive(false);
    }
}
