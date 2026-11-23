using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "GachaPull", menuName = "Scriptable Objects/GachaPull")]
public class GachaPull : ScriptableObject
{
    [SerializeField] private CharacterDatabase _pullablePersonagens;
    [SerializeField] private  List<Personagens> _personagens;
    [SerializeField] private PartyLoadout _team;
    [SerializeField] private Player_info playerInfo;

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

    public Personagens SortearPersonagem()
{
    Initialize();

    float geraNumeroRandom = Random.Range(0f, chanceTotal);

    foreach (var personagem in _personagens)
    {
    if (geraNumeroRandom < personagem.chance)
    {
        bool jaPossui = _team.GetMember(personagem.id) != null;

        if (!jaPossui)
        {
            _team.AddMember(
                new UnitEntry(personagem.id, 1)
            );

            repetido = false;
        }
        else
        {
            playerInfo.AddPoint(50);
            playerInfo.AddPower(30);

            repetido = true;
        }

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
        public string id;
    }

}