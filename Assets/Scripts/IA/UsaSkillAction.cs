using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "UsaSkill", story: "Agente usa skill", category: "Action", id: "f2b509e32667b51bff7be8cbb385ce17")]
public partial class UsaSkillAction : Action
{

    [SerializeReference] public BlackboardVariable<GameObject> Agente;
    [SerializeReference] public BlackboardVariable<GameObject> Inimigo;
    private CombatStats combat;
    private CombatStats combatEnemy;

    protected override Status OnStart()
    {
        combat = GameObject.GetComponent<CombatStats>();
        combatEnemy = Inimigo != null && Inimigo.Value != null? Inimigo.Value.GetComponent<CombatStats>() : null;

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        bool teste = combat.UseSkill(combatEnemy);
        Debug.Log("DISPAROU SKILL");
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

