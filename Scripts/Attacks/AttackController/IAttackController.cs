using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CriticalRole.Attacking
{

    //----------------------------------------------------------------------------
    //              Class Description
    //----------------------------------------------------------------------------
    //
    // The AI vs player attack controllers are going to be very different
    //
    // This shared base implementation allows the AttackManager to dependancy inject
    // into them both.
    //
    // It's possible this will be the only similiarity.
    //

    public interface IAttackController
    {
        void Initialise(IAttackManager attackManager);
    }

}
