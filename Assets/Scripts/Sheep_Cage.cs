using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sheep_Cage : MonoBehaviour
{


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Sheep"))
        {
            collision.gameObject.transform.SetParent(transform, true);
            collision.gameObject.layer = LayerMask.NameToLayer("SheepIsCaged");
            collision.gameObject.GetComponent<SheepMovement>().GetCaged();
        }
    }

}
