using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class WolfController : MonoBehaviour
{
    //parametros modificables
    [SerializeField] public float wolfSpeed = 7.5f;
    [SerializeField] public float maxLife = 5;

    [SerializeField] public float sheepAttackRange;
    [SerializeField] public float dogAttackingRange;

    [SerializeField] public float restartChaseCooldown;

    [SerializeField] public LayerMask sheepLayer;
    [SerializeField] public LayerMask dogLayer;

    //parametros internos
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator animator;

    [HideInInspector] public GameObject sheepCollection {get; private set;}
    [HideInInspector] public GameObject activeSheep { get; set; }

    [HideInInspector] public float currentSpeed;
    [HideInInspector] public float currentLife;

    [HideInInspector] public bool barrierWolf;


    //State Machine
    public StateMachine StateMachine { get; set; }

    //Movimiento
    public Wolf_IdleState IdleState { get; set; }
    public Wolf_ChaseState ChaseState { get; set; }
    public Wolf_AfraidState AfraidState { get; set; }
    public Wolf_RestartState RestartState { get; set; }


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();

        if (sheepCollection == null)
            sheepCollection = GameObject.FindGameObjectWithTag("SheepFree");


        barrierWolf = false;

        //Estados de la state machine
        StateMachine = new StateMachine();

        IdleState = new Wolf_IdleState(this, StateMachine);
        ChaseState = new Wolf_ChaseState(this, StateMachine);
        AfraidState = new Wolf_AfraidState(this, StateMachine);
        RestartState = new Wolf_RestartState(this, StateMachine);

        //inicia al el personaje con RestartState para reiniciar al lobo y darle un margen al jugador
        StateMachine.Initialize(RestartState);
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

    //funciones del lobo
    public void SheepSelect()
    {
        int totalSheepsActive = sheepCollection.transform.childCount;

        int randomSheep = Random.Range(0, totalSheepsActive);

        if (sheepCollection.transform.childCount > 0)
            activeSheep = sheepCollection.transform.GetChild(randomSheep).gameObject;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("WolfDeactivate") && StateMachine.CurrentState.ToString() == "Wolf_AfraidState")
        {
            barrierWolf = true;
        }
    }
}
