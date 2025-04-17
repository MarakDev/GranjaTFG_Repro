using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animaciones_Oveja : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(rb.velocity.magnitude) >= 0.2f && Mathf.Abs(rb.velocity.magnitude) < 1.25f)
        {
            animator.Play("Walk");
        }
        else if (Mathf.Abs(rb.velocity.magnitude) >= 1.25f && Mathf.Abs(rb.velocity.magnitude) < 4.1f)
        {
            animator.Play("Run");
        }
        else if (Mathf.Abs(rb.velocity.magnitude) >= 4.1f)
        {
            animator.Play("RunMore");
        }
        else
        {
            animator.Play("Idle");
        }

        if (rb.velocity.x < 0)
            transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        else
            transform.localScale = new Vector3(-2.5f, 2.5f, 2.5f);
    }
}
