using UnityEngine;

public static class DamageCalculator
{

    private const float DefenseWeight = 1.0f;
    private const float MinDamageFraction = 0.10f;
    private const float MaxDamageFraction = 2.0f;

    public static float CalculateDamage(float attack, float defense, float multiplier) 
    {
        
        float rawDamage = attack * multiplier;
        float damageFraction = GetDamageFraction(attack, defense);
        float finalDamage = rawDamage * damageFraction;

        return finalDamage; 
    }

    private static float GetDamageFraction(float attack, float defense) 
    {

        if (attack <= 0f) 
        {
            return 0f;
        }

        if (defense < 0f)
        {
            float defenseIntegrity = attack / (attack - DefenseWeight * defense);
            return MaxDamageFraction - (MaxDamageFraction - 1f) * defenseIntegrity;
        }


        float fraction = attack / (attack + (DefenseWeight * defense));
        return Mathf.Max(MinDamageFraction, fraction);
    }

}