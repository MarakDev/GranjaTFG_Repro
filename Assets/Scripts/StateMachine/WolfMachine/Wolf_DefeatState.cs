
using UnityEngine;

public class Wolf_DefeatState : State
{
    float timer = 0;
    float maxDuration = 5f;

    public Wolf_DefeatState(WolfController wolfController, StateMachine StateMachine) : base(StateMachine)
    {
        this.wC = wolfController;
    }

    public override void EnterState()
    {
        base.EnterState();

        wC.rb.velocity = Vector2.zero;

        //animacion de la sombra instanciada
        wC.GenerateShadow();

        wC.currentSpeed = 0.5f * wC.wolfSpeed;
    }

    public override void FrameUpdate()
    {
        timer += Time.deltaTime;

        if (timer > maxDuration)
        {
            wC.StateMachine.ChangeState(wC.RestartState);
            return;
        }
    }

    public override void PhysicsUpdate()
    {
        wC.rb.velocity = new Vector2(0 * wC.currentSpeed,  wC.currentSpeed);

    }

    public override void ExitState()
    {
        timer = 0;

        wC.EraseShadow();
    }

    public override void AnimationEnter()
    {
        wC.animator.Play("Baloon");

    }

    public override void AnimationExit()
    {

    }

}
