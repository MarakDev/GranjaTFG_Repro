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

        maxDuration = Random.Range(sC.restartIdleCooldown - 3, sC.restartIdleCooldown + 3);

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
