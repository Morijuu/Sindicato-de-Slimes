using UnityEngine;

public class SpawnAreaRef : MonoBehaviour
{
    public SpriteRenderer area;

    void Reset()
    {
        area = GetComponent<SpriteRenderer>();
    }
}
