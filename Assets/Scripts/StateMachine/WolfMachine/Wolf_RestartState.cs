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

        maxDuration = (int)Random.Range(wC.restartTime, wC.restartTime * 2);

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
