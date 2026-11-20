using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "GachaPull", menuName = "Scriptable Objects/GachaPull")]
public class GachaPull : ScriptableObject
{
    [SerializeField] private  List<Personagens> _personagens;

    private bool isInitialized = false;
    private float chanceTotal;

    private void Initialize(){
        if(!isInitialized){
            chanceTotal = _personagens.Sum(personagem => personagem.chance);
            isInitialized = true;
        }
    }

    public Personagens SortearPersonagem(){
        Initialize();
        
        float geraNumeroRandom = Random.Range(0f, chanceTotal);
        foreach(var personagem in _personagens){
            if(personagem.chance >= geraNumeroRandom){
                return personagem;
            }
            geraNumeroRandom -= personagem.chance;
        }
        throw new System.Exception("Deu merda");
    }

    [System.Serializable]
    public class Personagens {
        public string nome;
        public float chance;
        public Sprite sprite;
    }

}