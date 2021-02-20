using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public static Game instance;
    public static float x;

    public GameObject projectile, tower, particle;
    public Transform waypoints, projectilesLayer, towersLayer, otherLayer;

    public Text coinsCountText, enemyCountText, projectileCountText, currentActionCostText, killsCountText, evolutionProgressText; 
    public SpriteChanger multibuttonIconController;
    public GameObject multibuttonForeground;

    public List<Enemy> enemies;
    public List<GameObject> playerObjects;

    public int coins, enemyCount, projectileCount, currentActionCost, killsCount;
    public float evolutionProgress;
    private int _lastFrameActionCost;

    public void Start()
    {
        instance = this;
        enemies = new List<Enemy>();
        playerObjects = new List<GameObject>();
        exchangeCoins(50);

        multibuttonIconController.Start();
        multibuttonIconController.set(0);

        Game.instance.playerObjects.Add(gameObject);

        Application.targetFrameRate = 60;
    }

    public void Update()
    {
        updateInput();
        updateMultibuttonIcon();
        updateActionCostText();
        updateKillsCountText();
        updateEvolutionProgressText();

        multibuttonForeground.SetActive(currentActionCost > coins||!Character.instance.isOnGround);
    }

    private void updateInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
            healCastle();

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    public void incrementEnemyCount()
    {
        enemyCount++;
        enemyCountText.text = "Enemies: " + enemyCount.ToString();
    }

    public void dicrementEnemyCount()
    {
        enemyCount--;
        enemyCountText.text = "Enemies: " + enemyCount.ToString();
    }

    public void incrementProjectileCount()
    {
        projectileCount++;
        projectileCountText.text = "Projectiles: " + projectileCount.ToString();
    }

    public void dicrementProjectileCount()
    {
        projectileCount--;
        projectileCountText.text = "Projectiles: " + projectileCount.ToString();
    }

    public bool exchangeCoins(int count)
    {
        if (coins + count < 0)
            return false;

        coins += count;
        coinsCountText.text = coins.ToString();

        return true;
    }

    public void healCastle()
    {
        if (coins < 10)
            return;

        bool isHealed = Castle.instance.heal(25);

        if (isHealed)
        {
            exchangeCoins(-10);
        }
    }

    public void buyTower()
    {
        Tile tile = Character.instance.getUnderPlayerTile();

        if (coins < Tower.COST || !Character.instance.isOnGround || tile == null)
            return;

        int X = tile.X + 1;
        int Y = tile.Y + 1;

        bool isTowerPlaced = Tilemap.instance.setTower(X, Y);

        if (isTowerPlaced)
        {
            exchangeCoins(-Tower.COST);

            Vector3 pos = Tilemap.tiles[X, Y].installedObject.transform.position;

            createParticle(pos.x, pos.y + 3f, 2, 0);
        }
    }

    public void moveRight()
    {
        x = 1;
    }

    public void moveLeft()
    {
        x = -1;
    }

    public void moveReset()
    {
        x = 0;
    }

    public void updateMultibuttonIcon()
    {
        ICharacterInteractabe obj = Character.instance.currentInteactableObject;

        if (obj== null)
        {
            multibuttonIconController.set(0);
            return;
        }

        int state = obj.getState();

        switch (state)
        {
            case 0:
                multibuttonIconController.set(1);
                break;
            case 1:
                multibuttonIconController.set(2);
                break;
        }
    }

    public void updateActionCostText()
    {
        if (_lastFrameActionCost != currentActionCost) 
            currentActionCostText.text = currentActionCost.ToString();
        
        _lastFrameActionCost = currentActionCost;
    }

    public void incrementKillsCount()
    {
        killsCount++;
        killsCountText.text = killsCount.ToString();
    }

    public void increaseEvolutionProgress(float value = 0.001f)
    {
        evolutionProgress += value;
        evolutionProgress = (float) System.Math.Round(evolutionProgress, 2);
        evolutionProgressText.text = evolutionProgress.ToString() + "%";
    }

    public void updateEvolutionProgressText()
    {
        evolutionProgressText.text = evolutionProgress.ToString() + "%";
    }

    public void updateKillsCountText()
    {
        killsCountText.text = killsCount.ToString();
    }

    public void createParticle(float x, float y, int id, float spread = 0.5f)
    {
        GameObject obj = Instantiate(particle, otherLayer);
        obj.transform.position = new Vector3(x + Random.Range(-spread, spread), y, 1.5f);
        obj.GetComponent<Particle>().setSprite(id);
    }
}
