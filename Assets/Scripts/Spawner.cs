using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemy;
    public Transform enemiesLayer;
    public int FPE, FPP;
    private int FPPshift;
    private int framec0, framec1;

    [Header("Enemy")]
    public float speed;
    public float detectionRadius, projectileSpeed;
    public int hitPoints, FPS, damage;

    public void Start()
    {
        
    }

    public void Update()
    {


        if (Game.instance.enemyCount < 10)
            FPPshift = 20;
        if (Game.instance.enemyCount < 7)
            FPPshift = 50;
        if (Game.instance.enemyCount < 3)
            FPPshift = 300;
        if (Game.instance.enemyCount > 10)
            FPPshift = 0;

        if (framec0 >= FPE)
        {
            GameObject obj = Instantiate(enemy, enemiesLayer);
            obj.transform.position = transform.position;
            Enemy protoype = obj.GetComponent<Enemy>();
            protoype.speed = speed;
            protoype.hitPoints = hitPoints;
            protoype.detectionRadius = detectionRadius;
            protoype.projectileSpeed = projectileSpeed;
            protoype.FPS = FPS;
            protoype.damage = damage;
            Game.instance.enemies.Add(protoype);
            
            framec0 = 0;
        }

        if(framec1 >= (FPP - FPPshift))
        {
            int rand = Random.Range(0, 7);

            switch (rand)
            {
                case 0:
                    speed++;
                    Game.instance.increaseEvolutionProgress();
                    break;
                case 1:
                    detectionRadius++;
                    Game.instance.increaseEvolutionProgress(0.005f);
                    break;
                case 2:
                    projectileSpeed += 3 * Game.instance.evolutionProgress;
                    Game.instance.increaseEvolutionProgress(0.0005f);
                    break;
                case 3:
                    hitPoints += (int)(40 * Game.instance.evolutionProgress);
                    Game.instance.increaseEvolutionProgress(0.0075f);
                    break;
                case 4:
                    FPS -= 2;
                    Game.instance.increaseEvolutionProgress(0.0075f);
                    break;
                case 5:
                    damage += 1 + (int)(10 * Game.instance.evolutionProgress);
                    Game.instance.increaseEvolutionProgress(0.0075f);
                    break;
                case 6:
                    FPE -= 7;
                    Game.instance.increaseEvolutionProgress(0.05f);
                    break;
            }

            FPE -= 2;

            if (FPE < 5)
                FPE = 5;

            framec1 = 0;
        }

        framec0++;
        framec1++;
    }


}
