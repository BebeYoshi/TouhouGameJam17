using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Verifica se tem alguem vivo", story: "[Agent] tem inimigo vivo", category: "Conditions", id: "b6cfa2facc109778c6bf3f861cd9fbc2")]
public partial class VerificaSeTemAlguemVivoEnemyBehaviorCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Agente;
    [SerializeReference] public BlackboardVariable<GameObject> Circulo;
    private Rigidbody2D rb;
    private CombatStats combatEnemy;
    private bool temVivo;

    public override bool IsTrue()
    {
        temVivo = false;
        GameObject[] Inimigos = GameObject.FindGameObjectsWithTag("InimigoPlayer");

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
