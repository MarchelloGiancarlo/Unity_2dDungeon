using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This interface must be implemented by all the weapon on the game
public interface IWeapon
{
    //This method attivate the weapon attack and return true if the weapon was ready to attack
    bool Attack(Vector2 position);
    //This method return true if the weapon is ready to attack
    bool isReadyToAttack();
    float getWeaponRange();
}
