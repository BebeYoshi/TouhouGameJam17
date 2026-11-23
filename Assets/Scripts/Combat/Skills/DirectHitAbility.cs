using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DirectHitAbility", menuName = "Scriptable Objects/Abilities/Direct Hit Ability")]
public class DirectHitAbility : CombatAbility
{
    [SerializeField] private TargetMode targetMode = TargetMode.CurrentOrNearest;
    [SerializeField, Min(1)] private int targetCount = 1;
    [SerializeField] private MultiplierSource multiplierSource = MultiplierSource.Ult;
    [SerializeField, Min(0f)] private float customMultiplier = 0f;
    [SerializeField] private EffectSpec effect = new EffectSpec();

    public override bool Execute(CombatStats owner, CombatStats target, AttackKind kind)
    {
        List<CombatStats> targets = TargetSelector.Select(targetMode, owner, target, targetCount);

        if (targets.Count == 0)
        {
            return false;
        }

        float multiplier = ResolveMultiplier(owner, multiplierSource, customMultiplier);

        foreach (CombatStats unit in targets)
        {
            bool dealsDamage = unit.Side != owner.Side && multiplier > 0f;
            bool hit = !dealsDamage || owner.PerformAttack(unit, multiplier, kind);

            if (hit && effect != null)
            {
                unit.ApplyEffect(effect.Create(owner));
            }
        }

        return true;
    }
}
