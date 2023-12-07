using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]

public class Entity : MonoBehaviour
{
    #region Component
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }
    #endregion

    #region Entity setting
    [Header("Move")]
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float moveDir;
    [SerializeField] protected float entityDir;
    [SerializeField] protected bool isFacingRight;

    [Header("Ground Check")]
    [SerializeField] protected bool isGround;
    [SerializeField] protected float groundCheckLength;
    [SerializeField] protected LayerMask whatIsGround;


    [Header("Wall Check")]
    [SerializeField] protected bool isWall;
    [SerializeField] protected float wallCheckLength;

    [Header("Roll")]
    [SerializeField] protected float rollSpeed;
    [SerializeField] protected bool isRolling;


    [Header("Attack")]
    [SerializeField] protected float attackCountdown;
    [SerializeField] protected Transform attackCheck;
    [SerializeField] protected float attackCheckRadius;
    private float hurtTime = 0.2f;
    #endregion

    public StateMachine stateMachine { get; private set; }

    #region Getter Setter
    public float MoveSpeed() => moveSpeed;
    public float MoveDir() => moveDir;
    public float SetMoveDir(float _value) => moveDir = _value;
    public float RollSpeed() => rollSpeed;
    public bool IsGround() => isGround;
    public void SetIsGround(bool _isGround) => isGround = _isGround;
    public bool IsWall() => isWall;
    public bool IsRolling() => isRolling;
    public void SetIsRolling(bool _isRolling) => isRolling = _isRolling;
    public bool IsFacingRight() => isFacingRight;
    public float EntityDir() => entityDir;
    public float AttackCountDown() => attackCountdown;
    public Transform AttackCheck => attackCheck;
    public float AttackCheckRadius => attackCheckRadius;
    public float HurtTime => hurtTime;
    #endregion

    protected virtual void Awake()
    {
        stateMachine = new StateMachine();
    }

    protected virtual void Start()
    {
        #region Get Component

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

        rb.gravityScale = 4;
        #endregion
    }

    protected virtual void Update()
    {
        if (stateMachine != null)
            stateMachine.currentState.Update();

        entityDir = isFacingRight ? 1 : -1;

        CheckingGround();
        CheckingWall();
    }

    private void CheckingGround()
    {
        isGround = Physics2D.Raycast(
            transform.position,
            Vector2.down,
            groundCheckLength,
            whatIsGround
            );
    }

    private void CheckingWall()
    {
        isWall = Physics2D.Raycast(transform.position,
            Vector2.right * entityDir,
            wallCheckLength,
            whatIsGround);
    }
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(
            transform.position,
            new Vector2(transform.position.x, transform.position.y - groundCheckLength)
            );
        Gizmos.DrawLine(
            transform.position,
            new Vector2(transform.position.x + wallCheckLength * entityDir, transform.position.y)
            );
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }

    public void ChangeRotation()
    {
        transform.Rotate(0, 180, 0);
    }

    public void SetIsFacingRight(bool _isFacingRight)
    {
        isFacingRight = _isFacingRight;
    }

    public void ZeroVelocity() => rb.velocity = Vector2.zero;
    public void ChangeVelocity(Vector2 _newVelocity) => rb.velocity = _newVelocity;
    public void ChangeVelocity(float x, float y)
    {
        rb.velocity = new Vector2(x, y);
    }
    
    public virtual void TakeDamage(Entity _attacker)
    {
        Vector2 hit = new Vector2(4 * _attacker.EntityDir(), 4);
        ChangeVelocity(hit);
    }

    public virtual void CauseDamage(Entity _hitEntity)
    {
        Debug.Log("Cause damage");
        _hitEntity.TakeDamage(this);
    }
}