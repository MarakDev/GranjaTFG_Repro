using UnityEngine;

public class Sheep_IdleState : State
{
    float timer = 0;
    float maxDuration = 0;

    public Sheep_IdleState(SheepController sheepController, StateMachine StateMachine) : base(StateMachine)
    {
        this.sC = sheepController;
    }

    public override void EnterState()
    {
        sC.rb.velocity = Vector2.zero;

        maxDuration = Random.Range(sC.idleTime - 2, sC.idleTime + 2);

        if(maxDuration < 1)
            maxDuration = 3;

        sC.direction = Vector2.zero;
    }

    public override void FrameUpdate()
    {
        timer += Time.deltaTime;

        if (timer >= maxDuration)
        {
            sC.StateMachine.ChangeState(sC.WalkState);
            return;
        }

        if (sC.DogIsNear())
        {
            sC.StateMachine.ChangeState(sC.ChaseDogState);
            return;
        }

        if (sC.WolfIsNear())
        {
            sC.StateMachine.ChangeState(sC.ChaseWolfState);
            return;
        }

        if (sC.SheepFollowLogic())
        {
            sC.StateMachine.ChangeState(sC.FollowSheepState);
            return;
        }
    }

    public override void PhysicsUpdate()
    {

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
