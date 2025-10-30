using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // velocidad p�blica para modificar en el inspector
    private Rigidbody2D rb;  // referencia al Rigidbody2D
    private Vector2 movementInput;

    void Start()
    {
        // Obtener el componente Rigidbody2D del objeto
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Leer la entrada del jugador (WASD o flechas)
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // Crear el vector de movimiento
        movementInput = new Vector2(moveX, moveY).normalized;
    }

    void FixedUpdate()
    {
        // Aplicar el movimiento usando el Rigidbody2D (en FixedUpdate para f�sica)
        rb.linearVelocity = movementInput * speed;
    }
}