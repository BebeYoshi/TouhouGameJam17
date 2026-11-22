using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "EstaNoRange", story: "Agent esta próximo do [Inimigo]", category: "Conditions", id: "6d0ecf38095ad29b1fc5a43b412ede6f")]
public partial class EstaNoRangeCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Inimigo;
    [SerializeReference] public BlackboardVariable<float> Range;
    private Rigidbody2D rb;


    public override bool IsTrue()
    {
        Debug.Log("retornou");
        rb = GameObject.GetComponent<Rigidbody2D>();
        Vector2 posicaoDesejada = Inimigo.Value.transform.position;
        float distancia = Vector2.Distance(rb.position, posicaoDesejada);
        if(distancia <= Range){
           return true;
        }
        else{
            return false;
        }
    }

    

    public override void OnStart()
    {

    }

    public override void OnEnd()
    {
        
    }
}
