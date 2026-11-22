using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ProjectileBarrier : MonoBehaviour
{
    [SerializeField, Min(0.05f)] private float lifetime = 3f;

    private BattleSide side;

    public void Setup(CombatStats owner)
    {
        side = owner.Side;
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        SkillHitbox projectile = other.GetComponentInParent<SkillHitbox>();

        if (projectile != null)
        {
            if (projectile != null)
            {
                projectile.TryBlock(side);
            }
        }
    }
}
