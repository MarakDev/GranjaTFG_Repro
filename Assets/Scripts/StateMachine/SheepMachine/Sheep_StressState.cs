using UnityEngine;

public class Sheep_StressState : State
{
    float timer = 0;
    float maxDuration = 0;

    public Sheep_StressState(SheepController sheepController, StateMachine StateMachine) : base(StateMachine)
    {
        this.sC = sheepController;
    }

    public override void EnterState()
    {

        maxDuration = Random.Range(sC.idleTime - 3, sC.idleTime + 3);

        if(maxDuration < 1)
            maxDuration = 3;
    }

    public override void FrameUpdate()
    {
        timer += Time.deltaTime;

        if (timer >= maxDuration)
        {
            sC.StateMachine.ChangeState(sC.WalkState);
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
