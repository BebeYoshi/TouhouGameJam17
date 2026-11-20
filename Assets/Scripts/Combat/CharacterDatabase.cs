using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CharacterDatabase", menuName = "Scriptable Objects/CharacterDatabase")]
public class CharacterDatabase : ScriptableObject
{
    [SerializeField] private List<CharacterData> characters = new List<CharacterData>();

    public CharacterData FindById(string id)
    {
        foreach (CharacterData character in characters)
        {
            if (character.Id == id)
            {
                return character;
            }
        }

        Debug.LogWarning($"Character '{id}' not found.");
        return null;
    }
}
