using Pada1.BBCore.Framework;
using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;

namespace BBCore.Actions
{

    /// <summary>
    /// Implementation of the wait action using busy-waiting (spinning).
    /// </summary>
    [Action("Basic/WaitForSecondsRandomRange")]
    [Help("Action that success after a period of time.")]
    public class WaitForSecondsRandomRange : BasePrimitiveAction
    {
        ///<value>Input Amount of time to wait (in seconds) Parameter.</value>
        [InParam("seconds")]
        [Help("Amount of time to wait (in seconds)")]
        public Vector2 secondsRange;

        [OutParam("waitSeconds")]
        [Help("Amount of time to wait (in seconds)lllll")] 
        public float waitSeconds;

        private float elapsedTime;

        /// <summary>Initialization Method of WaitForSeconds.</summary>
        /// <remarks>Initializes the elapsed time to 0.</remarks>
        public override void OnStart()
        {
            elapsedTime = 0;
            waitSeconds = Random.Range(secondsRange.x, secondsRange.y);

            Debug.Log("wait seconds in wait seconds : " + waitSeconds);
        }

        /// <summary>Method of Update of WaitForSeconds.</summary>
        /// <remarks>Increase the elapsed time and check if you have exceeded the waiting time has ended.</remarks>
        public override TaskStatus OnUpdate()
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= waitSeconds)
                return TaskStatus.COMPLETED;
            return TaskStatus.RUNNING;
        }
    }
}
