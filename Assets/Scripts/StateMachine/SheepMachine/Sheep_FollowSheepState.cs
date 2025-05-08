using UnityEngine;

public class Sheep_FollowSheepState : State
{
    float timer = 0;
    float maxDuration = 0;

    public Sheep_FollowSheepState(SheepController sheepController, StateMachine StateMachine) : base(StateMachine)
    {
        this.sC = sheepController;
    }

    public override void EnterState()
    {
        base.EnterState();

        sC.rb.velocity = Vector2.zero;

    }

    public override void FrameUpdate()
    {

        if (sC.WolfIsNear())
        {
            sC.StateMachine.ChangeState(sC.ChaseWolfState);
            return;
        }

        if (!sC.SheepFollowLogic())
        {
            sC.StateMachine.ChangeState(sC.WalkState);
            return;
        }

        sC.UpdateSpriteDirection();
    }

    public override void PhysicsUpdate()
    {
        sC.rb.velocity = new Vector2(sC.direction.x * sC.currentSpeed, sC.direction.y * sC.currentSpeed);

    }

    public override void ExitState()
    {
        base .ExitState();

        timer = 0;
    }

    public override void AnimationEnter()
    {
        sC.animator.Play("DogRun");
        sC.transform.Find("Particle_Grass").GetComponent<ParticleSystem>().Play();

    }

    public override void AnimationExit()
    {
        sC.transform.Find("Particle_Grass").GetComponent<ParticleSystem>().Stop();

    }

}
