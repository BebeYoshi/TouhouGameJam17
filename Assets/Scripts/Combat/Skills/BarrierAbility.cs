using UnityEngine;

[CreateAssetMenu(fileName = "BarrierAbility", menuName = "Scriptable Objects/Abilities/Barrier Ability")]
public class BarrierAbility : CombatAbility
{
    [SerializeField] private ProjectileBarrier barrierPrefab;
    [SerializeField, Min(0f)] private float distanceInFront = 1f;

     public override bool Execute(CombatStats owner, CombatStats target, AttackKind kind)
    {
        if (barrierPrefab == null)
        {
            Debug.LogError($"{name}: barrier prefab not assigned.", this);
            return false;
        }

        if (target == null || !target.IsAlive)
        {
            return false;
        }

        Vector2 direction = (target.transform.position - owner.transform.position).normalized;
        Vector3 position = owner.transform.position + (Vector3)(direction * distanceInFront);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        ProjectileBarrier barrier = Instantiate(barrierPrefab, position, Quaternion.Euler(0f, 0f, angle), owner.transform);
        barrier.Setup(owner);
        return true;
    }
}
