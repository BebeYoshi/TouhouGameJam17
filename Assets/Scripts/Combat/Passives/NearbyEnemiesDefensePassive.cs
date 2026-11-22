using UnityEngine;

[CreateAssetMenu(fileName = "NearbyEnemiesDefensePassive", menuName = "Scriptable Objects/Passives/Nearby Enemies Defense")]
public class NearbyEnemiesDefensePassive : PassiveAbility
{
    [SerializeField, Min(0f)] private float bonusPerEnemy = 0.12f;
    [SerializeField, Min(0f)] private float radius = 2.5f;

    public override float GetDefenseBonus(CombatStats owner)
    {
        int nearby = 0;

        foreach (CombatStats enemy in BattleRoster.GetEnemiesOf(owner))
        {
            if (Vector2.Distance(owner.transform.position, enemy.transform.position) <= radius)
            {
                nearby++;
            }
        }

        return bonusPerEnemy * nearby;
    }
}
