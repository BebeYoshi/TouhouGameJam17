using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "GachaPull", menuName = "Scriptable Objects/GachaPull")]
public class GachaPull : ScriptableObject
{
    [SerializeField] private CharacterDatabase _pullablePersonagens;
    [SerializeField] private  List<Personagens> _personagens;
    [SerializeField] private CharacterDatabase _team;
    public bool repetido;


    private bool isInitialized = false;
    private float chanceTotal;
    

    // private void Awake(){
    //     isInitialized = false;
    //     Initialize();
    // }

    private void Initialize(){
        CheckPullable();
        if(!isInitialized){
            chanceTotal = _personagens.Sum(personagem => personagem.chance);
            Debug.Log(chanceTotal);
            isInitialized = true;
        }
    }
    
    public void CheckPullable(){
        var temps = _pullablePersonagens.ReturnOnlyPullable();
            foreach(var temp in temps){
                if(_personagens.Where(c => c.id == temp.id).Count() == 0)
                _personagens.Add(new Personagens{id= temp.id, chance= temp.pullChance, nome = temp.displayName, sprite = temp.sprite});
            }
    }

    public Personagens SortearPersonagem(){
        Initialize();
        
        float geraNumeroRandom = Random.Range(0f, chanceTotal);
        foreach(var personagem in _personagens){
            if(personagem.chance >= geraNumeroRandom){
            Debug.Log(personagem.id);
            CharacterData chaar = _pullablePersonagens.FindById(personagem.id);
            Debug.Log("NOME" + chaar.displayName);
            if(chaar != null){
                Debug.Log("ENTROU NO ADD");
                 _team.insertNewCharacter(chaar);
                repetido = false;
                return personagem;
            }
            //  else repetido = true;
            //  return personagem;
            }
            geraNumeroRandom -= personagem.chance;
            Debug.Log(personagem.chance + "  " + geraNumeroRandom);
        }
        throw new System.Exception("Deu merda");
    }

    [System.Serializable]
    public class Personagens {
        public string nome;
        public float chance;
        public Sprite sprite;
        public string id;
    }

}