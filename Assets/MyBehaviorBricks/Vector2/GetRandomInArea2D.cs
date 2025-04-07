using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;

namespace BBUnity.Actions
{
    /// <summary>
    /// It is an action to obtain a random position of an area.
    /// </summary>
    [Action("Vector2/GetRandomInArea2D")]
    [Help("Gets a random position from a given area")]
    public class GetRandomInArea2D : GOAction
    {

        [InParam("area")]
        [Help("GameObject that must have a BoxCollider or SphereColider, which will determine the area from which the position is extracted")]
        public GameObject area { get; set; }


        [OutParam("randomPosition")]
        [Help("Position randomly taken from the area")]
        public Vector2 randomPosition { get; set; }


        public override void OnStart()
        {
            if (area == null)
            {
                Debug.LogError("The area of moving is null", gameObject);
                return;
            }
            
            randomPosition = new Vector2(UnityEngine.Random.Range(area.transform.position.x - area.transform.localScale.x * area.transform.localScale.x * 0.5f,
                                                                      area.transform.position.x + area.transform.localScale.x * area.transform.localScale.x * 0.5f),
                                             UnityEngine.Random.Range(area.transform.position.y - area.transform.localScale.y * area.transform.localScale.y * 0.5f,
                                                                      area.transform.position.y + area.transform.localScale.y * area.transform.localScale.y * 0.5f));
            

            Debug.Log("randomPos: " + randomPosition);

            randomPosition = new Vector2(1, 2);

        }

        /// <summary>Abort method of GetRandomInArea.</summary>
        /// <remarks>Complete the task.</remarks>
        public override TaskStatus OnUpdate()
        {
            return TaskStatus.COMPLETED;
        }
    }
}
