using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HitboxAbility", menuName = "Scriptable Objects/Abilities/Hitbox Ability")]
public class HitboxAbility : CombatAbility
{
    [SerializeField] private SkillHitbox hitboxPrefab;
    [SerializeField] private HitboxSpawn spawnAt = HitboxSpawn.Owner;
    [SerializeField] private HitboxTargets targets = HitboxTargets.Enemies;
    [SerializeField] private MultiplierSource multiplierSource = MultiplierSource.Skill;
    [SerializeField, Min(0f)] private float customMultiplier = 0f;
    [SerializeField] private EffectSpec effect = new EffectSpec();

    [Header("Projectiles")]
    [SerializeField, Min(1)] private int projectileCount = 1;
    [SerializeField, Min(0f)] private float spawnRadius = 0f;
    [SerializeField, Min(0f)] private float launchInterval = 0f;
    [SerializeField] private bool homing = false;
    [SerializeField, Range(0f, 180f)] private float spreadAngle = 0f;
    [SerializeField] private bool spreadAcrossEnemies = false;
    [SerializeField, Min(0f)] private float outwardSpeed = 0f;
    [SerializeField, Min(0f)] private float outwardDuration = 0f;

    public override bool Execute(CombatStats owner, CombatStats target, AttackKind kind)
    {
        if (hitboxPrefab == null)
        {
            Debug.LogError($"{name}: hitbox prefab not assigned.", this);
            return false;
        }

        if ((target == null || !target.IsAlive || target.Side == owner.Side) && targets == HitboxTargets.Enemies)
        {
            target = BattleRoster.GetNearestEnemy(owner);
        }

        bool hasTarget = target != null && target.IsAlive;
        bool needsTarget = targets == HitboxTargets.Enemies || spawnAt == HitboxSpawn.Target;

        if (needsTarget && !hasTarget)
        {
            return false;
        }

        Vector3 position = spawnAt == HitboxSpawn.Target ? target.transform.position : owner.transform.position;
        Quaternion rotation = Quaternion.identity;

        if (hasTarget)
        {
            Vector2 direction = target.transform.position - owner.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rotation = Quaternion.Euler(0f, 0f, angle);
        }

        float multiplier = ResolveMultiplier(owner, multiplierSource, customMultiplier);
        
        List<CombatStats> spreadTargets = spreadAcrossEnemies ? BattleRoster.GetNearestEnemies(owner, projectileCount) : null;

        for (int i = 0; i < projectileCount; i++)
        {
            Vector3 offset = GetSpawnOffset(i);
            SkillHitbox hitbox = Instantiate(hitboxPrefab, position + offset, rotation * GetSpreadRotation(i));
            hitbox.Setup(owner, targets, multiplier, effect, kind);

            if (homing && hasTarget)
            {
                CombatStats homingTarget = spreadTargets != null && spreadTargets.Count > 0 ? spreadTargets[i % spreadTargets.Count] : target;
                Vector2 outwardVelocity = (Vector2)offset.normalized * outwardSpeed;
                hitbox.HomeOn(homingTarget, outwardDuration + i * launchInterval, outwardVelocity);
            }
        }

        return true;
    }

    private Quaternion GetSpreadRotation(int index)
    {
        if (projectileCount <= 1 || spreadAngle <= 0f)
        {
            return Quaternion.identity;
        }

        float angle = (index - (projectileCount - 1) / 2f) * spreadAngle;
        return Quaternion.Euler(0f, 0f, angle);
    }

    private Vector3 GetSpawnOffset(int index)
    {
        if (projectileCount <= 1 || spawnRadius <= 0f)
        {
            return Vector3.zero;
        }

        float angle = index * Mathf.PI * 2f / projectileCount;
        return new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * spawnRadius;
    }
}
