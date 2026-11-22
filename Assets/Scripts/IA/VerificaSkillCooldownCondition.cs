using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "VerificaSkillCooldown", story: "Agent verify if skill is on cooldown", category: "Conditions", id: "6f8238a2e6125f07ab76784099e9583b")]
public partial class VerificaSkillCooldownCondition : Condition
{
    private CombatStats combat;

    public override bool IsTrue()
    {
        combat = GameObject.GetComponent<CombatStats>();
        if(combat.IsSkillReady) return false;
        else return true;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
