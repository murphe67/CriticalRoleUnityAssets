using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriticalRole.BattleCamera;
using CriticalRole.Contents;
using CriticalRole.Turns;
using CriticalRole.Alterations;
using CriticalRole.Narration;
using CriticalRole.Death;

namespace CriticalRole.Attacking
{
    public interface IAttackManager
    {
        void RegisterAlterationManager(IAlterationManager alterationManager);
        void Attack(IHasTurn attacker, IContents victim);

        void RegisterCamManager(IBattleCamManager battleCamManager);

        void RegisterNarrator(INarrator narrator);

        void AddEndAttackEvent(IEndAttackEvent endAttackEvent);

        void RemoveEndAttackEvent(IEndAttackEvent endAttackEvent);
    }

    [RequireComponent(typeof(AttackManagerMarker))]
    public class AttackManager : MonoBehaviour, IAttackManager
    {
        //----------------------------------------------------------------------------
        //             Initialise
        //----------------------------------------------------------------------------

        #region Initialise

        public void Awake()
        {
            Initialise();
        }

        public void Initialise()
        {
            EndAttackEvents = new List<IEndAttackEvent>();
            _DependancyInjectionAttackControllers();
            _DependancyInjectionCanDie();
            _DependancyInjectionDeathManager();
        }



        #region Implementation

        private void _DependancyInjectionAttackControllers()
        {
            IAttackControllerMarker[] attackControllerMarkers = FindObjectsOfType<IAttackControllerMarker>();
            foreach (IAttackControllerMarker attackControllerMarker in attackControllerMarkers)
            {
                IAttackController attackController = attackControllerMarker.gameObject
                                                     .GetComponent<IAttackController>();
                attackController.Initialise(this);
            }
        }

        private void _DependancyInjectionCanDie()
        {
            HasTurnMarker[] hasTurnMarkers = FindObjectsOfType<HasTurnMarker>();
            foreach(HasTurnMarker hasTurnMarker in hasTurnMarkers)
            {
                hasTurnMarker.gameObject.GetComponent<ICanDie>().RegisterAttackManager(this);
            }
        }

        private void _DependancyInjectionDeathManager()
        {
            DeathManagerMarker[] deathManagerMarkers = FindObjectsOfType<DeathManagerMarker>();
            foreach (DeathManagerMarker deathManagerMarker in deathManagerMarkers)
            {
                deathManagerMarker.gameObject.GetComponent<IDeathManager>().RegisterAttackManager(this);
            }
        }

        #endregion

        #endregion





        //----------------------------------------------------------------------------
        //             RegisterCamManager
        //----------------------------------------------------------------------------

        #region RegisterCamManager

        public void RegisterCamManager(IBattleCamManager battleCamManager)
        {
            MyBattleCamManager = battleCamManager;
        }

        IBattleCamManager MyBattleCamManager;

        #endregion



        //----------------------------------------------------------------------------
        //             RegisterNarrator
        //----------------------------------------------------------------------------

        #region RegisterNarrator

        public void RegisterNarrator(INarrator narrator)
        {
            MyNarrator = narrator;
        }

        INarrator MyNarrator;


        #endregion


        //----------------------------------------------------------------------------
        //             RegisterAlterationManager
        //----------------------------------------------------------------------------

        #region RegisterAlterationManager
        public void RegisterAlterationManager(IAlterationManager alterationManager)
        {
            MyAlterationManager = alterationManager;
        }

        public IAlterationManager MyAlterationManager;

        #endregion





        //----------------------------------------------------------------------------
        //             Attack
        //----------------------------------------------------------------------------

        #region Attack

        public void Attack(IHasTurn attacker, IContents victim)
        {
            MyAttackerTurn = attacker;
            MyVictimContents = victim;

            MyHasAttack = MyAttackerTurn.MyHasAttack;
            MyIsVictim = MyVictimContents.MyHasTurn.MyIsVictim;

            StartCoroutine(AttackCoroutine());
        }

        IHasTurn MyAttackerTurn;
        IContents MyVictimContents;

        IHasAttack MyHasAttack;
        IIsVictim MyIsVictim;

        #endregion





        //----------------------------------------------------------------------------
        //             AttackCoroutine
        //----------------------------------------------------------------------------

        #region AttackCoroutine

        public IEnumerator AttackCoroutine()
        {
            yield return StartCoroutine(MyBattleCamManager.SwitchToScriptCamera());
            _RotateCharactersToFaceEachOther();

            Coroutine AttackFocusCoroutine = _StartAttackFocus();


            //get attack data from attacker
            IAttackData attackData = MyHasAttack.GetAttackData();
            attackData.SetIsVictim(MyIsVictim);

            MyAlterationManager.MyAttackDataAlterer.Alter(attackData);

            yield return StartCoroutine(MyNarrator.AttemptToAttack(attackData));


            //pass control to rolling for attack
            //to allow it to ask for input from the user if needed
            yield return StartCoroutine(attackData.RollForAttack());

            //pass control to checking attack roll against ac
            //to allow it to ask for input from the user if needed
            yield return MyIsVictim.DoesAttackRollHit(attackData.MyAttackRoll);

            if (attackData.MyAttackRoll.DidHit)
            {
                yield return StartCoroutine(MyNarrator.AttackHit(attackData.MyAttackRoll));

                //pass control to rolling damage
                //to allow it to ask for input from the user if needed
                yield return MyHasAttack.RollDamage(attackData.MyAttackRoll);

                MyAlterationManager.MyDamageDataAlterer.AlterDamage(MyHasAttack, MyIsVictim, attackData);

                //pass control to taking damage
                //to allow it to ask for input from the user if needed
                yield return MyIsVictim.TakeDamage(attackData.MyAttackRoll);
            }
            else
            {
                yield return StartCoroutine(MyNarrator.AttackMiss(attackData.MyAttackRoll));
            }


            StopCoroutine(AttackFocusCoroutine);
            yield return new WaitForSeconds(0.4f);

            StartCoroutine(EndAttack());
        }

        #region Implementation

        private void _RotateCharactersToFaceEachOther()
        {
            //this is gonna be strange if you're attacking like a barrel,
            //and might have to be replaced with a function inside of hex contents

            MyVictimContents.ContentTransform.LookAt(MyAttackerTurn.MyHexContents.ContentTransform);
            MyAttackerTurn.MyHexContents.ContentTransform.LookAt(MyVictimContents.ContentTransform);
        }

        private Coroutine _StartAttackFocus()
        {
            return StartCoroutine(MyBattleCamManager.MyAttackFocus
                                                .FocusOnAttack(MyAttackerTurn.MyHexContents.ContentTransform,
                                                 MyVictimContents.ContentTransform));
        }

        #endregion

        #endregion

        public IEnumerator EndAttack()
        {
            EndAttackEvents.Sort(new EndAttackEventSort());
            List<IEndAttackEvent> temp = new List<IEndAttackEvent>(EndAttackEvents);
            foreach (IEndAttackEvent endAttackEvent in temp)
            {
                yield return StartCoroutine(endAttackEvent.EndAttackEvent(MyAttackerTurn));
            }
            yield return StartCoroutine(MyBattleCamManager.SwitchToPlayerCamera());
            MyAttackerTurn.EndAttack();
        }

        public List<IEndAttackEvent> EndAttackEvents;
        public void AddEndAttackEvent(IEndAttackEvent endAttackEvent)
        {
            EndAttackEvents.Add(endAttackEvent);
        }

        public void RemoveEndAttackEvent(IEndAttackEvent endAttackEvent)
        {
            EndAttackEvents.Remove(endAttackEvent);
        }
    }
}

