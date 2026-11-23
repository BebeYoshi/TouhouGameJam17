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
        if (Inimigo.Value == null)
        {
            return false;
        }

        CombatStats self = GameObject.GetComponent<CombatStats>();

        if (self != null && self.IsInitialized)
        {
            return self.IsInRange(Inimigo.Value.GetComponent<CombatStats>());
        }

        rb = GameObject.GetComponent<Rigidbody2D>();
        return Vector2.Distance(rb.position, Inimigo.Value.transform.position) <= 1.5f;
    }

    

    public override void OnStart()
    {

    }

    public override void OnEnd()
    {
        
    }
}
