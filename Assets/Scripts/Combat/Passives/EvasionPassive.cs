// Mamizou
using UnityEngine;

[CreateAssetMenu(fileName = "EvasionPassive", menuName = "Scriptable Objects/Passives/Evasion")]
public class EvasionPassive : PassiveAbility
{
    [SerializeField, Range(0f, 1f)] private float evasionChance = 0.05f;

    public override float GetEvasionChance(CombatStats owner) => evasionChance;
}
