using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
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
    private bool consumed;

    public void Setup(CombatStats newOwner, HitboxTargets newTargets, float newDamageMultiplier, EffectSpec newEffect)
    {
        owner = newOwner;
        targets = newTargets;
        damageMultiplier = newDamageMultiplier;
        effect = newEffect;

        Rigidbody2D body = GetComponent<Rigidbody2D>();

        if (body != null && speed > 0f)
        {
            body.linearVelocity = transform.right * speed;
        }

        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (owner == null || consumed)
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
        bool hit = !dealsDamage || owner.PerformAttack(unit, damageMultiplier);

        if (destroyOnFirstHit)
        {
            Consume();
        }

        if (hit && effect != null)
        {
            unit.ApplyEffect(effect.Create(owner));
        }
    }

    private void Consume()
    {
        consumed = true;
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject);
    }
}
