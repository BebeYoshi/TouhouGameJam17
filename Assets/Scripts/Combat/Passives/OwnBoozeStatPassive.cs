// Suika e Yuugi
using UnityEngine;

[CreateAssetMenu(fileName = "OwnBoozeStatPassive", menuName = "Scriptable Objects/Passives/Own Booze Stat")]
public class OwnBoozeStatPassive : PassiveAbility
{
    [SerializeField, Min(0f)] private float maxAttackBonus = 0.2f;
    [SerializeField, Min(0f)] private float maxDefenseBonus = 0f;
    [SerializeField, Min(0f)] private float referenceBooze = 0f;

    public override float GetAttackBonus(CombatStats owner) => maxAttackBonus * GetFraction(owner);
    public override float GetDefenseBonus(CombatStats owner) => maxDefenseBonus * GetFraction(owner);

    private float GetFraction(CombatStats owner)
    {
        if (referenceBooze <= 0f)
        {
            return owner.BoozeFraction;
        }

        return Mathf.Clamp01(owner.CurrentBooze / referenceBooze);
    }
}
