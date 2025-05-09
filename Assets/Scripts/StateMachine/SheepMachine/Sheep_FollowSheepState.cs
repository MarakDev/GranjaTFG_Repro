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

        if (sC.rb.velocity.magnitude > sC.currentSpeed)
        {
            Vector2 limitVel = sC.rb.velocity.normalized * sC.currentSpeed;
            sC.rb.velocity = new Vector2(limitVel.x, limitVel.y);
        }
    }

    public override void PhysicsUpdate()
    {
        //sC.rb.velocity = new Vector2(sC.direction.x * sC.currentSpeed, sC.direction.y * sC.currentSpeed);

        Vector2 vel = new Vector2(sC.direction.x * sC.currentSpeed, sC.direction.y * sC.currentSpeed);

        sC.rb.AddForce(vel, ForceMode2D.Force);
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
