using UnityEngine;

[CreateAssetMenu(fileName = "UnleashAbility", menuName = "Scriptable Objects/Abilities/Unleash Ability")]
public class UnleashAbility : CombatAbility
{
    [SerializeField, Min(1f)] private float newMaxBooze = 300f;
    [SerializeField, Min(0f)] private float boozeLossOnHurt = 1f;
    [SerializeField, Range(0f, 1f)] private float boozeLossOnHurtPercent = 0.01f;

    public override bool Execute(CombatStats owner, CombatStats target, AttackKind kind)
    {
        owner.Unleash(newMaxBooze, boozeLossOnHurt, boozeLossOnHurtPercent);
        return true;
    }
}
