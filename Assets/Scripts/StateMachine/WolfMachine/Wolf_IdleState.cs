
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class Wolf_IdleState : State
{
    float timer = 0;
    float maxDuration = 2;
    Vector2 direction;

    bool wolfUnderFire;

    public Wolf_IdleState(WolfController wolfController, StateMachine StateMachine) : base(StateMachine)
    {
        this.wC = wolfController;
    }

    public override void EnterState()
    {
        wC.rb.velocity = Vector2.zero;

        float dirX = Random.Range(-1f, 1f);
        float dirY = 1 - dirX;

        if (dirX < 0f)
            dirY = 1 + dirX;

        direction = new Vector2(dirX, dirY);
    }

    public override void FrameUpdate()
    {
        timer += Time.deltaTime;


        UnderDogFire();

        if (timer >= maxDuration)
        {
            wC.StateMachine.ChangeState(wC.ChaseState);
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
        wC.rb.velocity = new Vector2(direction.x * wC.wolfSpeed * 0.25f, direction.y * wC.wolfSpeed * 0.25f);

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

            direction = (wC.transform.position - hitColliders[0].transform.position).normalized;
        }

    }

}
