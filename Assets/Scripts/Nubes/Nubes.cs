using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nubes : MonoBehaviour
{
    [SerializeField] private float speed;

    // Update is called once per frame
    void Update()
    {
        if(this.transform.position.x > 115)
            transform.position = new Vector2(-112, transform.position.y);

    }

    private void FixedUpdate()
    {
        transform.position = new Vector2(transform.position.x + speed, transform.position.y);

    }
}
