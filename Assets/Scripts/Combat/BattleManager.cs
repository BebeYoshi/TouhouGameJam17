using UnityEngine;
using System;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private List<CombatStats> inimigos;
    [SerializeField] private List<CombatStats> aliados;
    [SerializeField] private string SceneName;
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private GameObject canvasVitoria;
    [SerializeField] private GameObject canvasDerrota;
    [SerializeField] int level;

    public event Action Victory;

    private bool battleOver;


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

    private void DisparouMorteInimigo(CombatStats deadEnemy)
    {
        if (!battleOver && VerificaVitoria())
        {
            battleOver = true;
            canvasVitoria.SetActive(true);
            Victory?.Invoke();
        }
    }

    private void DisparouMorteAliado(CombatStats deadAlly)
    {
        if (!battleOver && VerificaDerrota())
        {
            battleOver = true;
            canvasDerrota.SetActive(true);
        }
    }

    private bool VerificaVitoria(){
         foreach(CombatStats inimigo in inimigos){
            if(inimigo != null && inimigo.IsAlive){
                Debug.Log("ENTROU NO FALSE VITORIA");
                return false;
            }
                            Debug.Log("ENTROU NO TRUE VITORIA");

        }
        return true;
        throw new System.Exception("Deu merda");

    }

    private bool VerificaDerrota(){
            foreach(CombatStats aliado in aliados){
            if(aliado != null && aliado.IsAlive){
             return false;
            }
         }
        return true;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
