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

        //25% de que la oveja se duerma solo afecta a la animacion
        if (Random.Range(0, 100) < 75)
            maxDuration = Random.Range(sC.idleTime - 2, sC.idleTime + 2);
        else
            maxDuration = Random.Range((sC.idleTime * 3), (sC.idleTime * 3) + 2);

        if (maxDuration < 1)
            maxDuration = 3;

        sC.direction = Vector2.zero;

        AnimationEnter();
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
        base.ExitState();

        timer = 0;
    }


    public override void AnimationEnter()
    {
        if(maxDuration <= sC.idleTime * 3)
        {
            sC.animator.Play("Idle", 0, Random.Range(0f, 1f));
        }
        else
        {
            sC.animator.Play("SleepTransition");

            sC.transform.Find("Particle_Z").GetComponent<ParticleSystem>().Play();
        }
    }

    public override void AnimationExit()
    {
        sC.transform.Find("Particle_Z").GetComponent<ParticleSystem>().Stop();
    }

}
