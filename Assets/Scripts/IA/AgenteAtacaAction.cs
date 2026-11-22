using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AgenteAtaca", story: "[Agente] attacks [enemy]", category: "Action", id: "4c3ad84605a2b94cef090eef74cb44a7")]
public partial class AgenteAtacaAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agente;
    [SerializeReference] public BlackboardVariable<GameObject> Inimigo;
    private CombatStats combat;
    private CombatStats combatEnemy;

    protected override Status OnStart()
    {
        combat = GameObject.GetComponent<CombatStats>();
        combatEnemy = Inimigo.Value.GetComponent<CombatStats>();

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        bool teste = combat.BasicAttack(combatEnemy);
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

