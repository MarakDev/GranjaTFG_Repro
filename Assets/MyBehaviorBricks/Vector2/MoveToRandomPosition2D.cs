using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;

namespace BBUnity.Actions
{
    /// <summary>
    /// It is an action to move the GameObject to a given position.
    /// </summary>
    [Action("Navigation/MoveToRandomPosition2D")]
    [Help("Moves the game object to a given position by using a NavMeshAgent")]
    public class MoveToRandomPosition2D : GOAction
    {
        [InParam("wanderSpeed")]
        [Help("Target position where the game object will be moved")]
        public float wanderSpeed;

        [InParam("wanderSeconds")]
        [Help("Target position where the game object will be moved")]
        public Vector2 wanderSeconds;

        private Vector2 direction;
        private Rigidbody2D rb;
        private float totalSeconds;

        public override void OnStart()
        {
            rb = gameObject.GetComponent<Rigidbody2D>();

            totalSeconds = Random.Range(wanderSeconds.x, wanderSeconds.y);

            //random direction
            direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        }

        public override TaskStatus OnUpdate()
        {
            rb.velocity = new Vector2(direction.x * wanderSpeed, direction.y * wanderSpeed);

            if (totalSeconds <= 0)
            {
                rb.velocity = Vector2.zero;
                return TaskStatus.COMPLETED;
            }

            totalSeconds -= Time.deltaTime;

            return TaskStatus.RUNNING;
        }

    }
}
