using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //var publics

    [SerializeField] private float speed;

    [SerializeField] private float wolfRange;
    [SerializeField] private LayerMask wolfLayer;

    //animator
    private Animator animatorD1;
    private Animator animatorD2;

    

    //var privates
    private GameObject dog1;
    private GameObject dog2;

    private Rigidbody2D dog1_rb;
    private Rigidbody2D dog2_rb;

    private Vector2 dog1_moveDirection;
    private Vector2 dog2_moveDirection;

    private bool dog1Area = false;
    private bool dog2Area = false;

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

        animatorD1 = dog1.GetComponent<Animator>();
        animatorD2 = dog2.GetComponent<Animator>();

        animatorD1.Play("Idle");
        animatorD2.Play("Idle");
    }

    private void Update()
    {
        //inputs
        dog1_moveDirection = dog1_input.ReadValue<Vector2>();
        dog2_moveDirection = dog2_input.ReadValue<Vector2>();

        if (dog1 != null)
            dog1Area = Physics2D.OverlapCircle(dog1.transform.position, wolfRange, wolfLayer);
        if (dog2 != null)
            dog2Area = Physics2D.OverlapCircle(dog2.transform.position, wolfRange, wolfLayer);

        UpdateAnimationD1();
        UpdateAnimationD2();
    }

    private void FixedUpdate()
    {
        dog1_rb.velocity = new Vector2(dog1_moveDirection.x * speed, dog1_moveDirection.y * speed);
        dog2_rb.velocity = new Vector2(dog2_moveDirection.x * speed, dog2_moveDirection.y * speed);
    }

    private void UpdateAnimationD1()
    {
        if (dog1_rb.velocity.x > 0.2f)
            dog1.transform.localScale = new Vector3(2.25f, dog1.transform.localScale.y, dog1.transform.localScale.z);
        else
            dog1.transform.localScale = new Vector3(-2.25f, dog1.transform.localScale.y, dog1.transform.localScale.z);

        if (dog1_rb.velocity.magnitude < 0.2f)
        {
            if (dog1Area) //si esta en rango de lobo
                animatorD1.Play("Idle_Shooting");
            else
                animatorD1.Play("Idle");
        }else if(dog1_rb.velocity.magnitude > 0.2f)
        {
            if (dog1Area) //si esta en rango de lobo
                animatorD1.Play("Run_Shooting");
            else
                animatorD1.Play("Run");
        }
    }

    private void UpdateAnimationD2()
    {
        if (dog2_rb.velocity.x > 0.2f)
            dog2.transform.localScale = new Vector3(2.25f, dog2.transform.localScale.y, dog2.transform.localScale.z);
        else
            dog2.transform.localScale = new Vector3(-2.25f, dog2.transform.localScale.y, dog2.transform.localScale.z);

        if (dog2_rb.velocity.magnitude < 0.2f)
        {
            if (dog2Area) //si esta en rango de lobo
                animatorD2.Play("Idle_Shooting");
            else
                animatorD2.Play("Idle");
        }
        else if (dog2_rb.velocity.magnitude > 0.2f)
        {
            if (dog2Area) //si esta en rango de lobo
                animatorD2.Play("Run_Shooting");
            else
                animatorD2.Play("Run");
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
