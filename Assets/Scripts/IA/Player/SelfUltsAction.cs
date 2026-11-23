using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Self Ults", story: "[Self] uts", category: "Action", id: "e0e57203811db5b3be70db2ef45d8c35")]
public partial class SelfUltsAction : Action
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
        bool teste = combat.UseUlt(combatEnemy);
        Debug.Log("DISPAROU ULT" + teste);
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

