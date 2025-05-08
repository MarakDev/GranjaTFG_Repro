using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;


public class Sheep_WalkState : State
{
    float timer = 0;
    float grasstimer = 0;
    float maxDuration = 0;

    Vector2 currentGrassPosition;
    bool searchingGrass = false;

    public Sheep_WalkState(SheepController sheepController, StateMachine StateMachine) : base(StateMachine)
    {
        this.sC = sheepController;
    }

    public override void EnterState()
    {
        base.EnterState();

        sC.rb.velocity = Vector2.zero;

        //eleccion de posicion aleatoria
        if (sC.currentStomachCapacity < 6)
            sC.direction = GrassDirection();
        else
            sC.direction = sC.RandomPosition();

        //maxima duracion en estado walk
        maxDuration = Random.Range(sC.walkTime - 3, sC.walkTime + 3);

        if (maxDuration < 2)
            maxDuration = 5;

        sC.currentSpeed = sC.sheepSpeed;
    }


    public override void FrameUpdate()
    {
        timer += Time.deltaTime;

        if (timer >= maxDuration && !searchingGrass)
        {
            sC.StateMachine.ChangeState(sC.IdleState);
            return;
        }

        if (searchingGrass)
        {
            grasstimer += Time.deltaTime;
            sC.direction = CurrentGrassDirection();

            //cuando la oveja esta en un rango de menos de 1 distancia
            if(Physics2D.OverlapCircle(sC.transform.position, 1f, sC.grassLayer))
            {
                sC.StateMachine.ChangeState(sC.EatState);
                return;
            }

            if (grasstimer >= 8)
            {
                sC.StateMachine.ChangeState(sC.IdleState);
                return;
            }
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

        sC.UpdateSpriteDirection();

    }

    public override void PhysicsUpdate()
    {
        sC.rb.velocity = new Vector2(sC.direction.x * sC.currentSpeed, sC.direction.y * sC.currentSpeed);

    }

    public override void ExitState()
    {
        base.ExitState();

        timer = 0;
        grasstimer = 0;
        searchingGrass = false;
    }

    private Vector2 GrassDirection()
    {
        Collider2D hitGrass = Physics2D.OverlapCircle(sC.transform.position, sC.grasssActionRange, sC.grassLayer);

        if (hitGrass != null && !sC.stomachFull)
        {
            searchingGrass = true;

            currentGrassPosition = hitGrass.transform.position;

            return (hitGrass.transform.position - sC.transform.position).normalized;
        }

        return sC.RandomPosition();
    }

    private Vector2 CurrentGrassDirection()
    {
        return (currentGrassPosition - (Vector2)sC.transform.position).normalized;
    }

    public override void AnimationEnter()
    {
        sC.animator.Play("Walk", 0, Random.Range(0f, 1f));

    }

    public override void AnimationExit()
    {

    }

}
