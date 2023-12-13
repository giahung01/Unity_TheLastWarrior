using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    #region PlayerState
    public PlayerIdleState idleState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerGroundState groundState { get; private set; }
    public PlayerFallState fallState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerGroundState rollState { get; private set; }
    public PlayerAttackState attackState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerDeadState deadState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerHurtState hurtState { get; private set; }
    #endregion

    #region Player setting

    [Header("Wall Slide")]
    [SerializeField] private float wallSlideGravity;
    [SerializeField] private float wallSlideJumpForce;
    [SerializeField] private bool isSliding;

    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private bool canDoubleJump;
    [SerializeField] private bool canHighJump;
    [SerializeField] private float moveSpeedInAir;

    [Header("Fall")]
    [SerializeField] private float fallGravity;
    [SerializeField] private float defaultGravity;

    [SerializeField] private bool isFirstAttack;

    #endregion

    private PlayerStat playerStat;
    float xxx = 3;

    #region Getter Setter
    public float JumpForce() => jumpForce;
    public float MoveSpeedInAir() => moveSpeedInAir;
    public bool IsFirstAttack() => isFirstAttack;
    public bool CanDoubleJump() => canDoubleJump;
    public void SetCanDoubleJump(bool _canDoubleJump) => canDoubleJump = _canDoubleJump;
    public bool CanHighJump() => canHighJump;
    public void SetCanHighJump(bool _canHighJump) => canHighJump = _canHighJump;
    public void SetIsFirstAttack(bool _isFirstAttack) => isFirstAttack = _isFirstAttack;
    public float WallSlideJumpForce() => wallSlideJumpForce;
    public bool IsSliding() => isSliding;
    public void SetIsSliding(bool _value) => isSliding = _value;
    public float WallSlideGravity() => wallSlideGravity;
    public PlayerStat PlayerStat => playerStat;
    #endregion

    protected override void Start()
    {
        base.Start();

        #region Init Player State
        idleState = new PlayerIdleState("Player_Idle");
        moveState = new PlayerMoveState("Player_Run");
        jumpState = new PlayerJumpState("Player_Jump");
        fallState = new PlayerFallState("Player_Fall");
        rollState = new PlayerRollState("Player_Roll");
        attackState = new PlayerAttackState("Player_Attack1");
        wallSlideState = new PlayerWallSlideState("Player_WallSlide");
        deadState = new PlayerDeadState("Player_Death");
        wallJumpState = new PlayerWallJumpState("Player_Jump");
        hurtState = new PlayerHurtState("Player_Hurt");
        #endregion

        playerStat = GetComponent<PlayerStat>();

        stateMachine.InitState(idleState);

        isFacingRight = true;
        canDoubleJump = true;
        canHighJump = true;
    }
    protected override void Update()
    {
        base.Update();

        //stateMachine.currentState.Update();
        moveDir = Input.GetAxisRaw("Horizontal");

      
        xxx -= Time.deltaTime;
        if (xxx < 0)
        {
            playerStat.TakeDamage(10);
            xxx = 3;
        }
    }
    public void ActivateFallGravity(bool _isActivate)
    {
        if (_isActivate)
        {
            rb.gravityScale = fallGravity;
        }
        else
        {
            rb.gravityScale = defaultGravity;
        }
    }

    public override void TakeDamage(Entity _attacker)
    {
        base.TakeDamage(_attacker);
        stateMachine.ChangeState(hurtState);
    }
}
