using UnityEngine;

public class Common3BattleState : EnemyBattleState
{
    Common3 common3;

    public Common3BattleState(Enemy enemy, string animName) : base(enemy, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        common3 = (Common3)enemy;
        //timer = common3.ToAttackStateTimer;
        timer = common3.ToAttackStateCountdown;

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        MoveController();

        if (timer < 0 && common3.DetectPlayer()) 
        {
            stateMachine.ChangeState(common3.attackState);
        }
    }
    private void MoveController()
    {
        if (Vector2.Distance(player.transform.position, common3.transform.position) < common3.DistanceStopMoving)
        {
            // move backward when player appproaches enemy
            common3.ChangeVelocity(common3.MoveBackwardSpeed * common3.EntityDir * -1, common3.rb.velocity.y);
        }
        if (Vector2.Distance(player.transform.position, common3.transform.position) > common3.DetectPlayerDistance)
        {
            // Chasing player when out detect player distance  
            common3.ChangeVelocity(common3.MoveSpeed * common3.EntityDir, common3.rb.velocity.y);
        }
      
    }
}
