using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriticalRole.Attacking;
using CriticalRole.Damage;
using CriticalRole.Weapons;
using TMPro;
using CriticalRole.Turns;
using CriticalRole.Death;

namespace CriticalRole.Narration
{

    //----------------------------------------------------------------------------
    //                    Class Description
    //----------------------------------------------------------------------------
    //
    // Narrator class takes the calculations and actions and narrates them slowly
    // Providing the tension and proper pacing of a 5e battle
    //
    // Another stand-alone system that other systems will have references to
    //
    // Cannot be instancable because it has canvas children so the dependancy
    // injection is less headache than spawning all the components it needs

    public interface INarrator
    {
        IEnumerator AttemptToAttack(IAttackData attackData);
        IEnumerator AttackHit(AttackRoll attackRoll);
        IEnumerator Damage();
        IEnumerator AttackMiss(AttackRoll attackRoll);
    }


    public class Narrator : MonoBehaviour, INarrator
    {

        public Canvas MyCanvas;
        public CanvasGroup MyCanvasGroup;
        public TextMeshProUGUI MyTextMesh;

        public IAttackData MyAttackData;
        public AttackRoll MyAttackRoll;

        //----------------------------------------------------------------------------
        //                    Awake
        //----------------------------------------------------------------------------

        #region Awake

        public void Awake()
        {
            AttackManagerMarker[] attackManagerMarkers = FindObjectsOfType<AttackManagerMarker>();
            foreach (AttackManagerMarker attackManagerMarker in attackManagerMarkers)
            {
                attackManagerMarker.gameObject.GetComponent<IAttackManager>().RegisterNarrator(this);
            }

            HasTurnMarker[] hasTurnMarkers = FindObjectsOfType<HasTurnMarker>();
            foreach(HasTurnMarker hasTurnMarker in hasTurnMarkers)
            {
                GameObject turnGO = hasTurnMarker.gameObject;
                turnGO.GetComponent<IIsVictim>().RegisterNarrator(this);
                turnGO.GetComponent<ICanDie>().RegisterNarrator(this);
            }

            TurnControllerMarker[] turnControllerMarkers = FindObjectsOfType<TurnControllerMarker>();

            foreach (TurnControllerMarker turnControllerMarker in turnControllerMarkers)
            {
                turnControllerMarker.gameObject.GetComponent<ITurnController>().RegisterNarrator(this);
            }


            MyCanvas.enabled = false;
            MyCanvasGroup.alpha = 0;
        }

        #endregion




        //----------------------------------------------------------------------------
        //                    AttemptToAttack
        //----------------------------------------------------------------------------

        #region AttemptToAttack

        public IEnumerator AttemptToAttack(IAttackData attackData)
        {
            MyAttackData = attackData;
            yield return new WaitForSeconds(1f); ;

            MyTextMesh.text = attackData.MyHasAttack.MyName + " turns to look at " + attackData.MyIsVictim.Name
                            + ", before bringing their " + attackData.MyHasAttack.MyWeapon.MyWeaponType +
                            " down on them.";

            yield return StartCoroutine(ShowCanvas());

            while (!Input.GetMouseButton(0))
            {
                yield return null;
            }

            yield return new WaitForSeconds(0.2f);
        }

        #endregion



        //----------------------------------------------------------------------------
        //                    AttackHit
        //----------------------------------------------------------------------------

        #region AttackHit

        public IEnumerator AttackHit(AttackRoll attackRoll)
        {
            MyAttackRoll = attackRoll;
            MyTextMesh.text = MyAttackData.MyHasAttack.MyName + " rolls a " + MyAttackRoll.PureRoll + " + " 
                        + MyAttackData.SumOfModifiers + " to hit, hitting " + MyAttackData.MyIsVictim.Name + "."
                        + " It hits against their AC of " + MyAttackData.MyIsVictim.AC;

            while (!Input.GetMouseButton(0))
            {
                yield return null;
            }

            yield return new WaitForSeconds(0.2f);
        }

        #endregion




        //----------------------------------------------------------------------------
        //                    Damage
        //----------------------------------------------------------------------------

        #region Damage

        public IEnumerator Damage()
        {
            WeaponObject weapon = MyAttackData.MyHasAttack.MyWeapon;
            SingleDamageID weaponDamageID = new SingleDamageID(weapon.MyDamageType, DamageSource.Roll);
            SingleDamageID modDamageID = new SingleDamageID(weapon.MyDamageType, DamageSource.Mod);

            int weaponDamage = (int)MyAttackRoll.MyDamageData.MyDamageDict[weaponDamageID];
            int modDamage = (int)MyAttackRoll.MyDamageData.MyDamageDict[modDamageID];

            
            MyTextMesh.text = "The attack deals " + weaponDamage + " + " 
                            + modDamage + " = " +
                            MyAttackRoll.MyDamageData.GetDamage() + " damage. " +
                            MyAttackData.MyIsVictim.Name + " flinches from the pain." 
                            + "Their health is now " + MyAttackData.MyIsVictim.MyHasHealth.GetHealth() + "/" 
                            + MyAttackData.MyIsVictim.MyHasHealth.GetMaxHealth();
                            
            while (!Input.GetMouseButton(0))
            {
                yield return null;
            }

            yield return StartCoroutine(HideCanvas());
            yield return new WaitForSeconds(0.2f);
        }

        #endregion





        //----------------------------------------------------------------------------
        //                    Death
        //----------------------------------------------------------------------------

        public IEnumerator HasTurnDeath(IHasTurn hasTurn)
        {
            MyTextMesh.text = hasTurn.Name + " falls to the ground, clutching their chest. Their face drains "
                                + "of blood, before they collapse entirely. Dead.";

            while (!Input.GetMouseButton(0))
            {
                yield return null;
            }

            yield return StartCoroutine(HideCanvas());
            yield return new WaitForSeconds(0.2f);

        }


        //----------------------------------------------------------------------------
        //                    AttackMiss
        //----------------------------------------------------------------------------

        #region AttackMiss

        public IEnumerator AttackMiss(AttackRoll attackRoll)
        {
            MyTextMesh.text = "The attack misses with a " + attackRoll.PureRoll + " + " + 
                               MyAttackData.SumOfModifiers +  ", glancing off " + MyAttackData.MyIsVictim.Name
                               + "'s shield.";

            while (!Input.GetMouseButton(0))
            {
                yield return null;
            }

            yield return StartCoroutine(HideCanvas());
            yield return new WaitForSeconds(0.2f);
        }

        #endregion





        //----------------------------------------------------------------------------
        //                    StartTurn
        //----------------------------------------------------------------------------

        #region StartTurn

        public IEnumerator StartTurn(IHasTurn hasTurn)
        {
            yield return new WaitForSeconds(1);

            MyTextMesh.text = hasTurn.Name + " is up next, rolling a " + hasTurn.Initiative.PureRollValue + " + "
                                + hasTurn.Initiative.Mods + ".";

            yield return StartCoroutine(ShowCanvas());

            while (!Input.GetMouseButton(0))
            {
                yield return null;
            }

            yield return StartCoroutine(HideCanvas());
            yield return new WaitForSeconds(0.2f);
        }

        #endregion




        //----------------------------------------------------------------------------
        //                    Show and Hide Canvas
        //----------------------------------------------------------------------------

        #region Show and Hide canvas

        public IEnumerator ShowCanvas()
        {
            MyCanvas.enabled = true;
            while (MyCanvasGroup.alpha < 1)
            {
                MyCanvasGroup.alpha += Time.deltaTime * 6;
                yield return null;
            }
        }

        public IEnumerator HideCanvas()
        {
            while (MyCanvasGroup.alpha > 0)
            {
                MyCanvasGroup.alpha -= Time.deltaTime * 6;
                yield return null;
            }
            MyCanvas.enabled = true;
        }
        #endregion

    }

}
