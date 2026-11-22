using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "IsUltimateReady", story: "Is [agent] ultimate ready", category: "Conditions", id: "412399f28b6e8f96b5ea76dc03e9f4b4")]
public partial class IsUltimateReadyCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
   private CombatStats combat;

    public override bool IsTrue()
    {
        combat = GameObject.GetComponent<CombatStats>();
        return combat != null && combat.IsUltReady;
    }
    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
