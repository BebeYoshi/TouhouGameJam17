using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "StageEncounter", menuName = "Scriptable Objects/Battle/Stage Encounter")]
public class StageEncounter : ScriptableObject
{
    [SerializeField] private string stageId = "";
    [SerializeField] private bool isBoss;
    [SerializeField] private List<UnitEntry> enemies = new List<UnitEntry>();

    [Header("Rewards")]
    [SerializeField, Min(0)] private int powerReward;
    [SerializeField, Min(0)] private int pointReward;
    [SerializeField, Min(0)] private int rerunPowerReward;
    [SerializeField, Min(0)] private int rerunPointReward;

    public string StageId => stageId;
    public bool IsBoss => isBoss;
    public IReadOnlyList<UnitEntry> Enemies => enemies;
    public int PowerReward => powerReward;
    public int PointReward => pointReward;
    public int RerunPowerReward => rerunPowerReward;
    public int RerunPointReward => rerunPointReward;
}
