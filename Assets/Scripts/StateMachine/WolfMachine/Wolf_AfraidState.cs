
using UnityEngine;

public class Wolf_AfraidState : State
{
    Vector2 direction;
    public Wolf_AfraidState(WolfController wolfController, StateMachine StateMachine) : base(StateMachine)
    {
        this.wC = wolfController;
    }

    public override void EnterState()
    {
        wC.rb.velocity = Vector2.zero;

        float dirX = Random.Range(-1f, 1f);
        float dirY = 1 - dirX;

        if (dirX < 0f)
            dirY = 1 + dirX;

        direction = new Vector2(dirX, dirY);
        wC.currentSpeed = wC.wolfSpeed * 2;
    }
    public override void FrameUpdate()
    {
        if (wC.barrierWolf)
        {
            wC.StateMachine.ChangeState(wC.RestartState);
            return;
        }
    }

    public override void PhysicsUpdate()
    {
        wC.rb.velocity = new Vector2(direction.x * wC.currentSpeed, direction.y * wC.currentSpeed);

    }

    public override void ExitState()
    {

    }

    public override void AnimationEnter()
    {

    }

    public override void AnimationExit()
    {

    }

}
