using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    [SerializeField] private float grassLife = 50;
    [SerializeField] private float sheepActionRange = 2;
    [SerializeField] private LayerMask sheepLayer;



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GrassLife();

        if (grassLife <= 0)
            Destroy(gameObject);
    }

    private void GrassLife()
    {
        Collider2D[] hitGrass = Physics2D.OverlapCircleAll(transform.position, sheepActionRange, sheepLayer);
        int nOfSheep = 0;

        if (hitGrass != null)
        {
            for (int i = 0; i < hitGrass.Length; i++)
            {
                if (hitGrass[i].GetComponent<SheepController>().StateMachine.CurrentState.ToString() == "Sheep_EatState")
                    nOfSheep++;
            }

            grassLife -= Time.deltaTime * nOfSheep;
        }
    }

    private void OnDestroy()
    {
        
    }
}
