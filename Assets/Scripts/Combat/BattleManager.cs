using UnityEngine;
using System;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private List<CombatStats> inimigos;
    [SerializeField] private List<CombatStats> aliados;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach(CombatStats inimigo in inimigos){
        inimigo.Died += DisparouMorteInimigo; 
        }
        foreach(CombatStats aliado in aliados){
        aliado.Died += DisparouMorteAliado;
        }
    }

    private void DisparouMorteInimigo(CombatStats deadEnemy){
        Debug.Log("DISPAROU 1");
        if(VerificaVitoria()) Debug.Log("ACABOU VENCEU");

    }

    private void DisparouMorteAliado(CombatStats deadAlly){
        Debug.Log("DISPAROU 2");
        if(VerificaDerrota()) Debug.Log("ACABOU PERDEU");
    }

    private bool VerificaVitoria(){
         foreach(CombatStats inimigo in inimigos){
            if(inimigo != null && inimigo.IsAlive){
                return false;
            }
            return true;
        }
        throw new System.Exception("Deu merda");

    }

    private bool VerificaDerrota(){
            foreach(CombatStats aliado in aliados){
            if(aliado != null && aliado.IsAlive){
             return false;
            }
             return true;
         }
         return false;

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
