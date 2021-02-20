using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{
    private Rigidbody rb;
    public float speed, detectionRadius, projectileSpeed;
    public int FPS, damage, hitPoints, maxHealth;

    public Transform waypoints;
    public GameObject currentTarget;

    private int currentPointId;
    private Vector3[] points;

    private int framec;

    void Start()
    {
        Game.instance.incrementEnemyCount();

        rb = GetComponent<Rigidbody>();

        maxHealth = hitPoints;

        waypoints = Game.instance.waypoints;

        Transform t;

        if (transform.position.x < Castle.instance.transform.position.x)
        {
            if (Random.Range(0, 100) > 50)
                t = waypoints.Find("Way 0");
            else
                t = waypoints.Find("Way 1");
        }
        else
            t = waypoints.Find("Way 2");

        points = new Vector3[t.childCount];

        for (int i = 0; i < t.childCount - 1; i++)
        {
            points[i] = t.GetChild(i).position;
        }

        points[t.childCount - 1] = new Vector3(t.GetChild(t.childCount - 1).position.x + Random.Range(-2, 2), t.GetChild(t.childCount - 1).position.y, t.GetChild(t.childCount - 1).position.z);
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        moveToPlayerBaseUpdate();

        findTarget();

        if (framec == FPS)
        {
            if (currentTarget != null)
                shoot();

            framec = 0;
        }

        if (currentTarget != null)
        {
            if (currentTarget.transform.position.x > transform.position.x)
                turnRight();
            if (currentTarget.transform.position.x < transform.position.x)
                turnLeft();
            Debug.DrawRay(transform.position, currentTarget.transform.position - transform.position);
        }

        framec++;
    }

    public void OnDestroy()
    {
        Game.instance.enemies.Remove(this);
        Game.instance.dicrementEnemyCount();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            hit(other.GetComponent<Projectile>().damage);
            Destroy(other.gameObject);
        }
    }

    private void findTarget()
    {
        float distance = float.MaxValue;

        for (int i = 0; i < Game.instance.playerObjects.Count; i++)
        {
            float d = (Game.instance.playerObjects[i].transform.position - transform.position).magnitude;
            if (d < detectionRadius && d < distance)
            {
                distance = d;
                currentTarget = Game.instance.playerObjects[i].gameObject;
            }
        }
    }

    public void moveToPlayerBaseUpdate()
    {
        if (points == null)
            return;

        if (points[currentPointId].x > transform.position.x)
            moveRight();
        else
            moveLeft();

        if (Mathf.Abs(points[currentPointId].x - transform.position.x) < 1)
        {
            currentPointId++;
            if (currentPointId >= points.Length)
                points = null;
        }
    }

    public void moveRight()
    {
        rb.velocity = new Vector3(speed, rb.velocity.y);
        turnRight();
    }

    public void moveLeft()
    {
        rb.velocity = new Vector3(-speed, rb.velocity.y);
        turnLeft();
    }

    public void die()
    {
        Destroy(gameObject);
        Game.instance.exchangeCoins((int)Random.Range(1, 4 + (5 * Game.instance.evolutionProgress)));
        Game.instance.incrementKillsCount();
    }

    public void shoot()
    {
        if (Physics.Linecast(transform.position, currentTarget.transform.position, 9))
            return;

        GameObject obj = Instantiate(Game.instance.projectile, Game.instance.projectilesLayer);
        obj.transform.position = new Vector3(transform.position.x, transform.position.y + 1);
        obj.layer = 11;
        obj.tag = "Projectile";
        
        Projectile projectile = obj.GetComponent<Projectile>();
        projectile.target = currentTarget;
        projectile.speed = projectileSpeed;
        projectile.damage = damage;
        projectile.isAutoTarget = currentTarget.tag != "Player";
    }

    private void turnRight()
    {
        transform.rotation = Quaternion.Euler(0, -90, 0);
    }

    private void turnLeft()
    {
        transform.rotation = Quaternion.Euler(0, 90, 0);
    }

    public void hit(int damage)
    {
        hitPoints -= damage;
        if (hitPoints <= 0)
            die();
    }

    public int getCurrentHealth()
    {
        return hitPoints;
    }

    public int getMaxHealth()
    {
        return maxHealth;
    }
}
