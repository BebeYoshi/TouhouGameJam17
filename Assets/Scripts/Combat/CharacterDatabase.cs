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
