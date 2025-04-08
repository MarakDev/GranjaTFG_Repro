using Pada1.BBCore;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace BBUnity.Conditions
{
    /// <summary>
    /// It is a perception condition to check if the objective is close depending on a given distance.
    /// </summary>
    [Condition("Perception/IsDogClose")]
    [Help("Checks whether a target is close depending on a given distance")]
    public class IsDogClose : GOCondition
    {
        ///<value>Input Target Parameter to to check the distance.</value>
        [InParam("dogLayer")]
        [Help("Target to check the distance")]
        public LayerMask dogLayer;

        ///<value>Input maximun distance Parameter to consider that the target is close.</value>
        [InParam("dogAreaRange")]
        [Help("The maximun distance to consider that the target is close")]
        public float sheepAreaRange_DOG;

        [OutParam("dogColliders")]
        public Collider2D[] hitColliders;

        /// <summary>
        /// Checks whether a target is close depending on a given distance,
        /// calculates the magnitude between the gameobject and the target and then compares with the given distance.
        /// </summary>
        /// <returns>True if the magnitude between the gameobject and de target is lower that the given distance.</returns>
        public override bool Check()
        {
            if (Physics2D.OverlapCircle(gameObject.transform.position, sheepAreaRange_DOG, dogLayer))
            {
                hitColliders = Physics2D.OverlapCircleAll(gameObject.transform.position, sheepAreaRange_DOG, dogLayer);

                Debug.Log("hitcolliders: " + hitColliders[0].gameObject.name);

                return true;
            }

            return false;
        }
    }
}