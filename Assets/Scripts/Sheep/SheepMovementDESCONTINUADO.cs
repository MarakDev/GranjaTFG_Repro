using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SheepMovementDESCONTINUADO : MonoBehaviour
{
    [SerializeField] private float wanderSpeed = 1;
    [SerializeField] private float pastorSpeed = 3;
    [SerializeField] private float afraidSpeed = 5;

    [SerializeField] private int maxStress = 100;

    [SerializeField] private Vector2 cooldownChangeDirection;
    [SerializeField] private Vector2 cooldownStay;

    [SerializeField] private float dogAreaRange;
    [SerializeField] private float wolfAreaRange;
    [SerializeField] private LayerMask dogLayer;
    [SerializeField] private LayerMask wolfLayer;


    //private
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Vector2 newDirection;

    private float speed;
    private float stress;

    private bool isCaged = false;
    private bool changeDirectionOnWalkFlag = true;
    private bool stressStun = false;
    private bool isWalking = true;
    private bool dogNear = false;
    private bool wolfNear = false;

    private float randomOffset;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        randomOffset = Random.Range(0.5f, 1f);

        newDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
    }

    void Update()
    {
        if (!isCaged)
        {
            if (!stressStun)
            {
                if (changeDirectionOnWalkFlag && !dogNear) //de normal se da paseo
                {
                    changeDirectionOnWalkFlag = false;
                    dogNear = false;
                    isWalking = true;
                    StartCoroutine(ChangeDirection());
                }

                DogBehaviour();

                WolfBehaviour();

                StressBehaivour();

                VelocityCheckers();

            }
            else
            {
                if (stress < 1)
                {
                    stressStun = false;
                    rb.mass = 1;
                }
            }

            StressCalculations();
            //Debug.Log("stress: " + stress);


        }
        else
        {
            if (changeDirectionOnWalkFlag)
            {
                changeDirectionOnWalkFlag = false;
                isWalking = true;
                StartCoroutine(ChangeDirection());
            }
        }

        UpdateAnimations();
    }

    private bool deceleration = false;
    private void VelocityCheckers()
    {
        //////////////////
        if (dogNear)
        {
            deceleration = true;
            if (rb.velocity.magnitude > pastorSpeed)
            {
                Vector2 limitVel = rb.velocity.normalized * pastorSpeed;
                rb.velocity = new Vector2(limitVel.x, limitVel.y);
            }
        }
        else if (wolfNear)
        {
            deceleration = true;

            if (rb.velocity.magnitude > afraidSpeed)
            {
                Vector2 limitVel = rb.velocity.normalized * afraidSpeed;
                rb.velocity = new Vector2(limitVel.x, limitVel.y);
            }
        }
        else if(isWalking)
        {
            if(rb.velocity.magnitude < wanderSpeed)
                deceleration = false;

            if (rb.velocity.magnitude > wanderSpeed && !deceleration)
            {
                Vector2 limitVel = rb.velocity.normalized * wanderSpeed;
                rb.velocity = new Vector2(limitVel.x, limitVel.y);
            }

        }
    }
    private void StressBehaivour()
    {
        if(stress > maxStress)
        {
            stressStun = true;
            wolfNear = false;
            speed = 0;
            rb.mass = 15;
        }
    }

    private void DogBehaviour()
    {
        if (Physics2D.OverlapCircle(transform.position, dogAreaRange, dogLayer) && !wolfNear)
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, dogAreaRange, dogLayer);

            float nDog = 1f; //empieza en 0.25 y aumenta un 0.25 por cada perro

            Vector2 dogPos = (Vector2)hitColliders[0].transform.position; //posicion del collider perro numero 1
            Vector2 vector_NewDir; //vector de nueva direccion

            if (hitColliders.Length > 1)
            {
                nDog = 1.5f;
                Vector2 dogPos2 = (Vector2)hitColliders[1].transform.position;

                Vector2 vector_dog1_sheep = (Vector2)transform.position - dogPos;
                Vector2 vector_dog2_sheep = (Vector2)transform.position - dogPos2;

                vector_NewDir = vector_dog2_sheep + vector_dog1_sheep; //direccion contraria a la posicion del perro
            }
            else
            {
                vector_NewDir = (Vector2)transform.position - dogPos; //direccion contraria a la posicion del perro
            }


            newDirection = vector_NewDir.normalized;

            speed = pastorSpeed * nDog;
            dogNear = true;
            StopCoroutine(ChangeDirection());
        }
        else
        {
            speed = wanderSpeed;
            dogNear = false;
        }
    }
    private void WolfBehaviour()
    {
        if (Physics2D.OverlapCircle(transform.position, wolfAreaRange, wolfLayer))
        {
            Collider2D hitCollider = Physics2D.OverlapCircle(transform.position, wolfAreaRange, wolfLayer);

            Vector2 wolfPos = (Vector2)hitCollider.transform.position;

            Vector2 newPos = (Vector2)transform.position - wolfPos; //direccion contraria a la posicion del perro

            newDirection = newPos.normalized;

            speed = afraidSpeed;
            wolfNear = true;
            StopCoroutine(ChangeDirection());
        }
        else if(!dogNear)
        {
            speed = wanderSpeed;
            wolfNear = false;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(newDirection.x * speed, newDirection.y * speed);
        //Vector2 vel = new Vector2(newDirection.x * speed, newDirection.y * speed);

        //if((isWalking || wolfNear || dogNear))
        //    rb.AddForce(vel, ForceMode2D.Force);

        //Debug.Log(stress);
    }

    private void StressCalculations()
    {
        float time = Time.deltaTime * 10;

        if (wolfNear)
            stress += time;
        else
        {
            if (stressStun)
                stress -= time;
            else
                stress -= time * 0.5f; //si no esta estuneado pierde el stres a la mitad de velocidad
        }

        if (stress < 0)
            stress = 0;

    }

    private IEnumerator ChangeDirection()
    {
        float rndCooldownChangeDirection = Random.Range(cooldownChangeDirection.x, cooldownChangeDirection.y);
        float rndCooldownStay = Random.Range(cooldownStay.x, cooldownStay.y);

        //Debug.Log("CChDir: " + rndCooldownChangeDirection + " Cstay " + rndCooldownStay);

        newDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));

        yield return new WaitForSeconds(rndCooldownChangeDirection);

        newDirection = Vector2.zero;
        isWalking = false;

        yield return new WaitForSeconds(rndCooldownStay);

        changeDirectionOnWalkFlag = true;

    }

    public void GetCaged()
    {
        isCaged = true;
        changeDirectionOnWalkFlag = true;
        stressStun = false;
        dogNear = false;
        wolfNear = false;

        speed = wanderSpeed;
    }

    private void UpdateAnimations()
    {

        if (rb.velocity.x > 0.2f)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;

        AnimatorClipInfo[] animatorinfo = this.animator.GetCurrentAnimatorClipInfo(0);

        if(stressStun && animatorinfo[0].clip.name != "Stress")
            animator.Play("Stress", 0, randomOffset);
        else if (rb.velocity.magnitude < 0.05f && animatorinfo[0].clip.name != "Idle" && !dogNear && !wolfNear && !stressStun)
            animator.Play("Idle", 0, randomOffset);
        else if (rb.velocity.magnitude > pastorSpeed + 0.5f && !stressStun)
            animator.Play("RunMore");
        else if (rb.velocity.magnitude > wanderSpeed + 0.5f && !stressStun)
            animator.Play("Run");
        else if (rb.velocity.magnitude > 0.05f && animatorinfo[0].clip.name != "Walk" && !dogNear && !wolfNear && !stressStun)
            animator.Play("Walk", 0, randomOffset);
    }



    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.green;

        //if (Physics2D.OverlapCircle(transform.position, dogAreaRange, dogLayer))
        //    Gizmos.color = Color.red;

        //Gizmos.DrawWireSphere(transform.position, dogAreaRange);

        //Gizmos.color = Color.cyan;

        //if(Physics2D.OverlapCircle(transform.position, wolfAreaRange, wolfLayer))
        //    Gizmos.color = Color.red;

        //Gizmos.DrawWireSphere(transform.position, wolfAreaRange);

    }
}
