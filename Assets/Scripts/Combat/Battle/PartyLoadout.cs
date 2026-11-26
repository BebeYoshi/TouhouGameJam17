using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "PartyLoadout", menuName = "Scriptable Objects/Battle/Party Loadout")]
public class PartyLoadout : ScriptableObject
{
    [SerializeField] private List<UnitEntry> members = new List<UnitEntry>();

    public IReadOnlyList<UnitEntry> Members => members;

    public void SetParty(List<UnitEntry> newMembers)
    {
    members.Clear();
    members.AddRange(newMembers);
    }

    public UnitEntry GetMember(string characterId)
    {
        return members.Find(
            member => member.CharacterId == characterId
        );
    }

    public void AddMember(UnitEntry member)
    {
        if (member == null)
            return;

        if (members.Exists(m => m.CharacterId == member.CharacterId))
            return;
        members.Add(member);
    }
    public void LevelUpMember(string characterId)
    {
        for (int i = 0; i < members.Count; i++)
        {
            if (members[i].CharacterId == characterId)
            {
                members[i].LevelUp();
                return;
            }
        }
    }
}
