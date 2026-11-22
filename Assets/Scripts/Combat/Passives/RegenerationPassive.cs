// Yuyuko
using UnityEngine;

[CreateAssetMenu(fileName = "RegenerationPassive", menuName = "Scriptable Objects/Passives/Regeneration")]
public class RegenerationPassive : PassiveAbility
{
    [SerializeField, Min(0f)] private float healMultiplier = 0.05f;

    public override void OnInitialized(CombatStats owner)
    {
        owner.ApplyEffect(StatusEffect.HealOverTime(owner, healMultiplier, float.PositiveInfinity));
    }
}
