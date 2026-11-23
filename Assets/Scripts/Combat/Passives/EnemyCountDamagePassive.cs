using UnityEngine;

[CreateAssetMenu(fileName = "EnemyCountDamagePassive", menuName = "Scriptable Objects/Passives/Enemy Count Damage")]
public class EnemyCountDamagePassive : PassiveAbility
{
    [SerializeField, Min(0f)] private float bonusPerMissingEnemy = 0.15f;
    [SerializeField, Min(1)] private int referenceEnemyCount = 3;

    public override float GetDamageBonus(CombatStats owner, CombatStats target)
    {
        int missing = Mathf.Max(0, referenceEnemyCount - BattleRoster.GetEnemiesOf(owner).Count);
        return bonusPerMissingEnemy * missing;
    }
}
