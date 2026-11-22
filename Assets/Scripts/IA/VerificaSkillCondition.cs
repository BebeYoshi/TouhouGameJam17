using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "VerificaSkill", story: "Agent verifies if skill is on cooldown", category: "Conditions", id: "d7bec85904715d62ae0f5065f70779c7")]
public partial class VerificaSkillCondition : Condition
{

    public override bool IsTrue()
    {
        return true;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
