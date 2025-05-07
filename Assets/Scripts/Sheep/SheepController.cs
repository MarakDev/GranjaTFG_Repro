using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    [SerializeField] public float dogActionRange = 6;
    [SerializeField] public float wolfAttackRange = 8;
    [SerializeField] public float grasssActionRange = 20;

    [SerializeField] public float idleTime = 4;
    [SerializeField] public float walkTime = 6;
    [SerializeField] public float eatTime = 10;

    [SerializeField] public LayerMask dogLayer;
    [SerializeField] public LayerMask wolfLayer;
    [SerializeField] public LayerMask grassLayer;

    //parametros internos
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator animator;

    [Header("Debug")]
    public float currentSpeed = 0;
    public float currentStomachCapacity = 0;
    public bool stomachFull = false;
    public float currentStress = 0;



    //State Machine
    public StateMachine StateMachine { get; set; }

    //Movimiento
    public Sheep_IdleState IdleState { get; set; }
    public Sheep_WalkState WalkState { get; set; }
    public Sheep_EatState EatState { get; set; }
    public Sheep_ChaseDogState ChaseDogState { get; set; }
    public Sheep_ChaseWolfState ChaseWolfState { get; set; }
    public Sheep_StressState StressState { get; set; }


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();


        currentStomachCapacity = Random.Range(0, maxStomachCapacity);


        //Estados de la state machine
        StateMachine = new StateMachine();

        IdleState = new Sheep_IdleState(this, StateMachine);
        WalkState = new Sheep_WalkState(this, StateMachine);
        EatState = new Sheep_EatState(this, StateMachine);
        ChaseDogState = new Sheep_ChaseDogState(this, StateMachine);
        ChaseWolfState = new Sheep_ChaseWolfState(this, StateMachine);
        StressState = new Sheep_StressState(this, StateMachine);

        //inicia al el personaje con RestartState para reiniciar al lobo y darle un margen al jugador
        StateMachine.Initialize(IdleState);
    }


    void Update()
    {
        StateMachine.CurrentState.FrameUpdate();

        StomachCalculations();

        Debug.Log("currentState: " + StateMachine.CurrentState.ToString());
    }


    void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }




    public Vector2 RandomPosition()
    {
        float dirX = Random.Range(-1f, 1f);
        float dirY = 1 - dirX;

        if (dirX < 0f)
            dirY = 1 + dirX;

        return new Vector2(dirX, dirY);
    }

    public void StomachCalculations()
    {
        if(currentStomachCapacity >= maxStomachCapacity && !stomachFull)
        {
            currentStomachCapacity = maxStomachCapacity;
            stomachFull = true;
        }

        if (stomachFull)
        {
            currentStomachCapacity -= Time.deltaTime;

            if (currentStomachCapacity <= 0)
            {
                stomachFull = false;
                currentStomachCapacity = 0;
            }
        }
        else if(!stomachFull && StateMachine.CurrentState.ToString() != "Sheep_EatState")
        {
            currentStomachCapacity -= Time.deltaTime * 0.1f;

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
        Gizmos.color = Color.magenta;

        if (Physics2D.OverlapCircle(transform.position, dogActionRange, dogLayer))
            Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, dogActionRange);

        Gizmos.color = Color.cyan;

        if (Physics2D.OverlapCircle(transform.position, wolfAttackRange, wolfLayer))
            Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, wolfAttackRange);

        Gizmos.color = Color.green;

        if (Physics2D.OverlapCircle(transform.position, grasssActionRange, grassLayer))
            Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, grasssActionRange);


    }
}
