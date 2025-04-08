using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;

namespace BBUnity.Actions
{
    /// <summary>
    /// It is an action to move the GameObject to a given position.
    /// </summary>
    [Action("Navigation/MoveDogScape")]
    [Help("Moves the game object to a given position by using a NavMeshAgent")]
    public class MoveDogScape : GOAction
    {
        [InParam("dogSpeed")]
        [Help("Target position where the game object will be moved")]
        public float sheep_dogSpeed;

        [InParam("dogColliders")]
        public Collider2D[] hitColliders;

        public Vector2 sheepDirection;
        public float dogSpeedMult;
        private Rigidbody2D rb;


        public override void OnStart()
        {
            rb = gameObject.GetComponent<Rigidbody2D>();

        }

        public override TaskStatus OnUpdate()
        {
            DogScape();
            rb.velocity = new Vector2(sheepDirection.x * sheep_dogSpeed * dogSpeedMult, sheepDirection.y * sheep_dogSpeed * dogSpeedMult);

            if(hitColliders.Length == 0)
            {
                return TaskStatus.COMPLETED;
            }

            return TaskStatus.RUNNING;
        }

        public void DogScape()
        {
            dogSpeedMult = 1f; //empieza en 0.25 y aumenta un 0.25 por cada perro

            Debug.Log("hitColliders[0].gameObject: " + hitColliders[0]);
            Vector2 dogPos = (Vector2)hitColliders[0].gameObject.transform.position; //posicion del collider perro numero 1
            Vector2 vector_NewDir; //vector de nueva direccion

            if (hitColliders.Length > 1)
            {
                dogSpeedMult = 1.5f;
                Vector2 dogPos2 = (Vector2)hitColliders[1].gameObject.transform.position;

                Vector2 vector_dog1_sheep = (Vector2)gameObject.transform.position - dogPos;
                Vector2 vector_dog2_sheep = (Vector2)gameObject.transform.position - dogPos2;

                vector_NewDir = vector_dog2_sheep + vector_dog1_sheep; //direccion contraria a la posicion del perro
            }
            else
            {
                vector_NewDir = (Vector2)gameObject.transform.position - dogPos; //direccion contraria a la posicion del perro
            }

            sheepDirection = vector_NewDir.normalized;
        }
    }
}
