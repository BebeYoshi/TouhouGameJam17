using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TeleportBoozeAbility", menuName = "Scriptable Objects/Abilities/Teleport Booze Ability")]
public class TeleportBoozeAbility : CombatAbility
{
    [SerializeField] private TargetMode targetMode = TargetMode.AllyClosestToUlt;
    [SerializeField, Min(0f)] private float boozeAmount = 30f;
    [SerializeField, Min(0f)] private float arrivalDistance = 1f;

    public override bool Execute(CombatStats owner, CombatStats target, AttackKind kind)
    {
        List<CombatStats> targets = TargetSelector.Select(targetMode, owner, target, 1);

        if (targets.Count == 0)
        {
            return false;
        }

        CombatStats ally = targets[0];
        TeleportNextTo(owner, ally);
        ally.AddBooze(boozeAmount);
        return true;
    }

    private void TeleportNextTo(CombatStats owner, CombatStats ally)
    {
        Vector2 allyPosition = ally.transform.position;
        Vector2 offset = (Vector2)owner.transform.position - allyPosition;
        Vector2 direction = offset.sqrMagnitude > 0.0001f ? offset.normalized : Vector2.left;
        Vector2 destination = allyPosition + direction * arrivalDistance;

        UnitMover.TeleportTo(owner, destination);
    }
}
