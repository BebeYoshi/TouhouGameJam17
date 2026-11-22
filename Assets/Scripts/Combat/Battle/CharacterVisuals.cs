using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CharacterVisuals", menuName = "Scriptable Objects/Battle/Character Visuals")]
public class CharacterVisuals : ScriptableObject
{
    [Serializable]
    private class Entry
    {
        public string characterId = "";
        public GameObject prefab;
    }

    [SerializeField] private List<Entry> entries = new List<Entry>();

    public GameObject FindPrefab(string characterId)
    {
        string baseId = characterId.EndsWith("_boss") ? characterId.Substring(0, characterId.Length - "_boss".Length) : characterId;

        foreach (Entry entry in entries)
        {
            if (entry.characterId == characterId || entry.characterId == baseId)
            {
                return entry.prefab;
            }
        }

        return null;
    }
}
