
using UnityEngine;

public class Wolf_DefeatState : State
{
    Vector2 direction;
    public Wolf_DefeatState(WolfController wolfController, StateMachine StateMachine) : base(StateMachine)
    {
        this.wC = wolfController;
    }

    public override void EnterState()
    {
        base.EnterState();

        wC.rb.velocity = Vector2.zero;


        wC.currentSpeed = wC.wolfSpeed;
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
        wC.rb.velocity = new Vector2(0 * wC.currentSpeed, 1 * wC.currentSpeed);

    }

    public override void ExitState()
    {

    }

    public override void AnimationEnter()
    {
        wC.animator.Play("Baloon");

    }

    public override void AnimationExit()
    {

    }

}
