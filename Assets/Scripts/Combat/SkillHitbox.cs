using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class SkillHitbox : MonoBehaviour
{
    [SerializeField, Min(0.05f)] private float lifetime = 0.5f;
    [SerializeField, Min(0f)] private float speed = 0f;
    [SerializeField] private bool destroyOnFirstHit = false;

    private readonly HashSet<CombatStats> alreadyHit = new HashSet<CombatStats>();
    private CombatStats owner;
    private HitboxTargets targets;
    private float damageMultiplier;
    private EffectSpec effect;
    private AttackKind attackKind;
    private bool consumed;
    private Rigidbody2D body;
    private Collider2D hitboxCollider;
    private CombatStats homingTarget;
    private Vector2 direction;
    private float launchTime;
    private float expireTime;
    private bool isHoming;

    public bool IsProjectile => speed > 0f;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        hitboxCollider = GetComponent<Collider2D>();
    }

    public void Setup(CombatStats newOwner, HitboxTargets newTargets, float newDamageMultiplier, EffectSpec newEffect, AttackKind kind)
    {
        owner = newOwner;
        targets = newTargets;
        damageMultiplier = newDamageMultiplier;
        effect = newEffect;
        attackKind = kind;
        direction = transform.right;
        expireTime = Time.time + lifetime;

        if (IsProjectile)
        {
            body.linearVelocity = direction * speed;
        }
    }

    public void HomeOn(CombatStats target, float delay, Vector2 outwardVelocity)
    {
        homingTarget = target;
        isHoming = true;
        launchTime = Time.time + delay;
        expireTime = launchTime + lifetime;
        hitboxCollider.enabled = delay <= 0f;
        body.linearVelocity = outwardVelocity;

        if (outwardVelocity.sqrMagnitude > 0.0001f)
        {
            direction = outwardVelocity.normalized;
            body.rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }
    }

    private void FixedUpdate()
    {
        if (Time.time >= expireTime)
        {
            Destroy(gameObject);
            return;
        }

        if (!isHoming || consumed || Time.time < launchTime)
        {
            return;
        }

        hitboxCollider.enabled = true;

        if (homingTarget == null || !homingTarget.IsAlive)
        {
            homingTarget = BattleRoster.GetNearestEnemy(owner);
        }

        if (homingTarget != null && homingTarget.IsAlive)
        {
            Vector2 offset = homingTarget.transform.position - transform.position;

            if (offset.sqrMagnitude > 0.0001f)
            {
                direction = offset.normalized;
            }
        }

        body.rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        body.linearVelocity = direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (owner == null || consumed || other.isTrigger)
        {
            return;
        }

        CombatStats unit = other.GetComponentInParent<CombatStats>();

        if (unit == null || !unit.IsAlive || alreadyHit.Contains(unit))
        {
            return;
        }

        bool isEnemy = unit.Side != owner.Side;

        if (isEnemy != (targets == HitboxTargets.Enemies))
        {
            return;
        }

        alreadyHit.Add(unit);

        bool dealsDamage = isEnemy && damageMultiplier > 0f;
         bool hit = !dealsDamage || owner.PerformAttack(unit, damageMultiplier, attackKind);

        if (destroyOnFirstHit)
        {
            Consume();
        }

        if (hit && effect != null)
        {
            unit.ApplyEffect(effect.Create(owner));
        }
    }

    public bool TryBlock(BattleSide barrierSide)
    {
        if (!IsProjectile || consumed || owner == null || owner.Side == barrierSide)
        {
            return false;
        }

        Consume();
        return true;
    }

    private void Consume()
    {
        consumed = true;
        hitboxCollider.enabled = false;
        Destroy(gameObject);
    }
}
