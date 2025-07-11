using System.Collections;
using UnityEngine;

public class Grass : MonoBehaviour
{
    [SerializeField] private float grassLife = 25;
    [SerializeField] private float sheepActionRange = 2.5f;
    [SerializeField] private LayerMask sheepLayer;

    bool grassDeactivated = false;

    void Update()
    {
        if(!grassDeactivated)
            GrassLife();

        if (grassLife <= 0 && !grassDeactivated)
            StartCoroutine(RespawnGrass());
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

    private IEnumerator RespawnGrass()
    {
        grassDeactivated = true;

        this.GetComponent<SpriteRenderer>().enabled = false;
        this.GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(10);

        grassLife = 25;
        grassDeactivated = false;

        this.GetComponent<SpriteRenderer>().enabled = true;
        this.GetComponent<Collider2D>().enabled = true;
    }
}
