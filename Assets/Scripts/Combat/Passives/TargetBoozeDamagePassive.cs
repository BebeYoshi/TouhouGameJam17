// Miyoi
using UnityEngine;

[CreateAssetMenu(fileName = "TargetBoozeDamagePassive", menuName = "Scriptable Objects/Passives/Target Booze Damage")]
public class TargetBoozeDamagePassive : PassiveAbility
{
    [SerializeField, Min(0f)] private float maxDamageBonus = 0.25f;

    public override float GetDamageBonus(CombatStats owner, CombatStats target) => maxDamageBonus * target.BoozeFraction;
}
