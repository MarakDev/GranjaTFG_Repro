using UnityEngine;


public class Wolf_ChaseState : State
{
    Vector2 direction;

    bool wolfUnderAttack;

    public Wolf_ChaseState(WolfController wolfController, StateMachine StateMachine) : base(StateMachine)
    {
        this.wC = wolfController;
    }

    public override void EnterState()
    {
        wC.SheepSelect();
        wC.currentSpeed = wC.wolfSpeed;
    }

    public override void FrameUpdate()
    {
        wolfUnderAttack = Physics2D.OverlapCircle(wC.transform.position, wC.dogActionRange, wC.dogLayer);

        wC.UnderDogAttack(wolfUnderAttack);

        SheepChaseDirection();

        if (Physics2D.OverlapCircle(wC.transform.position, 1, wC.sheepLayer) || wC.activeSheep.gameObject.layer == LayerMask.NameToLayer("SheepIsCaged"))
        {
            wC.StateMachine.ChangeState(wC.IdleState);
            return;
        }

        if (wolfUnderAttack)
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

        direction = (wC.activeSheep.transform.position - wC.transform.position).normalized;


    }

}
