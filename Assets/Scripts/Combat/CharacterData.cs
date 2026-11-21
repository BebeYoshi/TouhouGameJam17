using UnityEngine;

[System.Serializable]
public class CharacterData
{
    [Header("Identity")]
    [SerializeField] private string id = "";
    [SerializeField] private string displayName = "";

    [Header("Combat Profile")]
    [SerializeField] private CharacterRole role = CharacterRole.DPS;
    [SerializeField] private AttackType attackType = AttackType.Melee;

    [Header("Basic Stats")]
    [SerializeField, Min(1f)] private float maxHealth = 600f;
    [SerializeField, Min(1f)] private float attack = 55f;
    [SerializeField, Min(0f)] private float defense = 30f;

    [Header("Booze")]
    [SerializeField, Min(1f)] private float maxBooze = 100f;
    [SerializeField, Min(0f)] private float boozePerSecond = 1f;
    [SerializeField, Min(0f)] private float boozeOnHit = 5f;
    [SerializeField, Min(0f)] private float boozeOnHurt = 5f;
    [SerializeField, Min(0f)] private float boozeOnKill = 10f;

    [Header("Scaling")]
    [SerializeField, Min(0f)] private float basicMultiplier = 1f;
    [SerializeField, Min(0f)] private float skillMultiplier = 1.5f;
    [SerializeField, Min(0f)] private float ultMultiplier = 3f;

    public string Id => id;
    public string DisplayName => displayName;

    public CharacterRole Role => role;
    public AttackType AttackType => attackType;

    public float MaxHealth => maxHealth;
    public float Attack => attack;
    public float Defense => defense;

    public float MaxBooze => maxBooze;
    public float BoozePerSecond => boozePerSecond;
    public float BoozeOnHit => boozeOnHit;
    public float BoozeOnHurt => boozeOnHurt;
    public float BoozeOnKill => boozeOnKill;

    public float BasicMultiplier => basicMultiplier;
    public float SkillMultiplier => skillMultiplier;
    public float UltMultiplier => ultMultiplier;

}
