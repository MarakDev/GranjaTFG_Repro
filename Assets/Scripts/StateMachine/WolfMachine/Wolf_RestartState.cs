using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class Wolf_RestartState : State
{
    float timer = 0;
    float maxDuration;
    public Wolf_RestartState(WolfController wolfController, StateMachine StateMachine) : base(StateMachine)
    {
        this.wC = wolfController;
    }

    public override void EnterState()
    {
        wC.rb.velocity = Vector2.zero;

        wC.barrierWolf = false;
        wC.currentLife = wC.maxLife;

        float maxCooldown = wC.restartChaseCooldown;

        maxDuration = (int)Random.Range(maxCooldown - 3, maxCooldown + 3);

    }

    public override void FrameUpdate()
    {
        timer += Time.deltaTime;

        if (timer >= maxDuration)
        {
            wC.StateMachine.ChangeState(wC.ChaseState);
        }
    }

    public override void ExitState()
    {
        timer = 0;
    }
}
