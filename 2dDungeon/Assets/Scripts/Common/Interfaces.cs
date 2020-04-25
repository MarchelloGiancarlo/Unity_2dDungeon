using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void receivedDamage(int damage);
}
public interface IPushable
{
    void receivedPush(Vector2 vector);
}
public interface IChildOnTriggerEnter
{
    void ChildOnTriggerEnter(GameObject child, Collider2D other);
}
public interface IUnitRadius
{
    float getUnitRadius();
}