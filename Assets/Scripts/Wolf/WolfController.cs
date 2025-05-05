using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class WolfController : MonoBehaviour
{
    //parametros modificables
    [SerializeField] private float wolfSpeed = 7.5f;
    [SerializeField] private float maxLife = 100;

    [SerializeField] private float sheepAttackRange;

    [SerializeField] public float restartChaseCooldown;

    //parametros internos
    private Rigidbody2D rb;
    private Animator animator;

    public GameObject sheepCollection {get; private set;}
    public GameObject activeSheep { get; set; }

    private float currentSpeed;
    private float currentLife;


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
    }


    void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    //funciones del lobo

    public IEnumerator WolfActive(int cooldown)
    {
        yield return new WaitForSeconds(cooldown);

        if (sheepCollection == null)
            sheepCollection = GameObject.FindGameObjectWithTag("SheepFree");

        SheepSelect();

        StateMachine.ChangeState(ChaseState);

        //smokeDeath.SetActive(false);
        //smokeFire.SetActive(false);
    }

    private void SheepSelect()
    {
        int totalSheepsActive = sheepCollection.transform.childCount;

        int randomSheep = Random.Range(0, totalSheepsActive);

        if (sheepCollection.transform.childCount > 0)
            activeSheep = sheepCollection.transform.GetChild(randomSheep).gameObject;

    }
}
