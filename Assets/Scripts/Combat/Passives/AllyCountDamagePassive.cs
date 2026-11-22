using UnityEngine;

[CreateAssetMenu(fileName = "AllyCountDamagePassive", menuName = "Scriptable Objects/Passives/Ally Count Damage")]
public class AllyCountDamagePassive : PassiveAbility
{
    [SerializeField, Min(0f)] private float bonusPerAlly = 0.15f;

    public override float GetDamageBonus(CombatStats owner, CombatStats target)
    {
        return bonusPerAlly * BattleRoster.GetAlliesOf(owner, false).Count;
    }
}
