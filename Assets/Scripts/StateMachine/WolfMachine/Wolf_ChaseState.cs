////
//Author : Marcos Monge
//Created On : 01/03/2024
//Last Modified On : 04/03/2024
//Version : 0.2.1
//Description : Estado padre del que heredaran el resto de estados para el funcionamiento del personaje
////

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class Wolf_ChaseState : State
{
    public Wolf_ChaseState(WolfController wolfController, StateMachine StateMachine) : base(StateMachine)
    {
        this.wolfController = wolfController;
    }

    public override void EnterState()
    {


    }
    public override void FrameUpdate()
    {
 
    }

    public override void PhysicsUpdate()
    {

    }

    public override void ExitState()
    {

    }

    public override void AnimationEnter()
    {

    }

    public override void AnimationExit()
    {

    }

}
