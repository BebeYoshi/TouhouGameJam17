using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DashAbility", menuName = "Scriptable Objects/Abilities/Dash Ability")]
public class DashAbility : CombatAbility
{
    [SerializeField] private TargetMode targetMode = TargetMode.FarthestEnemy;
    [SerializeField] private MultiplierSource multiplierSource = MultiplierSource.Ult;
    [SerializeField, Min(0f)] private float customMultiplier = 0f;
    [SerializeField, Min(0.05f)] private float pathRadius = 0.5f;
    [SerializeField, Min(0f)] private float stopDistance = 1f;
    [SerializeField] private EffectSpec effect = new EffectSpec();

    public override bool Execute(CombatStats owner, CombatStats target, AttackKind kind)
    {
        List<CombatStats> targets = TargetSelector.Select(targetMode, owner, target, 1);

        if (targets.Count == 0)
        {
            return false;
        }

        Vector2 start = owner.transform.position;
        Vector2 end = targets[0].transform.position;
        Vector2 path = end - start;
        float distance = path.magnitude;
        Vector2 direction = distance > 0.0001f ? path / distance : Vector2.right;
        float multiplier = ResolveMultiplier(owner, multiplierSource, customMultiplier);

        foreach (CombatStats enemy in FindEnemiesOnPath(owner, start, direction, distance))
        {
            bool hit = multiplier <= 0f || owner.PerformAttack(enemy, multiplier, kind);

            if (hit && effect != null)
            {
                enemy.ApplyEffect(effect.Create(owner));
            }
        }

        UnitMover.TeleportTo(owner, start + direction * Mathf.Max(0f, distance - stopDistance));
        return true;
    }

    private List<CombatStats> FindEnemiesOnPath(CombatStats owner, Vector2 start, Vector2 direction, float distance)
    {
        List<CombatStats> enemies = new List<CombatStats>();
        RaycastHit2D[] hits = Physics2D.CircleCastAll(start, pathRadius, direction, distance);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.isTrigger)
            {
                continue;
            }

            CombatStats unit = hit.collider.GetComponentInParent<CombatStats>();

            if (unit != null && unit.IsAlive && unit.Side != owner.Side && !enemies.Contains(unit))
            {
                enemies.Add(unit);
            }
        }

        return enemies;
    }
}
