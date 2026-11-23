using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SelfEffectAbility", menuName = "Scriptable Objects/Abilities/Self Effect Ability")]
public class SelfEffectAbility : CombatAbility
{
    [SerializeField] private List<EffectSpec> effects = new List<EffectSpec>();

    public override bool Execute(CombatStats owner, CombatStats target, AttackKind kind)
    {
        bool applied = false;

        foreach (EffectSpec effect in effects)
        {
            StatusEffect created = effect.Create(owner);

            if (created != null)
            {
                owner.ApplyEffect(created);
                applied = true;
            }
        }

        return applied;
    }
}