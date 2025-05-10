using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //var publics

    [SerializeField] private float speed;

    [SerializeField] private float wolfRange;
    [SerializeField] private LayerMask wolfLayer;

    //var privates
    private GameObject dog1;
    private GameObject dog2;

    private Rigidbody2D dog1_rb;
    private Rigidbody2D dog2_rb;

    private Vector2 dog1_moveDirection;
    private Vector2 dog2_moveDirection;

    private float dog1_lastInput;
    private float dog2_lastInput;

    private float dog1_idleTimer;
    private float dog2_idleTimer;

    //inputs
    private PlayerControls actionMap;
    private InputAction dog1_input;
    private InputAction dog2_input;

    private void Awake()
    {
        actionMap = new PlayerControls();

    }

    void Start()
    {
        dog1 = transform.Find("Dog1").gameObject;
        dog2 = transform.Find("Dog2").gameObject;

        dog1_rb = dog1.GetComponent<Rigidbody2D>();
        dog2_rb = dog2.GetComponent<Rigidbody2D>();

        dog1_lastInput = 1;
        dog2_lastInput = -1;

        UpdateAnimation(dog1, dog1_lastInput,ref dog1_idleTimer);
        UpdateAnimation(dog2, dog2_lastInput, ref dog2_idleTimer);
    }

    private void Update()
    {
        //inputs
        dog1_moveDirection = dog1_input.ReadValue<Vector2>();
        if (dog1_moveDirection.x != 0)
            dog1_lastInput = dog1_moveDirection.x;

        UpdateAnimation(dog1, dog1_lastInput, ref dog1_idleTimer);


        dog2_moveDirection = dog2_input.ReadValue<Vector2>();
        if (dog2_moveDirection.x != 0)
            dog2_lastInput = dog2_moveDirection.x;

        UpdateAnimation(dog2, dog2_lastInput, ref dog2_idleTimer);
    }

    private void FixedUpdate()
    {
        dog1_rb.velocity = new Vector2(dog1_moveDirection.x * speed, dog1_moveDirection.y * speed);
        dog2_rb.velocity = new Vector2(dog2_moveDirection.x * speed, dog2_moveDirection.y * speed);
    }

    private void UpdateAnimation(GameObject dog, float inputDir, ref float idleTimer)
    {
        if (inputDir <= 0f)
            dog.GetComponent<SpriteRenderer>().flipX = true;
        else
            dog.GetComponent<SpriteRenderer>().flipX = false;

        if (dog.GetComponent<Rigidbody2D>().velocity.magnitude < 0.2f)
        {
            idleTimer += Time.deltaTime;

            if (idleTimer >= 5f)
            {
                if (idleTimer >= 7f)
                    idleTimer = 0;

                dog.GetComponent<Animator>().Play("IdleRest");
            }
            else
                dog.GetComponent<Animator>().Play("Idle");
        }
        else if(dog.GetComponent<Rigidbody2D>().velocity.magnitude > 0.2f)
        {
            idleTimer = 0;
            dog.GetComponent<Animator>().Play("Run");
        }

    }

    private void OnEnable()
    {
        dog1_input = actionMap.Controls.Move_Dog1;
        dog2_input = actionMap.Controls.Move_Dog2;

        dog1_input.Enable();
        dog2_input.Enable();
    }
    private void OnDisable()
    {
        dog1_input.Disable();
        dog2_input.Disable();
    }
}
