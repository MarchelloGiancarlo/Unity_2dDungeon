using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class CreatureModule : MonoBehaviour, IDamageable, IPushable, IUnitRadius
{
    [Serializable]
    public class CreatureAIProperties
    {
        [SerializeField] [Range(0f, 20)] public float attackRange = 5;
        [SerializeField] public bool moveAroundRandomly = false;
    }
    [SerializeField] private int lifePoints = 100;
    [SerializeField] private int onTouchDamage = 2;
    [SerializeField] private int movementSpeed = 1;
    [SerializeField] private float unitRadius = 0.3f;
    [SerializeField] [Range(5, 20)] private int searchRadius = 20;
    [SerializeField] private CreatureAIProperties creatureAIProperties;
    private Rigidbody2D rb;
    private AIPath pathAI;
    private AIDestinationSetter destinationSetter;
    private SpriteRenderer spriteRenderer;
    private IWeapon weapon;
    public Vector2 moveAroundTargetPoint;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pathAI = GetComponent<AIPath>();
        pathAI.maxSpeed = movementSpeed;
        destinationSetter = GetComponent<AIDestinationSetter>();
        if (destinationSetter != null)
            InvokeRepeating("searchOpponents", 0.5f, 0.5f);
        foreach (Transform child in transform)
        {
            if (child.GetComponent<SpriteRenderer>() != null)
                spriteRenderer = child.GetComponent<SpriteRenderer>();

            if (child.GetComponent<IWeapon>() != null)
                weapon = child.GetComponent<IWeapon>();
        }

        if (!Utils.Tag.isEnemy(gameObject))
            Debug.LogWarning("A Creature module is attached to a not enemy TAG object");
        // setAIPath();
        if (creatureAIProperties.moveAroundRandomly)
            setMoveAroundTargetPoint();
    }
    private void Update()
    {
        //AI logic
        Transform target = destinationSetter.target;
        if (target != null)
        {
            if (Utils.Ai.isInRange(gameObject, target.gameObject, creatureAIProperties.attackRange) &&
                Utils.Ai.isTargetVisible(gameObject, target.gameObject))
            {
                //Attack
                if (weapon != null)
                    weapon.Attack(target.position);
            }
        }
        else
        {
            if (creatureAIProperties.moveAroundRandomly)
            {
                if (Vector2.Distance(transform.position, moveAroundTargetPoint) > 0.5
                    && moveAroundTargetPoint != Vector2.zero)
                {
                    rb.AddForce((moveAroundTargetPoint - (Vector2)transform.position).normalized
                        * movementSpeed * 50);
                }
                else
                {
                    setMoveAroundTargetPoint();
                }
            }
        }
    }
    public void receivedDamage(int damage)
    {
        lifePoints -= damage;
        damagedEffect();
        if (lifePoints <= 0)
            Destroy(gameObject);
    }
    public void receivedPush(Vector2 vector)
    {
        pathAI.enabled = false;
        rb.velocity = new Vector2(0, 0);
        rb.AddForce(vector);
        Invoke("delayedAIPathRiactivation", 0.8f);
    }
    void delayedAIPathRiactivation()
    {
        pathAI.enabled = true;

    }
    private void damagedEffect()
    {
        spriteRenderer.color = Color.red;
        Invoke("disableDamagedEffect", 0.3f);
    }

    void disableDamagedEffect()
    {
        spriteRenderer.color = Color.white;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject);
        if (!Utils.Tag.isOppositeSite(gameObject, other.gameObject))
            return;
        if (other.gameObject.GetComponent<IDamageable>() != null)
        {
            other.gameObject.GetComponent<IDamageable>().receivedDamage(onTouchDamage);
        }
    }
    // public void ChildOnTriggerEnter(GameObject child, Collider2D other)
    // {
    //     Debug.Log(other.gameObject);
    //     if (!Utils.Tag.isOppositeSite(gameObject, other.gameObject))
    //         return;
    //     if (other.gameObject.GetComponent<IDamageable>() != null)
    //     {
    //         other.gameObject.GetComponent<IDamageable>().receivedDamage(onTouchDamage);
    //     }
    // }

    public float getUnitRadius()
    {
        return unitRadius;
    }
    private void searchOpponents()
    {
        if (destinationSetter.target == null)
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, searchRadius);
            foreach (Collider2D collider in hitColliders)
            {
                if (Utils.Tag.isOppositeSite(gameObject, collider.gameObject))
                {
                    destinationSetter.target = collider.transform;
                    pathAI.endReachedDistance = getUnitRadius()
                        + collider.GetComponent<IUnitRadius>().getUnitRadius()
                        + creatureAIProperties.attackRange - 0.5f;
                    return;
                }
            }
        }
    }
    private void setMoveAroundTargetPoint()
    {
        Vector2 point;
        for (int i = 0; i < 3; i++)
        {
            point = UnityEngine.Random.insideUnitCircle * 3 + (Vector2)transform.position;
            if (Utils.isPathClearOfWall(transform.position, point))
            {
                moveAroundTargetPoint = point;
                return;
            }
        }
    }

}
