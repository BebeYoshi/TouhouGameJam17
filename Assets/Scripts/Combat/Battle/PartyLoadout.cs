using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PartyLoadout", menuName = "Scriptable Objects/Battle/Party Loadout")]
public class PartyLoadout : ScriptableObject
{
    [SerializeField] private List<UnitEntry> members = new List<UnitEntry>();

    public IReadOnlyList<UnitEntry> Members => members;
}
