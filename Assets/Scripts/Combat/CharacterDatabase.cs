using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "CharacterDatabase", menuName = "Scriptable Objects/CharacterDatabase")]
public class CharacterDatabase : ScriptableObject
{
    [SerializeField] private List<CharacterData> characters = new List<CharacterData>();

    public bool insertNewCharacter(CharacterData characterToAdd){
        if(FindById(characterToAdd.id) == null){
        characters.Add(characterToAdd);
        return true;
        }
        else return false;
    }

    public List<CharacterData> ReturnOnlyPullable(){
        return characters.Where(c => c.isPullable).ToList();
    }

    public CharacterData FindById(string id)
    {
        foreach (CharacterData character in characters)
        {
            Debug.Log("ID DO PERSONAGEM" + character.id);
            if (character.id == id)
            {
                return character;
            }
        }

        Debug.LogWarning($"Character '{id}' not found.");
        return null;
    }

    public void EnablePullable(string id)
    {
    CharacterData character = characters.Find(c => c.Id == id);

    if (character == null)
    {
        Debug.LogWarning($"Character {id} não encontrado!");
        return;
    }

    character.isPullable = true;
    }

    public List<CharacterData> ReturnAll()
    {   
        return characters;
    }

    private void OnValidate()
    {
        HashSet<string> seenIds = new HashSet<string>();

        for (int i = 0; i < characters.Count; i++)
        {
            CharacterData character = characters[i];

            if (string.IsNullOrWhiteSpace(character.Id))
            {
                Debug.LogWarning($"{name}: character at index {i} has an empty ID.", this);
                continue;
            }

            if (!seenIds.Add(character.Id))
            {
                Debug.LogWarning($"{name}: duplicate ID '{character.Id}' at index {i}.", this);
            }
        }
    }
}
