using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletModule : MonoBehaviour
{
    private int damage = 0;
    private int pushBackForce = 0;
    private bool isPropertiesSetted = false;
    private Vector2 prevPosition;
    public bool destroyOnWall = true;
    private void Awake()
    {
        prevPosition = transform.position;
    }
    private void Update()
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position,
                            ((Vector2)transform.position - prevPosition),
                            ((Vector2)transform.position - prevPosition).magnitude);

        prevPosition = transform.position;
        if (hit.collider == null)
            return;
        Collider2D other = hit.collider;
        // Debug.Log(other.gameObject);
        if (other.GetComponent<IWeapon>() != null)
            return;
        if (other.GetComponent<BulletModule>() != null)
            return;

        if (Utils.Tag.isOppositeSite(gameObject, other.gameObject))
        {
            if (other.gameObject.GetComponent<IDamageable>() != null)
            {
                other.gameObject.GetComponent<IDamageable>().receivedDamage(damage);
            }
            if (other.gameObject.GetComponent<IPushable>() != null)
            {
                Vector2 direction = (other.transform.position - transform.position).normalized;
                other.gameObject.GetComponent<IPushable>().receivedPush(direction * pushBackForce);
            }
            if (!isPropertiesSetted)
                Debug.LogWarning("Bullet hitted without before setted damage");
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "Wall")
        {
            if (destroyOnWall)
                Destroy(gameObject);
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<BulletModule>().enabled = false;
        }
    }
    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.GetComponent<WeaponModule>() != null)
    //         return;
    //     if (other.GetComponent<BulletModule>() != null)
    //         return;

    //     if (Utils.Tag.isOppositeSite(gameObject, other.gameObject))
    //     {
    //         if (other.gameObject.GetComponent<IDamageable>() != null)
    //         {
    //             other.gameObject.GetComponent<IDamageable>().receivedDamage(damage);
    //         }
    //         if (other.gameObject.GetComponent<IPushable>() != null)
    //         {
    //             Vector2 direction = (other.transform.position - transform.position).normalized;
    //             other.gameObject.GetComponent<IPushable>().receivedPush(direction * pushBackForce);
    //         }
    //         if (!isPropertiesSetted)
    //             Debug.LogWarning("Bullet hitted without before setted damage");
    //         Destroy(gameObject);
    //     }
    //     if (other.gameObject.tag == "Wall")
    //         Destroy(gameObject);
    // }
    public void setBulletProperties(int damage, int pushBackForce, string throwerTag)
    {
        this.damage = damage;
        this.pushBackForce = pushBackForce;
        gameObject.tag = throwerTag;
        isPropertiesSetted = true;
    }
}
