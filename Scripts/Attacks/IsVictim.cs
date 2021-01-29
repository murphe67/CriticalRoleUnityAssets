using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriticalRole.Armored;
using CriticalRole.Health;
using CriticalRole.Narration;
using CriticalRole.Death;

//----------------------------------------------------------------------------
//             Class Description
//----------------------------------------------------------------------------
//
// Communication point for recieving attacks from the attack manager
//
// Damage is definitely going to go through here.
// Possibly other things.

namespace CriticalRole.Attacking
{

    //----------------------------------------------------------------------------
    //             IisVictimInterface
    //----------------------------------------------------------------------------

    #region IIsVictimInterface

    public interface IIsVictim
    {
        void Initialise();

        void RegisterACAlterer(IACAlterer acAlterer);
        void RegisterACSetter(IACSetter acSetter);

        void RegisterNarrator(Narrator narrator);

        IEnumerator DoesAttackRollHit(AttackRoll attackRoll);

        IEnumerator TakeDamage(AttackRoll attackRoll);





        string Name { get; }

        int AC { get; }

        IHasHealth MyHasHealth { get; }
    }

    #endregion



    public class IsVictim : MonoBehaviour, IIsVictim
    {


        //----------------------------------------------------------------------------
        //            Dependancy Injection
        //----------------------------------------------------------------------------

        #region Dependancy Injection
        public void RegisterACAlterer(IACAlterer acAlterer)
        {
            MyACAlterer = acAlterer;
            _ACAltererSet = true;
        }

        IACAlterer MyACAlterer;
        private bool _ACAltererSet = false;

        public void RegisterACSetter(IACSetter acSetter)
        {
            MyACSetter = acSetter;
            _ACSetterSet = true;
        }

        public IACSetter MyACSetter;
        private bool _ACSetterSet = false;

        public void RegisterNarrator(Narrator narrator)
        {
            MyNarrator = narrator;
        }

        public Narrator MyNarrator;

        #endregion






        //----------------------------------------------------------------------------
        //            Initialise
        //----------------------------------------------------------------------------

        #region Initialise
        public void Initialise()
        {
            if(!_ACAltererSet)
            {
                Debug.LogError("Error: No dependancy injection from AC Alterer");
            }
            if (!_ACSetterSet)
            {
                Debug.LogError("Error: No dependancy injection from AC Setter");
            }

            MyACSetter.SetAC(this, MyACAlterer);

            

            MyHasHealth = GetComponent<IHasHealth>();
            MyCanDie = GetComponent<ICanDie>();
        }

        #endregion




        //----------------------------------------------------------------------------
        //            Narrator Info
        //----------------------------------------------------------------------------

        #region Narrator Info

        public string Name
        {
            get
            {
                return gameObject.name;
            }
        }

        public int AC
        {
            get
            {
                return MyACAlterer.GetAC(this);
            }
        }

        #endregion




        //----------------------------------------------------------------------------
        //            DoesAttackRollHit
        //----------------------------------------------------------------------------
        
        #region DoesAttackRollHit

        public IEnumerator DoesAttackRollHit(AttackRoll attackRoll)
        {
            attackRoll.DidHit = attackRoll.Roll > MyACAlterer.GetAC(this);
            yield break;
        }

        #endregion




        //----------------------------------------------------------------------------
        //            TakeDamage
        //----------------------------------------------------------------------------

        #region TakeDamage
        public IEnumerator TakeDamage(AttackRoll attackRoll)
        {
            int damage = attackRoll.MyDamageData.GetDamage();
            MyHasHealth.TakeDamage(attackRoll.MyDamageData.GetDamage());

            if (MyHasHealth.GetHealth() != 0)
            {
                
                yield return StartCoroutine(MyNarrator.Damage());
            }
            else
            {
                yield return StartCoroutine(MyCanDie.TakeAttack(attackRoll));
            }
        }

        public IHasHealth MyHasHealth { get; set; }
        public ICanDie MyCanDie { get; set; }

        #endregion
    }

}

