using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CriticalRole.Move
{

    //----------------------------------------------------------------------------
    //              Class Description
    //----------------------------------------------------------------------------
    //
    // Here's the script that actually moves things once they know where they're going
    //
    // For now, theres no way for things to go wrong- once you've decided where you're going
    // You're going there
    //
    // Later on interruptions like tripping or opportunity attacks will probably be implemented here
    // So possibly this will also update the movement left of a character? so if it is interrupted
    // its at the right value

    public interface IMovement
    {
        /// <summary>
        /// IMovement calls on the IMovement controller once movement has finished <para />
        /// This relationship is established here
        /// </summary>
        void Initialise(IMovementController movementController);

        /// <summary>
        /// Move this IHexContent to the end of this path
        /// </summary>
        void DoMove(IHasTurn hasTurn, MapPath path);

        void LookAtHexHover(IHexagon ihexagon, Transform contentTransform);

    }

    public class Movement : MonoBehaviour, IMovement
    {
        //----------------------------------------------------------------------------
        //              Initialise
        //----------------------------------------------------------------------------

        #region Initialise
        /// <summary>
        /// IMovement calls on the IMovement controller once movement has finished <para />
        /// This relationship is established here
        /// </summary>
        public void Initialise(IMovementController movementController)
        {
            MyMovementController = movementController;
            LookingAtTransform = new GameObject("Movement LookingAt").transform;
            LookingAtTransform.parent = transform;

            LookedAtTransform = new GameObject("Movement LookedAt").transform;
            LookedAtTransform.parent = transform;

        }

        public IMovementController MyMovementController;

        public Transform LookingAtTransform;
        public Transform LookedAtTransform;


        #endregion


        //----------------------------------------------------------------------------
        //              DoMove
        //----------------------------------------------------------------------------

        #region DoMove
        /// <summary>
        /// Called by a movement controller to move the IHasTurn to the end of path
        /// </summary>
        public void DoMove(IHasTurn hasTurn, MapPath path)
        {
            MyHasTurn = hasTurn;
            ContentTransform = MyHasTurn.MyHexContents.ContentTransform;
            Path = path;
            StopAllCoroutines();
            Debug.Log("move");

            RemoveStartLocationFromPath();

            StartCoroutine(MoveToDestination());
        }

        /// <summary>
        /// The aIHasTurn being moved to a new hex
        /// </summary>
        IHasTurn MyHasTurn;

        Transform ContentTransform;

        /// <summary>
        /// The path to move Contents to the end of
        /// </summary>
        MapPath Path;

        public void RemoveStartLocationFromPath()
        {
            Path.PathStack.Pop();
        }

        #endregion

        //----------------------------------------------------------------------------
        //              MoveToDestination
        //----------------------------------------------------------------------------

        #region MoveToDestination
        /// <summary>
        /// The forking function- <para/>
        /// If the end of the path has not been reached, move a single hex forward <para/>
        /// Otherwise, end the movement <para/>
        /// Any kind of effect a hex will have on a hex content will probably be applied here
        /// </summary>
        public IEnumerator MoveToDestination()
        {
            while (PathLength > 0)
            {
                yield return StartCoroutine(StepDownPath());
            }

            MyMovementController.EndMove();
        }

        public int PathLength
        {
            get
            {
                return Path.PathStack.Count;
            }
        }
        #endregion

        //----------------------------------------------------------------------------
        //              StepDownpath
        //----------------------------------------------------------------------------

        #region StepDownPath

        public IEnumerator StepDownPath()
        {
            NextHex = GetNextPathHex();

            MoveInReferences();

            MyHasTurn.MyHasSpeed.UseMove(NextHex.MovementCost);

            yield return StartCoroutine(MoveTransform());
        }

        IHexagon NextHex;

        public IHexagon GetNextPathHex()
        {
            return Path.PathStack.Pop();
        }

        /// <summary>
        /// A IHexagon has a reference to its contents, and contents to which hex it is in. <para/>
        /// The IHasTurn must be moved in these references as well as in space.
        /// </summary>
        public void MoveInReferences()
        {
            //update the old hex
            MyHasTurn.MyHexContents.Location.Contents = null;

            //update MyHasTurn
            MyHasTurn.MyHexContents.Location = NextHex;

            //updat NextHex
            NextHex.Contents = MyHasTurn.MyHexContents;
        }

        #endregion

        //----------------------------------------------------------------------------
        //              MoveTransform
        //----------------------------------------------------------------------------

        #region MoveTransform

        /// <summary>
        /// Physically move Contents to NextHexTransform <para/>d
        /// </summary>
        public IEnumerator MoveTransform()
        {
            StartCoroutine(LookAtHexMove());

            while (DistanceFromDestination > 0.1)
            {
                ContentTransform.position = Vector3.MoveTowards(ContentTransform.position,
                                                                NextHex.HexTransform.position,
                                                             Time.deltaTime * PhysicalMoveSpeed);
                yield return null;
            }
        }

        //How far Contents should move per second
        public readonly float PhysicalMoveSpeed = 2.5f;

        public float DistanceFromDestination
        {
            get
            {
                return Vector3.Distance(MyHasTurn.MyHexContents.ContentTransform.position, NextHex.HexTransform.position);
            }
        }

        public IEnumerator LookAtHexMove()
        {
            if(PathLength > 0)
            {
                EndCheckPosition = Path.PathStack.Peek().HexTransform.position;
                StartCheckPosition = ContentTransform.position;
            }

            if (MoveAngle > 45)
            {
                LookedAtTransform.position = NextHex.HexTransform.position;
            }
            else
            {
                LookedAtTransform.position = ContentTransform.position + CheckDirection;
            }

            LookingAtTransform.position = ContentTransform.position;
            LookingAtTransform.LookAt(LookedAtTransform);

            while (Quaternion.Angle(ContentTransform.rotation, LookingAtTransform.rotation) > 0.01)
            {
                ContentTransform.rotation = Quaternion.RotateTowards(ContentTransform.rotation, LookingAtTransform.rotation, Time.deltaTime * 200);
                yield return null;
            }

        }

        Vector3 EndCheckPosition;
        Vector3 StartCheckPosition;

        Vector3 CheckDirection
        {
            get
            {
                return EndCheckPosition - StartCheckPosition;
            }
        }

        public float MoveAngle
        {
            get
            {
                return Vector3.Angle(ContentTransform.forward, CheckDirection);
            }
        }


        public void LookAtHexHover(IHexagon ihexagon, Transform contentTransform)
        {
            StopAllCoroutines();
            StartCoroutine(LookAtHexHoverCoroutine(ihexagon, contentTransform));
        }

        public IEnumerator LookAtHexHoverCoroutine(IHexagon hoverHex, Transform hoverTransform)
        {
            LookingAtTransform.position = hoverTransform.position;
            LookingAtTransform.LookAt(hoverHex.HexTransform);

            yield return new WaitForSeconds(0.05f);

            while (LookingAtTransform.transform.rotation != hoverTransform.rotation)
            {
                hoverTransform.rotation = Quaternion.RotateTowards(hoverTransform.rotation, LookingAtTransform.rotation, Time.deltaTime * 800);
                yield return null;
            }
        }

        #endregion
    }

}
