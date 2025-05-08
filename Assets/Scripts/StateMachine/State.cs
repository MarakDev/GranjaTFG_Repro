
public class State
{

    protected StateMachine stateMachine;
    protected WolfController wC;
    protected SheepController sC;

    public State(StateMachine StateMachine)
    {
        this.stateMachine = StateMachine;
    }

    public virtual void EnterState()
    {
        AnimationEnter();

    }
    public virtual void FrameUpdate()
    {
 
    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void ExitState()
    {
        AnimationExit();
    }

    public virtual void AnimationEnter()
    {

    }

    public virtual void AnimationExit()
    {

    }


}
