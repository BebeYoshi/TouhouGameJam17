// Reimu
using UnityEngine;

[CreateAssetMenu(fileName = "SpeciesDamagePassive", menuName = "Scriptable Objects/Passives/Species Damage")]
public class SpeciesDamagePassive : PassiveAbility
{
    [SerializeField] private Species targetSpecies = Species.Youkai;
    [SerializeField, Min(0f)] private float damageBonus = 0.05f;

    public override float GetDamageBonus(CombatStats owner, CombatStats target)
    {
        return target.Data.Species == targetSpecies ? damageBonus : 0f;
    }
}
