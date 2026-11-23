using UnityEngine;
using System;

[Serializable]
public class UnitEntry
{
    [SerializeField] private string characterId = "";
    [SerializeField, Min(1)] private int level = 1;

    public string CharacterId => characterId;
    public int Level => level;
    public bool IsEmpty => string.IsNullOrEmpty(characterId);

    public UnitEntry(string id, int unitLevel)
    {
        characterId = id;
        level = unitLevel;
    }

    public void LevelUp()
    {
        level++;
    }
}
