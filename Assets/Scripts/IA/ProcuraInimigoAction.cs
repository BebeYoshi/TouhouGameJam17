using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ProcuraInimigo", story: "Agente procura [Circulo] mais próximo", category: "Action", id: "4bb85654d40aea9e5aa7c9b8e91bc7d9")]
public partial class ProcuraInimigoAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Circulo;
    private Rigidbody2D rb;
    private CombatStats combatEnemy;


    protected override Status OnStart()
    {
        GameObject[] Inimigos = GameObject.FindGameObjectsWithTag("Inimigo");
        rb = GameObject.GetComponent<Rigidbody2D>();

        var position = rb.position;
        float dist = float.PositiveInfinity;

        GameObject maisProximo = null;
        if(Inimigos != null){

        foreach(var inimigo in Inimigos){
            // var i = (inimigo.transform.position - position).sqrMagnitude;
            var i = Vector2.Distance(rb.position, inimigo.transform.position);
            combatEnemy = inimigo.GetComponent<CombatStats>();


            if(i < dist && combatEnemy.IsAlive ){
                maisProximo = inimigo;
                dist = i;
            }

        }
        Circulo.Value = maisProximo;
        }
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }

    // public GameObject FindNearestEnemy(){

    // }
}

