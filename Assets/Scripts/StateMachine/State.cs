

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class State
{

    protected StateMachine stateMachine;
    protected WolfController wC;

    public State(StateMachine StateMachine)
    {
        this.stateMachine = StateMachine;
    }

    public virtual void EnterState()
    {


    }
    public virtual void FrameUpdate()
    {
 
    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void ExitState()
    {

    }

    public virtual void AnimationEnter()
    {

    }

    public virtual void AnimationExit()
    {

    }

}
