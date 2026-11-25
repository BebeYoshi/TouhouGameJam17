using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class PlayerSave
{
    public int power;
    public int point;
    public List<string> clearedStages = new List<string>();
    public List<string> pullableCharacterIds = new List<string>();
    public List<UnitEntry> rosterMembers = new List<UnitEntry>();
}
