using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private IDamagable obj;
    private Transform background, indicator;
    private int max;
    private float startScale;

    public void Start()
    {
        obj = GetComponentInParent<IDamagable>();
        background = transform.GetChild(0);
        indicator = transform.GetChild(1);

        startScale = indicator.localScale.x;
        max = obj.getMaxHealth();
    }

    public void Update()
    {
        indicator.localScale = new Vector3(startScale * ((float)obj.getCurrentHealth()/(float)obj.getMaxHealth()), indicator.localScale.y, indicator.localScale.z); ;
    }
}
