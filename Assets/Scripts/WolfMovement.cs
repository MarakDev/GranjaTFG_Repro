
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class WolfMovement : MonoBehaviour
{
    [SerializeField] private float wolfSpeed = 7.5f;
    [SerializeField] private float wolfAfraidSpeed = 10;
    [SerializeField] private float maxLife = 5;

    [SerializeField] private float sheepHarashRange;
    [SerializeField] private float dogAfraidRange;

    [SerializeField] public float wolfStartCooldown;
    [SerializeField] public float wolfRestartCooldown;

    [SerializeField] private GameObject smokeFire;
    [SerializeField] private GameObject smokeDeath;
    [SerializeField] private LayerMask sheepLayer;
    [SerializeField] private LayerMask dogLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 direction;

    private GameObject sheepCollection;
    private GameObject activeSheep;

    private float speed;
    private float currentLife;

    private bool wolfActive = false;
    private bool wolfSTOP = false;

    private bool wolfUnderFire = false;
    private bool wolfEscape = false;

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
            wolfUnderFire = Physics2D.OverlapCircle(transform.position, dogAfraidRange, dogLayer);

            SheepChase();

            UnderDogFire();

            WolfScape();
        }

        UpdateAnimations();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(direction.x * speed, direction.y * speed);

    }

    private void WolfScape()
    {
        if(currentLife > maxLife && !wolfEscape)
        {
            float dirX = Random.Range(-1f, 1f);
            float dirY = 1 - dirX;

            if(dirX < 0f)
                dirY = 1 + dirX;

            direction = new Vector2(dirX, dirY);

            speed = wolfAfraidSpeed;

            wolfEscape = true;
            currentLife = 0;
        }

    }

    private void UnderDogFire()
    {
        if (wolfUnderFire && !wolfEscape)
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, dogAfraidRange, dogLayer);

            if (hitColliders.Length == 2)
            {

                float velReduction = 1 - (currentLife * maxLife / 7.5f);

                if (velReduction < 0.1)
                    velReduction = 0.1f;

                currentLife += Time.deltaTime * 3;
                speed = wolfSpeed * velReduction;

            }
            else if (hitColliders.Length == 1)
            {
                float velReduction = 1 - (currentLife * maxLife / 15);

                if (velReduction < 0.25)
                    velReduction = 0.25f;

                currentLife += Time.deltaTime;
                speed = wolfSpeed * velReduction;

            }

        }

    }

    private void SheepChase()
    {
        if(!wolfUnderFire && !wolfEscape)
        {
            wolfUnderFire = false;
            speed = wolfSpeed;

            direction = (activeSheep.transform.position - transform.position).normalized;

            if (Physics2D.OverlapCircle(transform.position, sheepHarashRange, sheepLayer) || activeSheep.gameObject.layer == LayerMask.NameToLayer("SheepIsCaged"))
            {
                SheepSelect();
            }

        }
    }

    private void SheepSelect()
    {
        int totalSheepsActive = sheepCollection.transform.childCount;

        int randomSheep = Random.Range(0, totalSheepsActive);

        if(sheepCollection.transform.childCount > 0)
            activeSheep = sheepCollection.transform.GetChild(randomSheep).gameObject;

    }

    private void UpdateAnimations()
    {

        if (rb.velocity.x > 0.2f)
            transform.localScale = new Vector3(3, transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(-3, transform.localScale.y, transform.localScale.z);

        //sheep chase
        if (!wolfUnderFire && !wolfEscape)
        {
            if (currentLife < maxLife * 0.5f)
                animator.Play("Idle");
            else
            {
                smokeFire.SetActive(true);
                animator.Play("Idle_OnFire");

            }
        }
        else if(wolfUnderFire && !wolfEscape) //siendo disparado por el perro
        {
            if (currentLife < maxLife * 0.5f)
            {

                animator.Play("Shooting");
            }
            else
            {
                smokeFire.SetActive(true);
                animator.Play("Shooting_OnFire");

            }
        }
        else if (wolfEscape)
        {
            smokeDeath.SetActive(true);

            animator.Play("Death");
        }
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

        smokeDeath.SetActive(false);
        smokeFire.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("WolfDeactivate"))
        {
            wolfSTOP = true;
            wolfActive = false;
            wolfEscape = false;
            speed = 0;

            StartCoroutine(WolfActive(wolfRestartCooldown));
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        if (wolfUnderFire)
            Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, dogAfraidRange);

    }

}
