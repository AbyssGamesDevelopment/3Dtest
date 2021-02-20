using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour, IDamagable, ICharacterInteractabe
{
    public const int COST = 25;

    public GameObject currentTarget;

    public float detectionRadius, projectileSpeed;
    public int maxHitPoints, currentHitPoints, FPS, damage, healAmount, upgradeCost, currentUpgradeLevel, healCost;
   
    private int framec;

    public Upgrade[] upgrades = new Upgrade[] {
        new Upgrade(10, 20, 50, 20, 60, 4, 15),
        new Upgrade(12, 25, 90, 30, 50, 5, 25),
        new Upgrade(15, 30, 150, 70, 45, 7, 30),
        new Upgrade(25, 35, 200, 110, 30, 8, 45),
        new Upgrade(35, 40, 300, 175, 10, 10, 80),
        new Upgrade(40, 45, 500, 200, 9, 12, 100),
        new Upgrade(42, 47, 750, 250, 8, 13, 150),
        new Upgrade(44, 48, 1000, 275, 7, 14, 200),
        new Upgrade(45, 49, 1750, 400, 7, 17, 300),
        new Upgrade(50, 50, 3000, 1000, 5, 20, -1)
    };

    public void Start()
    {
        Game.instance.playerObjects.Add(gameObject);
        
        upgrade(true);

        currentHitPoints = maxHitPoints;
    }

    public void Update()
    {
        findTarget();

        if (framec >= FPS)
        {
            if (currentTarget != null)
                shoot();

            framec = 0;
        }

        if (currentTarget != null)
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

    private void OnDestroy()
    {
        Game.instance.playerObjects.Remove(gameObject);
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

    public void shoot()
    {
        if (Physics.Linecast(transform.position, currentTarget.transform.position, 9))
            return;

        GameObject obj = Instantiate(Game.instance.projectile, Game.instance.projectilesLayer);
        obj.transform.position = new Vector3(transform.position.x, transform.position.y + 2.5f);
        obj.layer = 10;
        obj.tag = "Projectile";
        
        Projectile projectile = obj.GetComponent<Projectile>();
        projectile.target = currentTarget;
        projectile.speed = projectileSpeed;
        projectile.damage = damage;
        projectile.isAutoTarget = true;
    }

    public void upgrade(bool isInitUpgrade = false)
    {
        if (currentUpgradeLevel > upgrades.Length)
            return;

        Upgrade upgrade = upgrades[currentUpgradeLevel];

        this.detectionRadius = upgrade.detectionRadius;
        this.projectileSpeed = upgrade.projectileSpeed;
        this.maxHitPoints = upgrade.maxHitPoints;
        this.healAmount = upgrade.healAmount;
        this.FPS = upgrade.FPS;
        this.damage = upgrade.damage;
        this.upgradeCost = upgrade.upgradeCost;

        currentUpgradeLevel++;

        if(!isInitUpgrade)
            Game.instance.createParticle(transform.position.x, transform.position.y + 3, 1);

        transform.GetChild(3).GetChild(0).GetComponent<Text>().text = currentUpgradeLevel.ToString();
    }

    public bool heal(int hitPoints)
    {
        if (currentHitPoints == maxHitPoints)
            return false;

        currentHitPoints += hitPoints;

        if (currentHitPoints > maxHitPoints)
            currentHitPoints = maxHitPoints;

        Game.instance.createParticle(transform.position.x, transform.position.y + 3, 0);

        return true;
    }

    private void die()
    {
        Destroy(gameObject);
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

    public void interact()
    {
        if (currentHitPoints == maxHitPoints&&upgradeCost > 0&&Game.instance.exchangeCoins(-upgradeCost))
            upgrade();

        if (currentHitPoints < maxHitPoints&&Game.instance.exchangeCoins(-healCost))
            heal(healAmount);
    }

    public int getState()
    {
        if (currentHitPoints < maxHitPoints)
            return 0;
        
        if (currentUpgradeLevel < upgrades.Length)
            return 1;
        
        return 2;
    }

    public int getCost()
    {
        if (currentHitPoints < maxHitPoints)
            return healCost;
        if (currentUpgradeLevel < upgrades.Length)
            return upgradeCost;
        else
            return healCost;
    }

    public class Upgrade
    {
        public float detectionRadius, projectileSpeed;
        public int maxHitPoints, healAmount, FPS, damage, upgradeCost;

        public Upgrade(float detectionRadius, float projectileSpeed, int maxHitPoints, int healAmount, int FPS, int damage, int upgradeCost)
        {
            this.detectionRadius = detectionRadius;
            this.projectileSpeed = projectileSpeed;
            this.maxHitPoints = maxHitPoints;
            this.healAmount = healAmount;
            this.FPS = FPS;
            this.damage = damage;
            this.upgradeCost = upgradeCost;;
        }
    }
}
