using UnityEngine;


public class Wolf_ChaseState : State
{
    Vector2 direction;

    bool wolfUnderFire;

    public Wolf_ChaseState(WolfController wolfController, StateMachine StateMachine) : base(StateMachine)
    {
        this.wC = wolfController;
    }

    public override void EnterState()
    {
        wC.SheepSelect();

    }

    public override void FrameUpdate()
    {
        wolfUnderFire = Physics2D.OverlapCircle(wC.transform.position, wC.dogAttackingRange, wC.dogLayer);

        SheepChaseDirection();

        UnderDogFire();

        if (Physics2D.OverlapCircle(wC.transform.position, wC.sheepAttackRange * 0.25f, wC.sheepLayer) || wC.activeSheep.gameObject.layer == LayerMask.NameToLayer("SheepIsCaged"))
        {
            wC.StateMachine.ChangeState(wC.IdleState);
            return;
        }

        if (wC.currentLife <= 0)
        {
            wC.StateMachine.ChangeState(wC.AfraidState);
            return;
        }
    }

    public override void PhysicsUpdate()
    {
        wC.rb.velocity = new Vector2(direction.x * wC.currentSpeed, direction.y * wC.currentSpeed);

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

    private void SheepChaseDirection()
    {
        if (!wolfUnderFire)
        {
            direction = (wC.activeSheep.transform.position - wC.transform.position).normalized;
            wC.currentSpeed = wC.wolfSpeed;
        }
    }

    private void UnderDogFire()
    {
        if (wolfUnderFire)
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(wC.transform.position, wC.dogAttackingRange, wC.dogLayer);

            if (hitColliders.Length == 2)
            {
                float velReduction = 1 - (wC.currentLife * wC.maxLife / 7.5f);

                if (velReduction < 0.1)
                    velReduction = 0.1f;

                wC.currentLife -= Time.deltaTime * 3;
                wC.currentSpeed = wC.wolfSpeed * velReduction;

            }
            else if (hitColliders.Length == 1)
            {
                float velReduction = 1 - (wC.currentLife * wC.maxLife / 15);

                if (velReduction < 0.25)
                    velReduction = 0.25f;

                wC.currentLife -= Time.deltaTime;
                wC.currentSpeed = wC.wolfSpeed * velReduction;

            }

        }

    }

}
