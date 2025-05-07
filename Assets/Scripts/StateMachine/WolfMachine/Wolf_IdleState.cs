using UnityEngine;

public class Wolf_IdleState : State
{
    float timer = 0;
    float maxDuration = 0;
    Vector2 direction;

    bool wolfUnderAttack;

    public Wolf_IdleState(WolfController wolfController, StateMachine StateMachine) : base(StateMachine)
    {
        this.wC = wolfController;
    }

    public override void EnterState()
    {
        wC.rb.velocity = Vector2.zero;

        direction = wC.RandomPosition();

        maxDuration = Random.Range(0, wC.idleTime);

        wC.currentSpeed = wC.wolfSpeed * 0.25f;
    }

    public override void FrameUpdate()
    {
        timer += Time.deltaTime;

        wolfUnderAttack = Physics2D.OverlapCircle(wC.transform.position, wC.dogActionRange, wC.dogLayer);

        wC.UnderDogAttack(wolfUnderAttack);

        if (timer >= maxDuration)
        {
            wC.StateMachine.ChangeState(wC.ChaseState);
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
        timer = 0;
    }

    public override void AnimationEnter()
    {

    }

    public override void AnimationExit()
    {

    }


}
