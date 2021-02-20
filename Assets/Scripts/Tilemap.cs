using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tilemap : MonoBehaviour
{
    public const int MAPSIZEX = 128;
    public const int MAPSIZEY = 128;

    public const float CELLSIZE = 1.9f;

    public static Tilemap instance;
    public static Tile[,] tiles; 

    public GameObject ground;
    public Transform groundLayer;

    public void Start()
    {
        instance = this;
        tiles = new Tile[MAPSIZEX, MAPSIZEY];

        init();
    }

    private void init()
    {
        for (int i = 0; i < groundLayer.childCount; i++)
        {
            int X = (int)(groundLayer.GetChild(i).position.x / CELLSIZE);
            int Y = (int)(groundLayer.GetChild(i).position.y / CELLSIZE);

            tiles[X, Y] = groundLayer.GetChild(i).gameObject.AddComponent<Tile>();
            tiles[X, Y].installedObject = groundLayer.GetChild(i).gameObject;
            tiles[X, Y].X = X;
            tiles[X, Y].Y = Y;
        }
    }

    public bool setTower(int X, int Y)
    {
        if (tiles[X, Y] == null)
        {
            GameObject obj = Instantiate(Game.instance.tower, Game.instance.towersLayer);
            obj.transform.position = new Vector3(X * CELLSIZE, Y * CELLSIZE + 1f, 2f);
            tiles[X, Y] = obj.gameObject.AddComponent<Tile>();
            tiles[X, Y].installedObject = obj.gameObject;
            tiles[X, Y].X = X;
            tiles[X, Y].Y = Y;

            return true;
        }
        else 
            return false;
    }

    public void setGround(int X, int Y)
    {
        if (tiles[X, Y].installedObject != null)
            return;

        GameObject obj = Instantiate(ground, groundLayer);
        obj.transform.position = new Vector3(X * CELLSIZE, Y * CELLSIZE, 0);

        tiles[X, Y].installedObject = obj;
    }

    public void Update()
    {
        
    }
}
