using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float movement = 5f;
    void Start()
    {

    }


    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += movement * Time.deltaTime * Vector3.up;
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.position += movement * Time.deltaTime * Vector3.down;
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.position += movement * Time.deltaTime * Vector3.left;
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.position += movement * Time.deltaTime * Vector3.right;
        }
    }
}