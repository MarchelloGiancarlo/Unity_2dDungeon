using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerModule : MonoBehaviour, IDamageable, IPushable, IUnitRadius
{
    public int maxLifePoints = 50;
    [SerializeField] [Range(2, 20)] float movementSpeed = 5;
    [SerializeField] [Range(10, 30)] float dashSpeed = 10;
    [SerializeField] private float unitRadius = 0.3f;
    private GameObject heroSprite, weaponObject;
    private IWeapon weapon;
    private Rigidbody2D rb;
    private Animator heroAnimator;
    private Vector2 moveDirection;
    private SpriteRenderer heroSpriteRenderer;
    private TrailRenderer trailRenderer;
    private bool isDashing = false;
    [SerializeField] private int lifePoints;
    [SerializeField] private Slider UIslider;
    private void Awake()
    {
        lifePoints = maxLifePoints;
        //This is based on the logic that a player have only the heroSprite and the weapon as children
        foreach (Transform child in transform)
        {
            if (child.GetComponent<IWeapon>() != null)
            {
                weaponObject = child.gameObject;
                weapon = child.GetComponent<IWeapon>();
            }
            else if (child.GetComponent<TrailRenderer>() != null)
            {
                trailRenderer = child.GetComponent<TrailRenderer>();
                trailRenderer.enabled = false;
            }
            else
            {
                heroSprite = child.gameObject;
                heroAnimator = heroSprite.GetComponent<Animator>();
            }
        }
        rb = GetComponent<Rigidbody2D>();
        if (!Utils.Tag.isAllied(gameObject))
            Debug.LogWarning("A Player module is attached to a not allied TAG object");
        heroSpriteRenderer = heroSprite.GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        playerMovement();
        setHeroSpriteDirection();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // rb.velocity = Vector2.zero;
            // Vector2 dashDirection = (target - (Vector2)transform.position).normalized;
            Vector2 dashDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
            rb.AddForce(dashDirection * dashSpeed * 100);
            isDashing = true;
        }
        if (weapon != null)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                weapon.Attack(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
        }
    }
    private void FixedUpdate()
    {
        moveDirection = moveDirection.normalized;
        rb.AddForce(moveDirection * movementSpeed * Time.fixedDeltaTime * 500);
    }
    private void playerMovement()
    {
        if (isDashing)
        {
            trailRenderer.enabled = true;
            moveDirection.x = 0;
            moveDirection.y = 0;
            if (rb.velocity.magnitude <= 5)
                isDashing = false;
        }
        else
        {
            trailRenderer.enabled = false;
            moveDirection.x = Input.GetAxis("Horizontal");
            moveDirection.y = Input.GetAxis("Vertical");
        }
    }
    private void setHeroSpriteDirection()
    {
        if (Mathf.Abs(moveDirection.x) > 0.1f)
        {
            if (moveDirection.x > 0f)
                heroSprite.transform.localScale = new Vector3(1, 1, 1);
            else
                heroSprite.transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    public void receivedDamage(int damage)
    {
        damagedEffect();
        if (UIslider != null)
            UIslider.value = (float)lifePoints / maxLifePoints;
        lifePoints -= damage;
    }
    public void receivedPush(Vector2 vector)
    {
        rb.AddForce(vector);
    }
    private void damagedEffect()
    {
        heroSpriteRenderer.color = Color.red;
        Invoke("disableDamagedEffect", 0.3f);
    }

    void disableDamagedEffect()
    {
        heroSpriteRenderer.color = Color.white;
    }

    public float getUnitRadius()
    {
        return unitRadius;
    }
}
