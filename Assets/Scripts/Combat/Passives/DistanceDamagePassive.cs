// Aya
using UnityEngine;

[CreateAssetMenu(fileName = "DistanceDamagePassive", menuName = "Scriptable Objects/Passives/Distance Damage")]
public class DistanceDamagePassive : PassiveAbility
{
    [SerializeField, Min(0f)] private float maxDamageBonus = 0.3f;
    [SerializeField, Min(0f)] private float fullBonusDistance = 1.5f;

    public override float GetDamageBonus(CombatStats owner, CombatStats target)
    {
        float distance = Vector2.Distance(owner.transform.position, target.transform.position);

        if (distance <= fullBonusDistance)
        {
            return maxDamageBonus;
        }

        float closeness = Mathf.InverseLerp(owner.Data.AttackRange, fullBonusDistance, distance);
        return maxDamageBonus * closeness;
    }
}
