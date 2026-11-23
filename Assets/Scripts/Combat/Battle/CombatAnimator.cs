using UnityEngine;
using System;

public class CombatAnimator : MonoBehaviour
{
    private const string IdleState = "idle";
    private const string WalkState = "walk";
    private const string MeleeState = "attackMelee";
    private const string RangedState = "attackRanged"; 
    private const string SkillState = "skill";
    private const string UltState = "ult";

    [SerializeField, Min(0f)] private float attackLockTime = 0.35f;
    [SerializeField, Min(0f)] private float skillLockTime = 0.6f;
    [SerializeField, Min(0f)] private float ultLockTime = 0.9f;
    [SerializeField, Min(0f)] private float walkThreshold = 0.05f;

    private CombatStats stats;
    private Animator animator;
    private Rigidbody2D body;
    private Transform visual;
    private string currentState;
    private float lockedUntil;

    private void Awake()
    {
        stats = GetComponent<CombatStats>();
        animator = GetComponentInChildren<Animator>();
        body = GetComponent<Rigidbody2D>();
        visual = animator != null ? animator.transform : null;
    }

    private void OnEnable()
    {
        stats.AttackPerformed += OnAttackPerformed;
        stats.SkillUsed += OnSkillUsed;
        stats.UltUsed += OnUltUsed;
        stats.Died += OnDied;
        stats.Revived += OnRevived;
    }

    private void OnDisable()
    {
        stats.AttackPerformed -= OnAttackPerformed;
        stats.SkillUsed -= OnSkillUsed;
        stats.UltUsed -= OnUltUsed;
        stats.Died -= OnDied;
        stats.Revived -= OnRevived;
    }

    private void Update()
    {
        if (animator == null || !stats.IsAlive || Time.time < lockedUntil)
        {
            return;
        }

        Vector2 velocity = body != null ? body.linearVelocity : Vector2.zero;
        bool isWalking = velocity.sqrMagnitude > walkThreshold * walkThreshold;

        if (isWalking)
        {
            Face(velocity.x);
        }

        Play(isWalking ? WalkState : IdleState, 0f);
    }

    private void OnAttackPerformed(CombatStats target)
    {
        FaceTarget(target);
        bool ranged = stats.Data.AttackType == AttackType.Ranged;
        Play(ranged && HasState(RangedState) ? RangedState : MeleeState, attackLockTime);
    }

    private void OnSkillUsed(CombatStats target)
    {
        FaceTarget(target);
        Play(SkillState, skillLockTime);
    }

    private void OnUltUsed(CombatStats target)
    {
        FaceTarget(target);
        Play(UltState, ultLockTime);
    }

    private void OnDied(CombatStats unit)
    {
        if (visual != null)
        {
            visual.gameObject.SetActive(false);
        }
    }

    private void OnRevived(CombatStats unit)
    {
        if (visual != null)
        {
            visual.gameObject.SetActive(true);
            animator = visual.GetComponent<Animator>();
            currentState = null;
        }
    }

    private void FaceTarget(CombatStats target)
    {
        if (target != null)
        {
            Face(target.transform.position.x - transform.position.x);
        }
    }

    private void Face(float directionX)
    {
        if (visual == null || Mathf.Abs(directionX) < 0.01f)
        {
            return;
        }

        Vector3 scale = visual.localScale;
        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(directionX);
        visual.localScale = scale;
    }

    private void Play(string state, float lockTime)
    {
        if (animator == null || !HasState(state))
        {
            return;
        }

        lockedUntil = Time.time + lockTime;

        if (lockTime <= 0f && currentState == state)
        {
            return;
        }

        animator.Play(state, 0, 0f);
        currentState = state;
    }

    private bool HasState(string state)
    {
        return animator != null && animator.HasState(0, Animator.StringToHash(state));
    }
}
