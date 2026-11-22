// Kasen
using UnityEngine;

[CreateAssetMenu(fileName = "BoozeGainPassive", menuName = "Scriptable Objects/Passives/Booze Gain")]
public class BoozeGainPassive : PassiveAbility
{
    [SerializeField, Min(0f)] private float boozeGainBonus = 0.25f;

    public override float GetBoozeGainBonus(CombatStats owner) => boozeGainBonus;
}
