using UnityEngine;

public class SlimeChargeToBoss : MonoBehaviour
{
    public Transform boss;
    public float chargeSpeed = 8f;
    public float hitDistance = 0.6f;

    private BossManager bossManager;

    public void Init(Transform bossTransform, BossManager manager, float speed)
    {
        boss = bossTransform;
        bossManager = manager;
        chargeSpeed = speed;
    }

    void Update()
    {
        if (boss == null)
        {
            // Si el boss ya no existe, desaparece el slime
            Destroy(gameObject);
            return;
        }

        // Mover hacia el boss
        transform.position = Vector3.MoveTowards(
            transform.position,
            boss.position,
            chargeSpeed * Time.deltaTime
        );

        // Si llega “lo suficiente cerca”, cuenta como impacto (sin depender de físicas)
        if (Vector2.Distance(transform.position, boss.position) <= hitDistance)
        {
            if (bossManager != null)
                bossManager.OnSlimeSacrificed();

            Destroy(gameObject);
        }
    }
}
