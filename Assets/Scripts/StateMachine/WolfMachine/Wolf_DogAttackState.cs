using UnityEngine;


public class Wolf_DogAttackState : State
{
    Vector2 direction;
    bool wolfUnderAttack;

    public Wolf_DogAttackState(WolfController wolfController, StateMachine StateMachine) : base(StateMachine)
    {
        this.wC = wolfController;
    }

    public override void EnterState()
    {
        wC.rb.velocity = Vector2.zero;

        direction = wC.RandomPosition();

        wC.currentSpeed = wC.wolfSpeed * 2;
    }

    public override void FrameUpdate()
    {
        wolfUnderAttack = Physics2D.OverlapCircle(wC.transform.position, wC.dogActionRange, wC.dogLayer);

        wC.UnderDogAttack(wolfUnderAttack);

        if (!wolfUnderAttack)
        {
            wC.StateMachine.ChangeState(wC.ChaseState);
            return;
        }

        if (wC.currentLife <= 0)
        {
            wC.StateMachine.ChangeState(wC.DefeatState);
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
