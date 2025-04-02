using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SheepMovement : MonoBehaviour
{
    [SerializeField] private float wanderSpeed = 1;
    [SerializeField] private float dogSpeed = 3;

    [SerializeField] private Vector2 cooldownChangeDirection;
    [SerializeField] private Vector2 cooldownStay;

    [SerializeField] private float dogAreaRange;
    [SerializeField] private LayerMask dogLayer;

    //private
    private Rigidbody2D rb;
    private Vector2 newDirection;

    private float speed;

    private bool changeDirectionOnWalkFlag = true;

    private bool isWalking = true;
    private bool dogNear = false;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        newDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
    }

    void Update()
    {

        if (changeDirectionOnWalkFlag && !dogNear) //de normal se da paseo
        {
            changeDirectionOnWalkFlag = false;
            dogNear = false;
            isWalking = true;
            StartCoroutine(ChangeDirection());
        }

        DogBehaviour();

        VelocityCheckers();

        
        if (changeDirectionOnWalkFlag)
        {
            changeDirectionOnWalkFlag = false;
            isWalking = true;
            StartCoroutine(ChangeDirection());
        }
        

    }

    private bool deceleration = false;
    private void VelocityCheckers()
    {
        //////////////////
        if (dogNear)
        {
            deceleration = true;
            if (rb.velocity.magnitude > dogSpeed)
            {
                Vector2 limitVel = rb.velocity.normalized * dogSpeed;
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

    private void DogBehaviour()
    {
        if (Physics2D.OverlapCircle(transform.position, dogAreaRange, dogLayer))
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

            speed = dogSpeed * nDog;
            dogNear = true;
            StopCoroutine(ChangeDirection());
        }
        else
        {
            speed = wanderSpeed;
            dogNear = false;
        }
    }

    private void FixedUpdate()
    {
        //rb.velocity = new Vector2(newDirection.x * speed, newDirection.y * speed);
        Vector2 vel = new Vector2(newDirection.x * speed, newDirection.y * speed);

        if((isWalking || dogNear))
            rb.AddForce(vel, ForceMode2D.Force);

        //Debug.Log(stress);
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




    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        if (Physics2D.OverlapCircle(transform.position, dogAreaRange, dogLayer))
            Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, dogAreaRange);


    }
}
