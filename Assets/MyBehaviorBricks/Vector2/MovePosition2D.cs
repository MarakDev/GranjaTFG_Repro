using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;

namespace BBUnity.Actions
{
    /// <summary>
    /// It is an action to move the GameObject to a given position.
    /// </summary>
    [Action("Navigation/MoveToPosition2D")]
    [Help("Moves the game object to a given position by using a NavMeshAgent")]
    public class MovePosition2D : GOAction
    {
        [InParam("wanderSpeed")]
        [Help("Target position where the game object will be moved")]
        public float wanderSpeed;

        [InParam("time")]
        [Help("Target position where the game object will be moved")]
        public float timeWander;

        private Vector2 target;
        public Rigidbody2D rb_sheep { get; set; }
        public override void OnStart()
        {
            rb_sheep = gameObject.GetComponent<Rigidbody2D>();
            Debug.Log("tiemwander start: " + timeWander);

            //random direction
            target = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        }

        public override TaskStatus OnUpdate()
        {
            rb_sheep.velocity = new Vector2(target.x * wanderSpeed, target.y * wanderSpeed);

            if (rb_sheep.position == target || timeWander <= 0)
            {
                rb_sheep.velocity = Vector2.zero;
                return TaskStatus.COMPLETED;
            }

            timeWander -= Time.deltaTime;
            Debug.Log("tiemwander: " + timeWander);

            return TaskStatus.RUNNING;
        }

    }
}
