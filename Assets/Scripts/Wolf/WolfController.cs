using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class WolfController : MonoBehaviour
{
    //parametros modificables
    [Tooltip("Velocidad base del lobo")]
    [SerializeField] public float wolfSpeed = 7.5f;
    [Tooltip("Vida maxima del lobo")]
    [SerializeField] public float maxLife = 5;

    [Tooltip("Rango en el que puede ser atacado por el perro")]
    [SerializeField] public float dogActionRange;

    [Tooltip("Tiempo en el que esta en idle state")]
    [SerializeField] public float idleTime = 2;
    [Tooltip("Tiempo en el que esta en restart state")]
    [SerializeField] public float restartTime = 6;

    [Header("Layers")]
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
    public Wolf_DogAttackState DogAttackState { get; set; }

    
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
        DogAttackState = new Wolf_DogAttackState(this, StateMachine);

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

    public void UnderDogAttack(bool isWolfUnderFire)
    {
        if (isWolfUnderFire)
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, dogActionRange, dogLayer);

            if (hitColliders.Length == 2)
            {
                float velReduction = 1 - (currentLife * maxLife / 7.5f);

                if (velReduction < 0.1)
                    velReduction = 0.1f;

                currentLife -= Time.deltaTime * 3;
                currentSpeed = wolfSpeed * velReduction;

            }
            else if (hitColliders.Length == 1)
            {
                float velReduction = 1 - (currentLife * maxLife / 15);

                if (velReduction < 0.25)
                    velReduction = 0.25f;

                currentLife -= Time.deltaTime;
                currentSpeed = wolfSpeed * velReduction;

            }

            
        }

    }

    public Vector2 RandomPosition()
    {
        float dirX = Random.Range(-1f, 1f);
        float dirY = 1 - dirX;

        if (dirX < 0f)
            dirY = 1 + dirX;

        return new Vector2(dirX, dirY);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("WolfDeactivate") && StateMachine.CurrentState.ToString() == "Wolf_AfraidState")
        {
            barrierWolf = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        if(Physics2D.OverlapCircle(transform.position, dogActionRange, dogLayer))
            Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, dogActionRange);

    }
}
