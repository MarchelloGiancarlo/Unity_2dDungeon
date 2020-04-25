using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWeaponModule : MonoBehaviour, IWeapon
{
    public enum SpawnWeaponPattern
    {
        AllAround
    };
    [SerializeField] private SpawnWeaponPattern pattern;
    [SerializeField] [Range(0.1f, 10)] private float attackPerSecond = 1;
    [SerializeField] [Range(1, 30)] private float weaponRange = 5;
    // [Range(1, 50)] public float maxBulletPerSecond = 1; //Not implemented yet
    [SerializeField] [Range(1, 10)] private float bulletSpeed = 1;
    [SerializeField] [Range(1, 20)] private int bulletDamage = 1;
    [SerializeField] [Range(1, 20)] private int numberOfBullets = 1;
    [SerializeField] [Range(10, 50)] private int pushBackForce = 10;
    [SerializeField] private float attackDelayForAnimatorCorrection = 0.3f;
    [SerializeField] private GameObject bulletSpawnPoint;
    [SerializeField] private GameObject ammoPrefab;
    // [SerializeField] private bool followLook = true;
    private GameObject weaponSprite;
    private Animator spriteAnimator;
    private GameObject weaponUser;
    private bool canAttack = true;
    private bool isPlayerUser;
    private Vector2 oldPosition;
    private Vector2 AttackTargetPosition;
    private void Awake()
    {
        weaponUser = transform.parent.gameObject;
        foreach (Transform child in transform)
        {
            weaponSprite = child.gameObject;
        }
        if (weaponSprite == null)
            foreach (Transform child in transform.parent)
            {
                if (child.GetComponent<Animator>() != null)
                    spriteAnimator = child.GetComponent<Animator>();
            }
        isPlayerUser = weaponUser.GetComponent<PlayerModule>() != null;
    }
    private void Update()
    {
        float speed = Vector2.Distance(oldPosition, transform.position) / Time.deltaTime;
        oldPosition = transform.position;
        if (spriteAnimator != null)
        {
            spriteAnimator.SetFloat("Speed", speed);
        }
        if (isPlayerUser)
        {
            Vector2 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float lookAngle = Utils.getAngleDirection(transform.position, targetPosition);
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, lookAngle));
        }
    }
    public bool Attack(Vector2 position)
    {
        if (!canAttack)
            return false;
        canAttack = false;
        if (spriteAnimator != null)
        {
            spriteAnimator.SetTrigger("Attack");
        }
        AttackTargetPosition = position;
        Invoke("canAttackRiactivate", 1f / attackPerSecond);
        Invoke("delayedAttack", attackDelayForAnimatorCorrection);
        return true;
    }
    void delayedAttack()
    {
        float angle = Utils.getAngleDirection(transform.position, AttackTargetPosition);
        switch (pattern)
        {
            case SpawnWeaponPattern.AllAround:
                float deltaAngle = 360 / numberOfBullets;
                for (int i = 0; i < numberOfBullets; i++)
                {
                    spawnBullet(angle + deltaAngle * i);
                }
                break;
        }
    }
    public bool isReadyToAttack()
    {
        return canAttack;
    }
    public float getWeaponRange()
    {
        return weaponRange;
    }
    void canAttackRiactivate()
    {
        canAttack = true;
    }
    private void spawnBullet(float angleToShoot)
    {
        GameObject ammo = Instantiate(ammoPrefab,
            bulletSpawnPoint.transform.position,
            Quaternion.AngleAxis(angleToShoot - 90, Vector3.forward));
        ammo.GetComponent<BulletModule>().setBulletProperties(bulletDamage,
            pushBackForce, weaponUser.tag);
        ammo.GetComponent<Rigidbody2D>().AddForce(ammo.transform.up * bulletSpeed * 100);
    }
}
