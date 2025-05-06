using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepController : MonoBehaviour
{
    //parametros modificables
    [SerializeField] public float sheepSpeed = 7.5f;
    [SerializeField] public float maxLife = 10;

    [SerializeField] public float wolfAttackRange;
    [SerializeField] public float dogActionRange;

    [SerializeField] public float restartIdleCooldown;
    [SerializeField] public float restartWalkCooldown;

    [SerializeField] public LayerMask wolfLayer;
    [SerializeField] public LayerMask dogLayer;

    //parametros internos
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator animator;

    [HideInInspector] public GameObject sheepCollection { get; private set; }
    [HideInInspector] public GameObject activeSheep { get; set; }

    [HideInInspector] public float currentSpeed;
    [HideInInspector] public float currentLife;

    [HideInInspector] public bool barrierWolf;


    //State Machine
    public StateMachine StateMachine { get; set; }

    //Movimiento
    public Sheep_IdleState IdleState { get; set; }
    public Sheep_WalkState WalkState { get; set; }


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();

        //Estados de la state machine
        StateMachine = new StateMachine();

        IdleState = new Sheep_IdleState(this, StateMachine);
        WalkState = new Sheep_WalkState(this, StateMachine);

        //inicia al el personaje con RestartState para reiniciar al lobo y darle un margen al jugador
        StateMachine.Initialize(IdleState);
    }


    void Update()
    {
        StateMachine.CurrentState.FrameUpdate();

        //Debug.Log("currentState: " + StateMachine.CurrentState.ToString() + "   barrierWolf " + barrierWolf);
    }


    void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        if (Physics2D.OverlapCircle(transform.position, dogActionRange, dogLayer))
            Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, dogActionRange);

        Gizmos.color = Color.cyan;

        if (Physics2D.OverlapCircle(transform.position, wolfAttackRange, wolfLayer))
            Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, wolfAttackRange);

    }
}
