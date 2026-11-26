using UnityEngine;

public abstract class CombatAbility : ScriptableObject
{
    public abstract bool Execute(CombatStats owner, CombatStats target, AttackKind kind);
    [SerializeField, Min(0f)] private float castDelay = 0f;
    public float CastDelay => castDelay;

    protected static float ResolveMultiplier(CombatStats owner, MultiplierSource source, float customMultiplier)
    {
        switch (source)
        {
            case MultiplierSource.Basic:
                return owner.Data.BasicMultiplier;

            case MultiplierSource.Skill:
                return owner.Data.SkillMultiplier;

            case MultiplierSource.Ult:
                return owner.Data.UltMultiplier;

            default:
                return customMultiplier;
        }
    }
}