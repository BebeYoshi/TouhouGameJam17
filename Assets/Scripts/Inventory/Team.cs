using UnityEngine;

public class Team : MonoBehaviour
{
    [SerializeField]CharacterDatabase TeamCharacters;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool addToTeam(CharacterData chaar){

        if(TeamCharacters.FindById(chaar.id) != null){
            TeamCharacters.insertNewCharacter(chaar);
            return true;
        }
        else return false;
    }

    // public listCharacters(){
    //     foreach(characters in TeamCharacters.g)
    // }
}
