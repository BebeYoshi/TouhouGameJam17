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

    public override bool Execute(CombatStats owner, CombatStats target)
    {
        if (hitboxPrefab == null)
        {
            Debug.LogError($"{name}: hitbox prefab not assigned.", this);
            return false;
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

        SkillHitbox hitbox = Instantiate(hitboxPrefab, position, rotation);
        hitbox.Setup(owner, targets, GetDamageMultiplier(owner), effect);
        return true;
    }

    private float GetDamageMultiplier(CombatStats owner)
    {
        switch (multiplierSource)
        {
            case MultiplierSource.Basic:
                return owner.Data.BasicMultiplier;

            case MultiplierSource.Skill:
                return owner.Data.SkillMultiplier;

            default:
                return customMultiplier;
        }
    }
}
