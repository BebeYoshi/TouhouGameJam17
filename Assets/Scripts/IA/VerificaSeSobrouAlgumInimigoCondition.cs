using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Verifica se sobrou algum inimigo", story: "[Agente] verifica se tem algum [inimigo] vivo", category: "Conditions", id: "f4237c60aae43dc6ed4198be67560b0c")]
public partial class VerificaSeSobrouAlgumInimigoCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Agente;
    [SerializeReference] public BlackboardVariable<GameObject> Circulo;
    private Rigidbody2D rb;
    private CombatStats combatEnemy;
    private bool temVivo;

    public override bool IsTrue()
    {
        temVivo = false;
        GameObject[] Inimigos = GameObject.FindGameObjectsWithTag("Inimigo");

        if(Inimigos != null){
            foreach(var inimigo in Inimigos){
                combatEnemy = inimigo.GetComponent<CombatStats>();

                if(combatEnemy.IsAlive ){
                    return true;
                }
            }
        }
        return false;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
