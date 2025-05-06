using UnityEngine;


public class Sheep_WalkState : State
{
    float timer = 0;
    float maxDuration = 0;

    Vector2 direction;
    public Sheep_WalkState(SheepController sheepController, StateMachine StateMachine) : base(StateMachine)
    {
        this.sC = sheepController;
    }

    public override void EnterState()
    {
        sC.rb.velocity = Vector2.zero;

        float dirX = Random.Range(-1f, 1f);
        float dirY = 1 - dirX;

        if (dirX < 0f)
            dirY = 1 + dirX;

        direction = new Vector2(dirX, dirY);

        maxDuration = Random.Range(sC.restartIdleCooldown - 5, sC.restartIdleCooldown + 5);

        if (maxDuration < 2)
            maxDuration = 5;
    }

    public override void FrameUpdate()
    {
        timer += Time.deltaTime;

        if (timer >= maxDuration)
        {
            sC.StateMachine.ChangeState(sC.IdleState);
            return;
        }
    }

    public override void PhysicsUpdate()
    {
        sC.rb.velocity = new Vector2(direction.x * sC.sheepSpeed, direction.y * sC.sheepSpeed);

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
