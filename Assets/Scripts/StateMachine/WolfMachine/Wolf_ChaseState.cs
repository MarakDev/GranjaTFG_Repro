using UnityEngine;


public class Wolf_ChaseState : State
{
    Vector2 direction;

    bool wolfUnderFire;

    public Wolf_ChaseState(WolfController wolfController, StateMachine StateMachine) : base(StateMachine)
    {
        this.wC = wolfController;
    }

    public override void EnterState()
    {
        wC.SheepSelect();

    }

    public override void FrameUpdate()
    {
        wolfUnderFire = Physics2D.OverlapCircle(wC.transform.position, wC.dogActionRange, wC.dogLayer);

        wC.UnderDogFire(wolfUnderFire);

        SheepChaseDirection();

        if (Physics2D.OverlapCircle(wC.transform.position, 1, wC.sheepLayer) || wC.activeSheep.gameObject.layer == LayerMask.NameToLayer("SheepIsCaged"))
        {
            wC.StateMachine.ChangeState(wC.IdleState);
            return;
        }

        if (wolfUnderFire)
        {
            wC.StateMachine.ChangeState(wC.DogAttackState);
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

    private void SheepChaseDirection()
    {
        if (!wolfUnderFire)
        {
            direction = (wC.activeSheep.transform.position - wC.transform.position).normalized;
            wC.currentSpeed = wC.wolfSpeed;
        }
    }

}
