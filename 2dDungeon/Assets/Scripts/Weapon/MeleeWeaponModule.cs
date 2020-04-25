using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponModule : MonoBehaviour, IWeapon
{
    [SerializeField] [Range(1, 10)] private int damage = 1;
    [SerializeField] [Range(10, 100)] private int pushBackForce = 10;
    [SerializeField] [Range(0.1f, 10)] private float attackPerSecond = 1;
    [SerializeField] [Range(0.1f, 10)] private float weaponRange = 0.5f;
    // [SerializeField] private bool followLook = true;
    private GameObject weaponSprite;
    private Animator spriteAnimator;
    private GameObject weaponUser;
    private bool canAttack = true;
    private bool isPlayerUser;
    private Vector2 oldPosition;
    private void Awake()
    {
        weaponUser = transform.parent.gameObject;
        foreach (Transform child in transform)
        {
            weaponSprite = child.gameObject;
        }
        if (weaponSprite != null)
            spriteAnimator = weaponSprite.GetComponent<Animator>();
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
        Invoke("canAttackRiactivate", 1f / attackPerSecond);
        if (spriteAnimator != null)
            spriteAnimator.SetTrigger("Attack");
        return true;
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
}
