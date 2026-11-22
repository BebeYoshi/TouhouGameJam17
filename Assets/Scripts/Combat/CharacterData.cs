using UnityEngine;

[System.Serializable]
public class CharacterData
{
    [Header("Identity")]
    [SerializeField] public string id = "";
    [SerializeField] public string displayName = "";
    [SerializeField] private Species species = Species.Youkai;
    [SerializeField] public Sprite sprite;
    [SerializeField] public bool isPullable;
    [SerializeField] public float pullChance;

    [Header("Combat Profile")]
    [SerializeField] private CharacterRole role = CharacterRole.DPS;
    [SerializeField] private AttackType attackType = AttackType.Melee;

    [Header("Combat Timing")]
    [SerializeField, Min(0.1f)] private float attackSpeed = 1f;
    [SerializeField, Min(0.1f)] private float attackRange = 1.5f;
    [SerializeField, Min(0f)] private float moveSpeed = 2f;
    [SerializeField, Min(0f)] private float skillCooldown = 8f;

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

    [Header("Kit")]
    [SerializeField] private CombatAbility basicAttackAbility;
    [SerializeField] private PassiveAbility passive;
    [SerializeField] private CombatAbility skill;
    [SerializeField] private CombatAbility ultAbility;

    [Header("Scaling")]
    [SerializeField, Min(0f)] private float basicMultiplier = 1f;
    [SerializeField, Min(0f)] private float skillMultiplier = 1.5f;
    [SerializeField, Min(0f)] private float ultMultiplier = 3f;
    [SerializeField] private bool hasUlt = true;

    public string Id => id;
    public string DisplayName => displayName;
    public Species Species => species;

    public CharacterRole Role => role;
    public AttackType AttackType => attackType;

    public float AttackSpeed => attackSpeed;
    public float AttackRange => attackRange;
    public float MoveSpeed => moveSpeed;
    public float SkillCooldown => skillCooldown;

    public float MaxHealth => maxHealth;
    public float Attack => attack;
    public float Defense => defense;

    public float MaxBooze => maxBooze;
    public float BoozePerSecond => boozePerSecond;
    public float BoozeOnHit => boozeOnHit;
    public float BoozeOnHurt => boozeOnHurt;
    public float BoozeOnKill => boozeOnKill;

    public CombatAbility BasicAttackAbility => basicAttackAbility;
    public PassiveAbility Passive => passive;
    public CombatAbility Skill => skill;
    public CombatAbility UltAbility => ultAbility;

    public float BasicMultiplier => basicMultiplier;
    public float SkillMultiplier => skillMultiplier;
    public float UltMultiplier => ultMultiplier;
    public bool HasUlt => hasUlt;

}
