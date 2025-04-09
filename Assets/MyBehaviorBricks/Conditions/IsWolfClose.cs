using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace BBUnity.Conditions
{
    /// <summary>
    /// It is a perception condition to check if the objective is close depending on a given distance.
    /// </summary>
    [Condition("Perception/IsWolfClose")]
    [Help("Checks whether a target is close depending on a given distance")]
    public class IsWolfClose : GOCondition
    {
        ///<value>Input Target Parameter to to check the distance.</value>
        [InParam("wolfLayer")]
        [Help("Target to check the distance")]
        public LayerMask wolfLayer;

        ///<value>Input maximun distance Parameter to consider that the target is close.</value>
        [InParam("sheepAreaRange_Wolf")]
        [Help("The maximun distance to consider that the target is close")]
        public float sheepAreaRange_WOLF;


        /// <summary>
        /// Checks whether a target is close depending on a given distance,
        /// calculates the magnitude between the gameobject and the target and then compares with the given distance.
        /// </summary>
        /// <returns>True if the magnitude between the gameobject and de target is lower that the given distance.</returns>
        public override bool Check()
        {
            if (Physics2D.OverlapCircle(gameObject.transform.position, sheepAreaRange_WOLF, wolfLayer))
                return true;
            else
                return false;
        }


    }
}