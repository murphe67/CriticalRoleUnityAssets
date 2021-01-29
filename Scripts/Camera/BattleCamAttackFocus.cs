using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CriticalRole.Contents;
using CriticalRole.Turns;

namespace CriticalRole.BattleCamera
{

    //----------------------------------------------------------------------------
    //              Class Description
    //----------------------------------------------------------------------------
    //
    // Subclass for BattleCamManager
    //
    // Provides interesting camera angles to a fight
    //
    // currently has 3 basic animations, 2 play for each fight

    public class BattleCamAttackFocus : MonoBehaviour
    {

        //----------------------------------------------------------------------------
        //             Initialise
        //----------------------------------------------------------------------------

        #region Initialise

        public void Initialise(BattleCamManager battleCamManager)
        {
            MyBattleCamManager = battleCamManager;
            ScriptCam = MyBattleCamManager.ScriptCamController;
            ScriptCamInput = MyBattleCamManager.ScriptCamInput;
        }

        BattleCamManager MyBattleCamManager;
        IBattleCamController ScriptCam;
        IBattleCamScriptInput ScriptCamInput;


        #endregion






        //----------------------------------------------------------------------------
        //             FocusOnAttack
        //----------------------------------------------------------------------------

        #region FocusOnAttack
        public IEnumerator FocusOnAttack(Transform attackerTransform, Transform victimTransform)
        {
            Vector3 attackPosition = attackerTransform.position;
            Vector3 victimPosition = victimTransform.position;
            _MiddlePosition = (attackPosition + victimPosition) / 2f;

            _AttackerTransform = attackerTransform;
            _VictimTransform = victimTransform;

            while(true)
            {
                List<int> focusIndex = Enumerable.Range(0, _NoFocuses).OrderBy(i => Random.value).ToList();
                foreach(int index in focusIndex)
                {
                    yield return StartCoroutine(ChooseFocus(index));
                    yield return StartCoroutine(MyBattleCamManager.SwitchToScriptCamera());
                }
            }

        }



        #region Implementation

        /// <summary>
        /// Position in between victim and attacker
        /// </summary>
        Vector3 _MiddlePosition;
        Transform _AttackerTransform;
        Transform _VictimTransform;

        private readonly int _NoFocuses = 3;

        #endregion


        #endregion





        //----------------------------------------------------------------------------
        //             ChooseFocus
        //----------------------------------------------------------------------------

        #region ChooseFocus

        public IEnumerator ChooseFocus(int index)
        {
            if(index == _PrevIndex)
            {
                index = ((index + 1) % _NoFocuses);
            }
            _PrevIndex = index;

            switch(index)
            {
                case 0:
                    yield return StartCoroutine(_PanOverBoth());
                    break;
                case 1:
                    yield return StartCoroutine(_RotateAroundBoth());
                    break;
                case 2:
                    yield return StartCoroutine(_LookAtAttacker());
                    break;
            }
        }

        private int _PrevIndex = 100;

        #endregion





        //----------------------------------------------------------------------------
        //             Focuses
        //----------------------------------------------------------------------------

        #region Focuses

        #region PanOverBoth
        private IEnumerator _PanOverBoth()
        {
            ScriptCam.MyBattleCamTransform.JumpTo(_MiddlePosition.x, _MiddlePosition.z);
            ScriptCam.MyBattleCamTransform.RotateTo(_VictimTransform.rotation);
            ScriptCam.MyBattleCamTransform.RotateAround(_MiddlePosition, 90);
            ScriptCam.MyBattleCamTransform.ZoomTo(1.5f);


            Vector3 displacement = _VictimTransform.forward * 0.4f;
            ScriptCam.MyBattleCamTransform.Move(displacement);

            float PanSpeed = 0.02f;
            ScriptCamInput.SetInputs(0, 0, PanSpeed, 0);

            yield return new WaitForSeconds(5f);
        }

        #endregion

        #region RotateAroundBoth

        private IEnumerator _RotateAroundBoth()
        {
            ScriptCam.MyBattleCamTransform.JumpTo(_MiddlePosition.x, _MiddlePosition.z);
            ScriptCam.MyBattleCamTransform.RotateTo(_VictimTransform.rotation);
            ScriptCam.MyBattleCamTransform.RotateAround(_MiddlePosition, 280);
            ScriptCam.MyBattleCamTransform.ZoomTo(6);

            float RotateSpeed = 0.05f;
            ScriptCamInput.SetInputs(RotateSpeed, 0, 0, 0);

            yield return new WaitForSeconds(5f);
        }

        #endregion


        #region LookAtAttacker
        private IEnumerator _LookAtAttacker()
        {
            ScriptCam.MyBattleCamTransform.JumpTo(_MiddlePosition.x, _MiddlePosition.z);
            ScriptCam.MyBattleCamTransform.RotateTo(_VictimTransform.rotation);
            ScriptCam.MyBattleCamTransform.RotateAround(_MiddlePosition, 15);
            ScriptCam.MyBattleCamTransform.ZoomTo(1);

            ScriptCamInput.SetInputs(0.02f, 0, 0, 0);
            yield return new WaitForSeconds(5f);
        }

        #endregion

        #endregion
    
    
    }

}
