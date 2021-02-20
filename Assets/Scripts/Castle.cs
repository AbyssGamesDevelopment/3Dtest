using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Castle : MonoBehaviour, IDamagable, ICharacterInteractabe
{
    public static Castle instance;

    public float detectionRadius, projectileSpeed;
    public GameObject currentTarget;

    public int maxHitPoints, currentHitPoints, FPS, damage, healCost, healAmount, upgradeCost;
    private int framec;

    public void Start()
    {
        instance = this;

        currentHitPoints = maxHitPoints;

        Game.instance.playerObjects.Add(gameObject);
    }

    public void Update()
    {
        findTarget();

        if (framec == FPS)
        {
            if (currentTarget != null)
                shoot();

            framec = 0;
        }

        if(currentTarget != null)
            Debug.DrawRay(transform.position, currentTarget.transform.position - transform.position);

        framec++;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            hit(other.GetComponent<Projectile>().damage);
            Destroy(other.gameObject);
        }
    }

    public bool heal(int hitPoints)
    {
        if (currentHitPoints == maxHitPoints)
            return false;

        if (Game.instance.exchangeCoins(-healCost))
            currentHitPoints += hitPoints;
        else
            return false;

        Game.instance.createParticle(transform.position.x, transform.position.y + 5, 0);

        if (currentHitPoints > maxHitPoints)
            currentHitPoints = maxHitPoints;

        return true;
    }

    public void shoot()
    {
        if (Physics.Linecast(transform.position, currentTarget.transform.position, 9))
            return;

        GameObject obj = Instantiate(Game.instance.projectile, Game.instance.projectilesLayer);
        obj.transform.position = new Vector3(transform.position.x, transform.position.y + 4);
        obj.layer = 10;
        obj.tag = "Projectile";
        obj.GetComponent<Projectile>().target = currentTarget;
        obj.GetComponent<Projectile>().speed = projectileSpeed;
        obj.GetComponent<Projectile>().damage = damage;
        obj.GetComponent<Projectile>().isAutoTarget = true;
    }

    private void findTarget()
    {
        float distance = float.MaxValue;

        for (int i = 0; i < Game.instance.enemies.Count; i++)
        {
            float d = (Game.instance.enemies[i].transform.position - transform.position).magnitude;
            if (d < detectionRadius && d < distance)
            {
                distance = d;
                currentTarget = Game.instance.enemies[i].gameObject;
            }
        }
    }

    public int getCurrentHealth()
    {
        return currentHitPoints;
    }

    public int getMaxHealth()
    {
        return maxHitPoints;
    }

    private void die()
    {
        SceneManager.LoadScene(0);
    }

    public void interact()
    {
        bool isHealthFull = !heal(healAmount);

        if (isHealthFull && Game.instance.exchangeCoins(-upgradeCost))
            Character.instance.upgrade();
    }

    public int getState()
    {
        if (currentHitPoints < maxHitPoints)
            return 0;
        else
            return 1;
    }

    public int getCost()
    {
        if (currentHitPoints < maxHitPoints)
            return healCost;
        else
            return upgradeCost;
    }

    public void hit(int damage)
    {
        currentHitPoints -= damage;
        if (currentHitPoints <= 0)
            die();
    }

}
