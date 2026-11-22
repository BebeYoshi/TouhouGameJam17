using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "StageEncounter", menuName = "Scriptable Objects/Battle/Stage Encounter")]
public class StageEncounter : ScriptableObject
{
    [SerializeField] private string stageId = "";
    [SerializeField] private bool isBoss;
    [SerializeField] private List<UnitEntry> enemies = new List<UnitEntry>();

    public string StageId => stageId;
    public bool IsBoss => isBoss;
    public IReadOnlyList<UnitEntry> Enemies => enemies;
}
