using UnityEngine;


public class Wolf_DogAttackState : State
{
    Vector2 direction;
    bool wolfUnderFire;

    public Wolf_DogAttackState(WolfController wolfController, StateMachine StateMachine) : base(StateMachine)
    {
        this.wC = wolfController;
    }

    public override void EnterState()
    {
        

    }

    public override void FrameUpdate()
    {
        wolfUnderFire = Physics2D.OverlapCircle(wC.transform.position, wC.dogActionRange, wC.dogLayer);

        wC.UnderDogFire(wolfUnderFire);

        if (!wolfUnderFire)
        {
            wC.StateMachine.ChangeState(wC.ChaseState);
            return;
        }

        if (wC.currentLife <= 0)
        {
            wC.StateMachine.ChangeState(wC.AfraidState);
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
