using UnityEngine;

public class Sheep_EatState : State
{
    float timer = 0;
    float maxDuration = 0;

    public Sheep_EatState(SheepController sheepController, StateMachine StateMachine) : base(StateMachine)
    {
        this.sC = sheepController;
    }

    public override void EnterState()
    {
        base.EnterState();

        maxDuration = Random.Range(sC.eatTime - 3, sC.eatTime + 3);

        if(maxDuration < 1)
            maxDuration = 3;
    }

    public override void FrameUpdate()
    {
        timer += Time.deltaTime;

        sC.currentStomachCapacity += Time.deltaTime;

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
        base.ExitState();
        timer = 0;
    }

    public override void AnimationEnter()
    {
        sC.animator.Play("EatingTransition");
    }

    public override void AnimationExit()
    {

    }

}
