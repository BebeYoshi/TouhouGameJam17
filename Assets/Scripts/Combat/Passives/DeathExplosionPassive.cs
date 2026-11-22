using UnityEngine;

[CreateAssetMenu(fileName = "DeathExplosionPassive", menuName = "Scriptable Objects/Passives/Death Explosion")]
public class DeathExplosionPassive : PassiveAbility
{
    [SerializeField] private SkillHitbox explosionPrefab;
    [SerializeField, Min(0f)] private float damageMultiplier = 1f;

    public override void OnDied(CombatStats owner)
    {
        if (explosionPrefab == null)
        {
            Debug.LogError($"{name}: explosion prefab not assigned.", this);
            return;
        }

        SkillHitbox explosion = Instantiate(explosionPrefab, owner.transform.position, Quaternion.identity);
        explosion.Setup(owner, HitboxTargets.Enemies, damageMultiplier, null, AttackKind.Skill);
    }
}
