
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class WolfMovement : MonoBehaviour
{
    [SerializeField] private float wolfSpeed = 7.5f;
    [SerializeField] private float wolfAfraidSpeed = 10;
    [SerializeField] private float maxFear = 5;

    [SerializeField] private float sheepHarashRange;
    [SerializeField] private float dogAfraidRange;

    [SerializeField] public float wolfStartCooldown;
    [SerializeField] public float wolfRestartCooldown;


    [SerializeField] private LayerMask sheepLayer;
    [SerializeField] private LayerMask dogLayer;
    [SerializeField] private GameObject smoke;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 direction;

    private GameObject sheepCollection;
    private GameObject activeSheep;

    private float speed;
    private float wolfFear;

    private bool wolfActive = false;
    private bool wolfSTOP = false;

    private bool wolfAfraid = false;
    private bool wolfScape = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();

        StartCoroutine(WolfActive(wolfStartCooldown));
    }


    void Update()
    {
        if (!wolfSTOP && wolfActive)
        {
            wolfAfraid = Physics2D.OverlapCircle(transform.position, dogAfraidRange, dogLayer);

            SheepChase();

            DogAfraid();

            DogScape();

            if (wolfFear > maxFear * 0.5f)
                animator.SetBool("onFire", true);
        }

    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(direction.x * speed, direction.y * speed);

    }

    private void DogScape()
    {
        if(wolfFear > maxFear && !wolfScape)
        {
            float dirX = Random.Range(-1f, 1f);
            float dirY = 1 - dirX;

            if(dirX < 0f)
                dirY = 1 + dirX;

            direction = new Vector2(dirX, dirY);

            speed = wolfAfraidSpeed;

            wolfScape = true;
            wolfFear = 0;

            animator.Play("Death");

            smoke.SetActive(true);

            this.GetComponent<CircleCollider2D>().enabled = false;
        }

    }

    private void DogAfraid()
    {
        if (wolfAfraid && !wolfScape)
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, dogAfraidRange, dogLayer);

            if (hitColliders.Length == 2)
            {

                float velReduction = 1 - (wolfFear * maxFear / 7.5f);

                if (velReduction < 0.1)
                    velReduction = 0.1f;

                wolfFear += Time.deltaTime * 3;
                speed = wolfSpeed * velReduction;

            }
            else if (hitColliders.Length == 1)
            {
                float velReduction = 1 - (wolfFear * maxFear / 15);

                if (velReduction < 0.25)
                    velReduction = 0.25f;

                wolfFear += Time.deltaTime;
                speed = wolfSpeed * velReduction;

            }

            if (wolfFear < maxFear * 0.5f)
                animator.SetBool("shooting", true);
            else
                animator.SetBool("shooting_OnFire", true);


        }

    }

    private void SheepChase()
    {
        if(!wolfAfraid && !wolfScape)
        {
            wolfAfraid = false;
            speed = wolfSpeed;

            direction = (activeSheep.transform.position - transform.position).normalized;

            if (Physics2D.OverlapCircle(transform.position, sheepHarashRange, sheepLayer) || activeSheep.gameObject.layer == LayerMask.NameToLayer("SheepIsCaged"))
            {
                SheepSelect();
            }

            if (wolfFear < maxFear * 0.5f)
                animator.SetBool("shooting", false);
            else
                animator.SetBool("shooting_OnFire", false);

        }
    }

    private void SheepSelect()
    {
        int totalSheepsActive = sheepCollection.transform.childCount;

        int randomSheep = Random.Range(0, totalSheepsActive);

        if(sheepCollection.transform.childCount > 0)
            activeSheep = sheepCollection.transform.GetChild(randomSheep).gameObject;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        if (wolfAfraid)
            Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, dogAfraidRange);

    }

    private IEnumerator WolfActive(float cooldown)
    {
        float coldownRange = (int)Random.Range(cooldown -3, cooldown +3);

        yield return new WaitForSeconds(coldownRange);

        if (sheepCollection == null)
            sheepCollection = GameObject.FindGameObjectWithTag("SheepFree");

        SheepSelect();

        wolfSTOP = false;
        wolfActive = true;

        smoke.SetActive(false);

        this.GetComponent<CircleCollider2D>().enabled = true;

        animator.SetBool("shooting", false);
        animator.SetBool("shooting_OnFire", false);
        animator.SetBool("onFire", false);
        animator.Play("Idle");

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("WolfDeactivate"))
        {
            wolfSTOP = true;
            wolfActive = false;
            wolfScape = false;
            speed = 0;

            StartCoroutine(WolfActive(wolfRestartCooldown));
        }
    }

}
