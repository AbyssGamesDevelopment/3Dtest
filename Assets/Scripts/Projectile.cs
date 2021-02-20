using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private const int MAXLIFEFRAMES = 300;

    private Rigidbody rb;
    public GameObject target;

    public float speedX, speedY;
    public bool isAutoTarget;
    public float speed;
    public int damage;

    private int framec;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();

        Game.instance.incrementProjectileCount();

        float angle;

        if(target.transform.position.y > transform.position.y)
            angle = Vector3.Angle(target.transform.position - transform.position, Vector3.right);
        else
            angle = -Vector3.Angle(target.transform.position - transform.position, Vector3.right);

        speedX = speed * Mathf.Cos(angle * Mathf.Deg2Rad);
        speedY = speed * Mathf.Sin(angle * Mathf.Deg2Rad);
    }

    public void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        if (isAutoTarget)
        {
            Vector3 predicate = Vector3.MoveTowards(transform.position, target.transform.position, 1);

            float dx = predicate.x - transform.position.x;
            float dy = predicate.y - transform.position.y;
            float dz = predicate.z - transform.position.z;

            rb.velocity = new Vector3(dx * speed, dy * speed, dz * speed);
        }
        else
        {
            rb.velocity = new Vector3(speedX, speedY, 0);
        }

        if (framec > MAXLIFEFRAMES)
            Destroy(gameObject);

        framec++;
    }

    public void OnDestroy()
    {
        Game.instance.dicrementProjectileCount();
    }

}
