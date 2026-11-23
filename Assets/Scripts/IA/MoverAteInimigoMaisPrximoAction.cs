using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Mover ate inimigo mais próximo", story: "Agente anda até [inimigo]", category: "Action", id: "9b2ae6bca3b93bb33be73bfd59d25ed7")]
public partial class MoverAteInimigoMaisPrximoAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Inimigo;
    [SerializeReference] public BlackboardVariable<float> Velocidade = new(3f);
    [SerializeReference] public BlackboardVariable<float> DistanciaInimigo = new(1.5f);
    [SerializeReference] public BlackboardVariable<float> range;


    private Rigidbody2D rb;


    protected override Status OnStart()
    {
        rb = GameObject.GetComponent<Rigidbody2D>();
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if(Inimigo.Value == null || rb == null)
            return Status.Failure;

        CombatStats self = GameObject.GetComponent<CombatStats>(); 

        if (self != null && !self.CanAct)
        {
            rb.linearVelocity = Vector2.zero;
            return Status.Running;
        }

        Vector2 posicaoDesejada = Inimigo.Value.transform.position;
        bool inRange = self != null && self.IsInitialized
            ? self.IsInRange(Inimigo.Value.GetComponent<CombatStats>())
            : Vector2.Distance(rb.position, posicaoDesejada) <= range;

        if (inRange)
        {
            rb.linearVelocity = Vector2.zero;
            return Status.Success;
        }

        Vector2 direcao =  (posicaoDesejada - rb.position).normalized;

        rb.linearVelocity = direcao * Velocidade.Value;
        return Status.Running;
    }

    protected override void OnEnd()
    {
        if(rb != null) 
            rb.linearVelocity = Vector2.zero;
    }
}

