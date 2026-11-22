using UnityEngine;

public abstract class CombatAbility : ScriptableObject
{
    public abstract bool Execute(CombatStats owner, CombatStats target);
}
