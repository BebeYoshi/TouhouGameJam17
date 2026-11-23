using UnityEngine;

public class CombatAnimator : MonoBehaviour
{
    private static readonly int IdleHash = Animator.StringToHash("idle");
    private static readonly int WalkHash = Animator.StringToHash("walk");
    private static readonly int MeleeHash = Animator.StringToHash("attackMelee");
    private static readonly int RangedHash = Animator.StringToHash("attackRanged");
    private static readonly int SkillHash = Animator.StringToHash("skill");
    private static readonly int UltHash = Animator.StringToHash("ult");

    private float baseOffsetX;

    [SerializeField, Min(0f)] private float transitionDuration = 0.08f;
    [SerializeField, Min(0f)] private float walkThreshold = 0.05f;

    private CombatStats stats;
    private Animator animator;
    private Rigidbody2D body;
    private Transform visual;

    private int currentStateHash;
    private int lockedStateHash;
    private bool actionEnteredState;
    private bool isUltLocked;

    private void Awake()
    {
        stats = GetComponent<CombatStats>();
        animator = GetComponentInChildren<Animator>();
        body = GetComponent<Rigidbody2D>();
        visual = animator != null ? animator.transform : null;
        baseOffsetX = visual != null ? visual.localPosition.x : 0f;
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
        if (animator == null || !stats.IsAlive)
        {
            return;
        }

        if (IsActionLocked())
        {
            return;
        }

        Vector2 velocity = body != null ? body.linearVelocity : Vector2.zero;
        bool isWalking = velocity.sqrMagnitude > walkThreshold * walkThreshold;

        if (isWalking)
        {
            Face(velocity.x);
        }

        Play(isWalking ? WalkHash : IdleHash, restartIfSame: false);
    }

    private void OnAttackPerformed(CombatStats target)
    {
        if (IsActionLocked())
        {
            return;
        }

        FaceTarget(target);
        bool ranged = stats.Data.AttackType == AttackType.Ranged;
        int targetHash = (ranged && HasState(RangedHash)) ? RangedHash : MeleeHash;
        StartLockedAction(targetHash, isUlt: false);
    }

    private void OnSkillUsed(CombatStats target)
    {
        if (IsActionLocked())
        {
            return;
        }

        FaceTarget(target);
        StartLockedAction(SkillHash, isUlt: false);
    }

    private void OnUltUsed(CombatStats target)
    {
        FaceTarget(target);
        StartLockedAction(UltHash, isUlt: true);
    }

    private void StartLockedAction(int stateHash, bool isUlt)
    {
        if (!HasState(stateHash))
        {
            return;
        }

        lockedStateHash = stateHash;
        actionEnteredState = false;
        isUltLocked = isUlt;
        Play(stateHash, restartIfSame: true);
    }

    private bool IsActionLocked()
    {
        if (lockedStateHash == 0)
        {
            return false;
        }

        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        bool inTransitionToLocked = animator.IsInTransition(0) &&
                                    animator.GetNextAnimatorStateInfo(0).shortNameHash == lockedStateHash;
        bool isInLockedState = currentInfo.shortNameHash == lockedStateHash;

        if (!actionEnteredState)
        {
            if (inTransitionToLocked || isInLockedState)
            {
                actionEnteredState = true;
            }
            return true;
        }

        if (inTransitionToLocked)
        {
            return true;
        }

        if (isInLockedState && currentInfo.normalizedTime < 1f)
        {
            return true;
        }

        lockedStateHash = 0;
        actionEnteredState = false;
        isUltLocked = false;
        return false;
    }

    private void Play(int stateHash, bool restartIfSame)
    {
        if (animator == null || !HasState(stateHash))
        {
            return;
        }

        if (!restartIfSame && currentStateHash == stateHash)
        {
            return;
        }

        float blend = (stateHash == UltHash) ? 0f : transitionDuration;
        animator.CrossFadeInFixedTime(stateHash, blend, 0, 0f);
        currentStateHash = stateHash;
    }

    private void OnDied(CombatStats unit)
    {
        ResetLocks();
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
            ResetLocks();
        }
    }

    private void ResetLocks()
    {
        lockedStateHash = 0;
        currentStateHash = 0;
        actionEnteredState = false;
        isUltLocked = false;
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

        float sign = Mathf.Sign(directionX);

        Vector3 scale = visual.localScale;
        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(directionX);
        visual.localScale = scale;

        Vector3 pos = visual.localPosition;
        pos.x = baseOffsetX * -sign;
        visual.localPosition = pos;
    }

    private bool HasState(int stateHash)
    {
        return animator != null && animator.HasState(0, stateHash);
    }
}
