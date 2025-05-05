////
//Author : Marcos Monge
//Created On : 01/03/2024
//Last Modified On : 04/03/2024
//Version : 0.2.1
//Description : Estado padre del que heredaran el resto de estados para el funcionamiento del personaje
////

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class Wolf_RestartState : State
{
    float timer = 0;
    float maxDuration;
    public Wolf_RestartState(WolfController wolfController, StateMachine StateMachine) : base(StateMachine)
    {
        this.wolfController = wolfController;
    }

    public override void EnterState()
    {
        float maxCooldown = wolfController.restartChaseCooldown;
        maxDuration = (int)Random.Range(maxCooldown - 3, maxCooldown + 3);

    }

    public override void FrameUpdate()
    {
        timer += Time.deltaTime;

        if (timer >= maxDuration)
        {
            wolfController.StateMachine.ChangeState(wolfController.ChaseState);
        }
    }

}
