using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class InEditorGrid : MonoBehaviour
{

    public void Start()
    {

    }

    public void Update()
    {
       for (int i = 0; i < transform.childCount; i++)
        {
            Transform obj = transform.GetChild(i);
            obj.position = new Vector3(round(obj.position.x), round(obj.position.y), 0);
        }
    }

    private float round(float value)
    {
        float a = value / Tilemap.CELLSIZE;
        a = Mathf.RoundToInt(a) * Tilemap.CELLSIZE;
        return a;
    }
}
