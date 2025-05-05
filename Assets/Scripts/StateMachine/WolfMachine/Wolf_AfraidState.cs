
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

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
        wC.rb.velocity = new Vector2(direction.x * wC.wolfSpeed + 3, direction.y * wC.wolfSpeed + 3);

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
