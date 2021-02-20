using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, IDamagable
{
    public static Character instance;

    private Rigidbody rb;
    public GameObject currentTarget;
    public ICharacterInteractabe currentInteactableObject;

    public bool isOnGround;
    public float speed, jumpForce, detectionRadius, projectileSpeed;
    public int maxHitPoints, currentHitPoints, FPS, damage;

    private int framec;

    public void Start()
    {
        instance = this;
        rb = GetComponent<Rigidbody>();

        currentHitPoints = maxHitPoints;

        Game.instance.playerObjects.Add(gameObject);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            jump();
        }

        findTarget();

        updateCurrentInteractableObject();

        if (framec >= FPS)
        {
            if(currentTarget != null)
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
        else
        {
            if (Input.GetAxis("Horizontal") + Game.x > 0)
                turnRight();
            if (Input.GetAxis("Horizontal") + Game.x < 0)
                turnLeft();
        }

        if (transform.position.y < -1)
            die();

        isOnGround = Physics.Linecast(transform.position, new Vector3(transform.position.x, transform.position.y - 2, transform.position.z), 9);

        framec++;
    }

    public void FixedUpdate()
    {
        rb.velocity = new Vector3((Input.GetAxis("Horizontal") + Game.x) * speed, rb.velocity.y);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            hit(other.GetComponent<Projectile>().damage);
            Destroy(other.gameObject);
        }
 }

    public void updateCurrentInteractableObject()
    {
        RaycastHit hit;

        Physics.Linecast(transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z + 3), out hit);

        if (hit.collider == null)
        {
            currentInteactableObject = null;
            Game.instance.currentActionCost = Tower.COST;
        }
        else if (hit.collider.CompareTag("PlayerInteractable"))
        {
            currentInteactableObject = hit.collider.GetComponent<ICharacterInteractabe>();
            Game.instance.currentActionCost = currentInteactableObject.getCost();
        }
    }

    public Tile getUnderPlayerTile()
    {
        RaycastHit hit;
       
        Physics.Linecast(transform.position, new Vector3(transform.position.x, transform.position.y - 2, transform.position.z), out hit, 9);

        return hit.collider == null ? null : hit.collider.transform.GetComponent<Tile>();
    }

    private void turnRight()
    {
        transform.rotation = Quaternion.Euler(0, -90, 0);
    }

    private void turnLeft()
    {
        transform.rotation = Quaternion.Euler(0, 90, 0);
    }

    private void findTarget()
    {
        float distance = float.MaxValue;

        for (int i = 0; i < Game.instance.enemies.Count; i++)
        {
            float d = (Game.instance.enemies[i].transform.position - transform.position).magnitude;
            if (d < detectionRadius&&d < distance)
            {
                distance = d;
                currentTarget = Game.instance.enemies[i].gameObject;
            }
        }
    }

    public void jump()
    { 
        if (isOnGround)
            rb.AddForce(new Vector3(0, jumpForce, 0));
    }

    public void shoot()
    {
        if (Physics.Linecast(transform.position, currentTarget.transform.position, 9))
            return;

        GameObject obj = Instantiate(Game.instance.projectile, Game.instance.projectilesLayer);
        obj.transform.position = new Vector3(transform.position.x, transform.position.y + 1);
        obj.layer = 10;
        obj.tag = "Projectile";
        
        Projectile projectile = obj.GetComponent<Projectile>();
        projectile.target = currentTarget;
        projectile.speed = projectileSpeed;
        projectile.damage = damage;
        projectile.isAutoTarget = true;
    }

    private void die()
    {
        transform.position = Castle.instance.transform.position;
        currentHitPoints = maxHitPoints;
    }

    public void hit(int damage)
    {
        currentHitPoints -= damage;
        if (currentHitPoints <= 0)
            die();
    }

    public int getCurrentHealth()
    {
        return currentHitPoints;
    }

    public int getMaxHealth()
    {
        return maxHitPoints;
    }

    public void upgrade()
    {
        detectionRadius += 2;
        projectileSpeed += 2;
        maxHitPoints += 30;
        FPS -= 1;
        damage += 1;

        if (FPS < 10)
            FPS = 10;

        Castle.instance.upgradeCost += 15;

        Game.instance.createParticle(transform.position.x, transform.position.y + 2.5f, 1);
    }

    public void contextedInteraction()
    {
        if (currentInteactableObject == null)
            Game.instance.buyTower();
        else
            currentInteactableObject.interact();
    }
}
