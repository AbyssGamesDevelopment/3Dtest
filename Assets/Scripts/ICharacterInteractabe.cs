using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterInteractabe
{
    void interact();

    int getState();

    int getCost();
}
