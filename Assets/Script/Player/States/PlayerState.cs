using UnityEngine;

public class PlayerState : BaseState
{
    protected Player player = PlayerManager.instance.player;

    public PlayerState(string animName) : base(animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateMachine = player.stateMachine;
        player.anim.Play(animName);
    }

    public override void Update()
    {
        base.Update();
        Flip();
    }

    private void Flip()
    {
        if (player.MoveDir != 0 && !player.IsSliding)
        {
            if (player.MoveDir != player.EntityDir)
            {
                player.SetIsFacingRight(player.MoveDir == -1 ? false : true);
                player.ChangeRotation();
            }
        }
    }

    protected virtual void MoveController(float moveSpeed)
    {
        Vector2 speed = new Vector2(
          moveSpeed * player.MoveDir,
          player.rb.velocity.y
          );
        player.ChangeVelocity(speed);
    }

}
