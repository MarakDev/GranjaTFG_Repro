using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //var publics

    [SerializeField] private float speed;

    [SerializeField] private float rabitAreaRange;
    [SerializeField] private LayerMask rabitsLayer;


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

    }

    private void Update()
    {
        //inputs
        dog1_moveDirection = dog1_input.ReadValue<Vector2>();
        dog2_moveDirection = dog2_input.ReadValue<Vector2>();

        if (dog1 != null)
            dog1Area = Physics2D.OverlapCircle(dog1.transform.position, rabitAreaRange, rabitsLayer);
        if (dog2 != null)
            dog2Area = Physics2D.OverlapCircle(dog2.transform.position, rabitAreaRange, rabitsLayer);

    }

    private void FixedUpdate()
    {
        dog1_rb.velocity = new Vector2(dog1_moveDirection.x * speed, dog1_moveDirection.y * speed);
        dog2_rb.velocity = new Vector2(dog2_moveDirection.x * speed, dog2_moveDirection.y * speed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        if (dog1Area)
            Gizmos.color = Color.red;

        if (dog1 != null)
            Gizmos.DrawWireSphere(dog1.transform.position, rabitAreaRange);

        Gizmos.color = Color.yellow;

        if(dog2Area)
            Gizmos.color = Color.red;

        if(dog2 != null)
            Gizmos.DrawWireSphere(dog2.transform.position, rabitAreaRange);
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
