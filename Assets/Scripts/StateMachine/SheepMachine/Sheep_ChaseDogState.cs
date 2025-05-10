using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Sheep_ChaseDogState : State
{
    float timer = 0;
    float maxDuration = 0.15f;

    public Sheep_ChaseDogState(SheepController sheepController, StateMachine StateMachine) : base(StateMachine)
    {
        this.sC = sheepController;
    }

    public override void EnterState()
    {
        base.EnterState();


    }

    public override void FrameUpdate()
    {
        UpdateDogState();

        if (timer >= maxDuration)
        {
            sC.StateMachine.ChangeState(sC.WalkState);
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

    public void UpdateDogState()
    {
        if (!Physics2D.OverlapCircle(sC.transform.position, sC.dogActionRange, sC.dogLayer))
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
            sC.direction = UpdateDogDirection();
        }
    }

    public Vector2 UpdateDogDirection()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(sC.transform.position, sC.dogActionRange, sC.dogLayer);

        float nDog = 1f; //empieza en 0.25 y aumenta un 0.25 por cada perro

        Vector2 dogPos = Vector2.zero;

        if (hitColliders.Length > 0)
            dogPos = (Vector2)hitColliders[0].transform.position; //posicion del collider perro numero 1

        Vector2 vector_NewDir; //vector de nueva direccion

        if (hitColliders.Length > 1)
        {
            nDog = 1.5f;
            Vector2 dogPos2 = (Vector2)hitColliders[1].transform.position;

            Vector2 vector_dog1_sheep = (Vector2)sC.transform.position - dogPos;
            Vector2 vector_dog2_sheep = (Vector2)sC.transform.position - dogPos2;

            vector_NewDir = vector_dog2_sheep + vector_dog1_sheep; //direccion contraria a la posicion del perro
        }
        else
        {
            vector_NewDir = (Vector2)sC.transform.position - dogPos; //direccion contraria a la posicion del perro
        }

        sC.currentSpeed = sC.sheepChaseDogSpeed * nDog;

        return vector_NewDir.normalized;
    }

    public override void AnimationEnter()
    {
        sC.animator.Play("DogRun", 0, Random.Range(0f, 1f));
        sC.transform.Find("Particle_Grass").GetComponent<ParticleSystem>().Play();

    }

    public override void AnimationExit()
    {
        sC.transform.Find("Particle_Grass").GetComponent<ParticleSystem>().Stop();

    }
}
