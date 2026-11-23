using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "check if itself is alive", story: "agent is alive", category: "Conditions", id: "2a8b410905171b98f9e6da19ad9b8fc4")]
public partial class CheckIfItselfIsAliveCondition : Condition
{
    private CombatStats combat;

    public override bool IsTrue()
    {
        combat = GameObject.GetComponent<CombatStats>();
        if(combat.IsAlive != true) return false;

        return true;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
