using UnityEngine;

public class Sheep_ChaseWolfState : State
{
    float timer = 0;
    float maxDuration = 0.5f;

    public Sheep_ChaseWolfState(SheepController sheepController, StateMachine StateMachine) : base(StateMachine)
    {
        this.sC = sheepController;
    }

    public override void EnterState()
    {
        base.EnterState();

        sC.currentSpeed = sC.sheepChaseDogSpeed * 1.15f;
    }

    public override void FrameUpdate()
    {
        UpdateWolfState();

        if (timer >= maxDuration)
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
        base.ExitState();
        timer = 0;
    }

    public void UpdateWolfState()
    {
        if (!Physics2D.OverlapCircle(sC.transform.position, sC.wolfAttackRange, sC.wolfLayer))
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
            sC.direction = UpdateWolfDirection();
        }
    }

    public Vector2 UpdateWolfDirection()
    {
        Collider2D hitCollider = Physics2D.OverlapCircle(sC.transform.position, sC.wolfAttackRange, sC.wolfLayer);

        Vector2 wolfPos = (Vector2)hitCollider.transform.position;

        Vector2 newPos = (Vector2)sC.transform.position - wolfPos; //direccion contraria a la posicion del perro

        return newPos.normalized;

    }

    public override void AnimationEnter()
    {
        sC.animator.Play("WolfRun", 0, Random.Range(0f, 1f));
        sC.transform.Find("Particle_Grass").GetComponent<ParticleSystem>().Play();

    }

    public override void AnimationExit()
    {
        sC.transform.Find("Particle_Grass").GetComponent<ParticleSystem>().Stop();

    }

}
