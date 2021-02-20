using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    void hit(int damage);

    int getCurrentHealth();

    int getMaxHealth();
}
