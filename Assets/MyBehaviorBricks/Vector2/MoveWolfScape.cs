using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace BBUnity.Actions
{
    /// <summary>
    /// It is an action to move the GameObject to a given position.
    /// </summary>
    [Action("Navigation/MoveWolfScape")]
    [Help("Moves the game object to a given position by using a NavMeshAgent")]
    public class MoveWolfScape : GOAction
    {
        [InParam("sheep_wolfSpeed")]
        [Help("Target position where the game object will be moved")]
        public float sheep_wolfSpeed;

        [InParam("wolfLayer")]
        [Help("Target to check the distance")]
        public LayerMask wolfLayer;

        ///<value>Input maximun distance Parameter to consider that the target is close.</value>
        [InParam("sheepAreaRange_WOLF")]
        [Help("The maximun distance to consider that the target is close")]
        public float sheepAreaRange_WOLF;

        Collider2D[] hitCollider;
        public Vector2 sheepDirection;
        private Rigidbody2D rb;

        public override void OnStart()
        {
            rb = gameObject.GetComponent<Rigidbody2D>();

        }

        public override TaskStatus OnUpdate()
        {
            WolfScape();
            rb.velocity = new Vector2(sheepDirection.x * sheep_wolfSpeed, sheepDirection.y * sheep_wolfSpeed);

            if(hitCollider.Length == 0)
            {
                return TaskStatus.COMPLETED;
            }

            return TaskStatus.RUNNING;
        }

        public void WolfScape()
        {

            if (Physics2D.OverlapCircle(gameObject.transform.position, sheepAreaRange_WOLF, wolfLayer))
            {
                hitCollider = Physics2D.OverlapCircleAll(gameObject.transform.position, sheepAreaRange_WOLF, wolfLayer);

                Vector2 wolfPos = (Vector2)hitCollider[0].gameObject.transform.position;

                Vector2 newPos = (Vector2)gameObject.transform.position - wolfPos; //direccion contraria a la posicion del perro

                sheepDirection = newPos.normalized;

            }

        }
    }
}
