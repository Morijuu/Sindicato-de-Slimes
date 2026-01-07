using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; 
    private Rigidbody2D rb;  
    private Vector2 movementInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Pasa la posición del ratón de pantalla a coordenadas del mundo
        mouseWorld.z = 0f;                                                      // Fija la posición en el plano 2D (eje Z a 0)

        Vector3 dir = mouseWorld - transform.position;                          // Vector dirección desde el jugador hacia el ratón

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;                // Calcula el ángulo en grados hacia el ratón

        transform.rotation = Quaternion.Euler(0f, 0f, angle);                   // Rota el jugador en el eje Z para que apunte al ratón


        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        movementInput = new Vector2(moveX, moveY).normalized;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movementInput * speed;
    }
}