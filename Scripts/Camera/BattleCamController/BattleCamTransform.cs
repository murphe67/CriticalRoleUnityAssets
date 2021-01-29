using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CriticalRole.BattleCamera
{

    //----------------------------------------------------------------------------
    //                    Class Description
    //----------------------------------------------------------------------------
    //
    // Subclass of the BattleCamController
    //
    // Provides equivalent properties and functions of Unity's Transform/Gameobject classes

    public interface IBattleCamTransform
    {
        Vector3 Position { get; }

        Vector3 Forward { get; }
        Vector3 Right { get; }

        void JumpTo(float x, float z);

        void ZoomTo(float zoom);

        void RotateTo(Quaternion newRotation);

        void RotateAround(Vector3 point, float angle);


        void Move(Vector3 displacement);


        void SetActive(bool isActive);

    }

    public class BattleCamTransform : IBattleCamTransform
    {

        public BattleCamTransform(BattleCamController battleCamController)
        {
            MyCamController = battleCamController;
        }

        BattleCamController MyCamController;



        public Vector3 Position
        {
            get
            {
                return MyCamController.gameObject.transform.position;
            }
        }

        public Vector3 Forward
        {
            get
            {
                return MyCamController.gameObject.transform.forward;
            }
        }
        public Vector3 Right
        {
            get
            {
                return MyCamController.gameObject.transform.right;
            }
        }

        //----------------------------------------------------------------------------
        //                    JumpTo
        //----------------------------------------------------------------------------

        #region JumpTo

        public void JumpTo(float x, float z)
        {
            float y = MyCamController.FocusControlled.position.y;
            MyCamController.FocusControlled.position = new Vector3(x, y, z);
            MyCamController.FocusLerped.position = new Vector3(x, y, z);
        }

        #endregion



        //----------------------------------------------------------------------------
        //                    ZoomTo
        //----------------------------------------------------------------------------

        #region ZoomTo
        public void ZoomTo(float zoom)
        {
            zoom = Mathf.Clamp(zoom, MyCamController.MinimumZoom, MyCamController.MaximumZoom);
            MyCamController.ZoomLerped = zoom;
            MyCamController.ZoomControlled = zoom;
        }
        #endregion

        //----------------------------------------------------------------------------
        //                    Rotation
        //----------------------------------------------------------------------------

        #region Rotation

        public void RotateTo(Quaternion newRotation)
        {
            MyCamController.FocusControlled.rotation = newRotation;
            MyCamController.FocusLerped.rotation = newRotation;
        }

        public void RotateAround(Vector3 point, float angle)
        {
            MyCamController.FocusControlled.RotateAround(point, Vector3.up, angle);
            MyCamController.FocusLerped.RotateAround(point, Vector3.up, angle);
        }

        #endregion

        //----------------------------------------------------------------------------
        //                    Move
        //----------------------------------------------------------------------------

        #region Move

        public void Move(Vector3 displacement)
        {
            MyCamController.FocusControlled.position += displacement;
            MyCamController.FocusLerped.position += displacement;
        }

        #endregion

        //----------------------------------------------------------------------------
        //                    SetActive
        //----------------------------------------------------------------------------

        #region SetActive
        public void SetActive(bool isActive)
        {
            MyCamController.gameObject.SetActive(isActive);
        }
        #endregion
    }
}
