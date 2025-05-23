using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class SheepController : MonoBehaviour
{
    //parametros modificables
    [SerializeField] public float sheepSpeed = 1;
    [SerializeField] public float sheepChaseDogSpeed = 1;
    [SerializeField] public float sheepChaseWolfSpeed = 1;
    [SerializeField] public float maxLife = 10;

    [SerializeField] private int maxStomachCapacity = 20;
    [SerializeField] private int maxStress = 100;

    [SerializeField] public float sheepActionRange = 4;
    [SerializeField] public float dogActionRange = 6;
    [SerializeField] public float wolfAttackRange = 8;
    [SerializeField] public float grasssActionRange = 20;

    [SerializeField] public float idleTime = 4;
    [SerializeField] public float walkTime = 6;
    [SerializeField] public float eatTime = 10;

    [SerializeField] public LayerMask sheepLayer;
    [SerializeField] public LayerMask dogLayer;
    [SerializeField] public LayerMask wolfLayer;
    [SerializeField] public LayerMask grassLayer;
    [SerializeField] public LayerMask farmBarrierLayer;

    //parametros internos
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator animator;

    [Header("Debug")]
    public float currentSpeed = 0;
    public float currentStomachCapacity = 0;
    public bool stomachFull = false;
    public float currentStress = 0;
    public Vector2 direction;

    //State Machine
    public StateMachine StateMachine { get; set; }

    //Movimiento
    public Sheep_IdleState IdleState { get; set; }
    public Sheep_WalkState WalkState { get; set; }
    public Sheep_EatState EatState { get; set; }
    public Sheep_ChaseDogState ChaseDogState { get; set; }
    public Sheep_ChaseWolfState ChaseWolfState { get; set; }
    public Sheep_StressState StressState { get; set; }
    public Sheep_FollowSheepState FollowSheepState { get; set; }


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentStomachCapacity = Random.Range(0, maxStomachCapacity);

        transform.Find("Particle_Z").GetComponent<ParticleSystem>().Stop();
        transform.Find("Particle_Grass").GetComponent<ParticleSystem>().Stop();

        //Estados de la state machine
        StateMachine = new StateMachine();

        IdleState = new Sheep_IdleState(this, StateMachine);
        WalkState = new Sheep_WalkState(this, StateMachine);
        EatState = new Sheep_EatState(this, StateMachine);
        ChaseDogState = new Sheep_ChaseDogState(this, StateMachine);
        ChaseWolfState = new Sheep_ChaseWolfState(this, StateMachine);
        StressState = new Sheep_StressState(this, StateMachine);
        FollowSheepState = new Sheep_FollowSheepState(this, StateMachine);

        //inicia al el personaje con RestartState para reiniciar al lobo y darle un margen al jugador
        StateMachine.Initialize(IdleState);
    }


    void Update()
    {
        StateMachine.CurrentState.FrameUpdate();

        StomachCalculations();

        //Debug.Log("currentState: " + StateMachine.CurrentState.ToString() + " vel: " + rb.velocity.magnitude);

        WallCheckers();
    }


    void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }



    public Vector2 RandomPosition()
    {
        float dirX = Random.Range(-1f, 1f);
        float dirY = Random.Range(-1f, 1f);

        return new Vector2(dirX, dirY);
    }

    public void StomachCalculations()
    {
        if(currentStomachCapacity >= maxStomachCapacity && !stomachFull)
        {
            currentStomachCapacity = maxStomachCapacity;
            stomachFull = true;
        }
        
        if(StateMachine.CurrentState.ToString() != "Sheep_EatState")
        {
            currentStomachCapacity -= Time.deltaTime * 0.25f;

            if (currentStomachCapacity <= 0)
            {
                stomachFull = false;
                currentStomachCapacity = 0;
            }
        }
    }

    private void StressCalculations()
    {
        //float time = Time.deltaTime * 10;

        //if (wolfNear)
        //    stress += time;
        //else
        //{
        //    if (stressStun)
        //        stress -= time;
        //    else
        //        stress -= time * 0.5f; //si no esta estuneado pierde el stres a la mitad de velocidad
        //}

        //if (stress < 0)
        //    stress = 0;

    }

    public void WallCheckers()
    {
        Debug.DrawRay(transform.position, Vector2.up * 2f, Color.red);
        Debug.DrawRay(transform.position, Vector2.down * 2f, Color.red);
        Debug.DrawRay(transform.position, Vector2.right * 2f, Color.red);
        Debug.DrawRay(transform.position, Vector2.left * 2f, Color.red);

        bool upBarrier = Physics2D.Raycast(transform.position, Vector2.up, 2f, farmBarrierLayer);
        bool downBarrier = Physics2D.Raycast(transform.position, Vector2.down, 2f, farmBarrierLayer);
        bool rightBarrier = Physics2D.Raycast(transform.position, Vector2.right, 2f, farmBarrierLayer);
        bool leftBarrier = Physics2D.Raycast(transform.position, Vector2.left, 2f, farmBarrierLayer);


        //esquinas
        if (upBarrier && leftBarrier) //arriba izquierda
        {
            direction = new Vector2(1, -1);

            return;
        }
        if (upBarrier && rightBarrier) //arriba izquierda
        {
            direction = new Vector2(-1, -1);

            return;
        }
        if (downBarrier && leftBarrier) //arriba izquierda
        {
            direction = new Vector2(1, 1);

            return;
        }
        if (downBarrier && rightBarrier) //arriba izquierda
        {
            direction = new Vector2(-1, 1);

            return;
        }


        //casos de una sola pared
        if (upBarrier)
        {
            if (direction.x > 0) //mayor que 0
                direction = new Vector2 (1, -Mathf.Abs(direction.y));
            else  //menor que 0
                direction = new Vector2(-1, -Mathf.Abs(direction.y));

            return;
        }
        if (downBarrier)
        {
            if (direction.x > 0) //mayor que 0
                direction = new Vector2(1, Mathf.Abs(direction.y));
            else  //menor que 0
                direction = new Vector2(-1, Mathf.Abs(direction.y));

            return;
        }
        if (rightBarrier)
        {
            if (direction.y > 0) //mayor que 0
                direction = new Vector2(-Mathf.Abs(direction.x), 1);
            else  //menor que 0
                direction = new Vector2(-Mathf.Abs(direction.x), -1);
            
            return;
        }
        if (leftBarrier)
        {
            if (direction.y > 0) //mayor que 0
                direction = new Vector2(Mathf.Abs(direction.x), 1);
            else  //menor que 0
                direction = new Vector2(Mathf.Abs(direction.x), -1);

            return;
        }



    }

    public void UpdateSpriteDirection()
    {

        if (direction.x < 0 && rb.velocity.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            transform.Find("Particle_Z").transform.localPosition = new Vector3(-0.18f, transform.Find("Particle_Z").transform.localPosition.y, transform.Find("Particle_Z").transform.localPosition.z);
            transform.Find("Particle_Grass").transform.localPosition = new Vector3(0.25f, transform.Find("Particle_Grass").transform.localPosition.y, transform.Find("Particle_Grass").transform.localPosition.z);
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = false;
            transform.Find("Particle_Z").transform.localPosition = new Vector3(0.18f, transform.Find("Particle_Z").transform.localPosition.y, transform.Find("Particle_Z").transform.localPosition.z);
            transform.Find("Particle_Grass").transform.localPosition = new Vector3(-0.25f, transform.Find("Particle_Grass").transform.localPosition.y, transform.Find("Particle_Grass").transform.localPosition.z);

        }
    }

    public bool SheepFollowLogic()
    {
        Vector2 dirBetweenTwoSheeps = Vector2.zero;
        GameObject currentSheepFollow = null;
        float distClosestSheep = 0;

        Collider2D[] sheepFollow = Physics2D.OverlapCircleAll(transform.position, sheepActionRange, sheepLayer);

        if (sheepFollow.Length > 1)
        {
            //con esto saco la referencia de la oveja que este mas proxima a la oveja principal
            for (int i = 0; i < sheepFollow.Length; i++)
            {
                if (sheepFollow[i].name != this.name && sheepFollow[i].GetComponent<SheepController>().StateMachine.CurrentState.ToString() == "Sheep_ChaseDogState")
                {

                    float distBetweenSheeps = Vector2.Distance(transform.position, sheepFollow[i].transform.position);

                    //Debug.Log("sheepFollower: " + sheepFollow[i].gameObject + "   distBetween: " + distBetweenSheeps);

                    if (distBetweenSheeps < distClosestSheep)
                    {
                        distClosestSheep = distBetweenSheeps;
                        currentSheepFollow = sheepFollow[i].gameObject;
                        dirBetweenTwoSheeps = (transform.position - currentSheepFollow.transform.position).normalized;
                    }

                    if (distClosestSheep == 0)
                    {
                        distClosestSheep = distBetweenSheeps;
                        currentSheepFollow = sheepFollow[i].gameObject;
                        dirBetweenTwoSheeps = (transform.position - currentSheepFollow.transform.position).normalized;
                    }
                }
            }

            if (!currentSheepFollow.IsUnityNull() /*&& currentSheepFollow.GetComponent<SheepController>().StateMachine.CurrentState.ToString() == "Sheep_ChaseDogState"*/)
            {
                //Debug.Log("ovejaseleccionada: " + currentSheepFollow.name + "  - direction: " + direction);

                Vector2 dirOtherSheep = currentSheepFollow.GetComponent<SheepController>().direction;

                //Vector2 newDir = (dirBetweenTwoSheeps - dirOtherSheep).normalized;
                direction = ((dirBetweenTwoSheeps / 5) + dirOtherSheep).normalized;
                //direction = dirOtherSheep.normalized;

                currentSpeed = sheepChaseDogSpeed;

                return true;
            }
        }

        return false;
    }

    public bool DogIsNear()
    {
        return Physics2D.OverlapCircle(transform.position, dogActionRange, dogLayer);
    }

    public bool WolfIsNear()
    {
        return Physics2D.OverlapCircle(transform.position, wolfAttackRange, wolfLayer);
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.magenta;

        //if (Physics2D.OverlapCircle(transform.position, dogActionRange, dogLayer))
        //    Gizmos.color = Color.red;

        //Gizmos.DrawWireSphere(transform.position, dogActionRange);

        //Gizmos.color = Color.cyan;

        //if (Physics2D.OverlapCircle(transform.position, wolfAttackRange, wolfLayer))
        //    Gizmos.color = Color.red;

        //Gizmos.DrawWireSphere(transform.position, wolfAttackRange);

        //Gizmos.color = Color.green;

        //if (Physics2D.OverlapCircle(transform.position, grasssActionRange, grassLayer))
        //    Gizmos.color = Color.red;

        //Gizmos.DrawWireSphere(transform.position, grasssActionRange);

        //Gizmos.color = Color.green;

        //if (Physics2D.OverlapCircle(transform.position, sheepActionRange, sheepLayer))
        //    Gizmos.color = Color.red;

        //Gizmos.DrawWireSphere(transform.position, sheepActionRange);
    }
}
