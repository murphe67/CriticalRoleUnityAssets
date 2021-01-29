using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriticalRole.Map;
using CriticalRole.Turns;

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

        /// <summary>
        /// Stop any conflicts if somehow a rotation starts before the previous one finished. <para/>
        /// There should only be one, stored in this. <para/>
        /// A rotating coroutine should store themselves in this.
        /// </summary>
        public Coroutine RotatingCoroutine;

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

            MyHasTurn.MyHasSpeed.UseMove(NextHex.MyHexMap.MovementCost);

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
            LookAtHexMove();

            while (DistanceFromDestination > 0.1)
            {
                ContentTransform.position = Vector3.MoveTowards(ContentTransform.position,
                                                                NextHex.HexTransform.position,
                                                             Time.deltaTime * PhysicalMoveSpeed);
                yield return null;
            }
        }

        #region Implementation

        //How far Contents should move per second
        public readonly float PhysicalMoveSpeed = 2.5f;

        public float DistanceFromDestination
        {
            get
            {
                return Vector3.Distance(MyHasTurn.MyHexContents.ContentTransform.position, NextHex.HexTransform.position);
            }
        }

        #endregion

        #endregion





        //----------------------------------------------------------------------------
        //              LookAtHexMove
        //--------------------------------------------------------------------------

        #region LookAtHexMove

        // Straight lines in hex maps are sometimes c shaped 
        // Rotating twice to walk in a straight line looks dumb, 
        // so in order to look in the direction you're walking 
        // You need to check if the next current hex, next hex, and 3rd hex form a straight line

        // if they don't, rotate to face NextHeex

        public void LookAtHexMove()
        {
            if(RotatingCoroutine != null)
            {
                StopCoroutine(RotatingCoroutine);
            }

            _UpdateMoveCheckPositions();
            _UpdateMoveLookedAtTransform();

            UpdateLookingAtTransform();

            RotatingCoroutine = StartCoroutine(RotateToLookingAtTransform(MoveRotationSpeed));
        }



        #region Implementation

        private readonly int MoveRotationSpeed = 200;

        private Vector3 _EndCheckPosition;
        private Vector3 _StartCheckPosition;

        /// <summary>
        /// A 'straight' c shape move has a MoveAngle of between 28 and 32 degrees <para/>
        /// For a 'straight' move, you should look at your final destination, 2 hexes away. <para/>
        /// Otherwise, you should look at the next hex.
        /// </summary>
        private void _UpdateMoveLookedAtTransform()
        {
            if (_MoveAngle > 45)
            {
                LookedAtTransform.position = NextHex.HexTransform.position;
            }
            else
            {
                LookedAtTransform.position = ContentTransform.position + _CheckDirection;
            }
        }

        #region _MoveAngle

        /// <summary>
        /// What is angle between <para/>
        /// your current position and your position in 2 steps?
        /// </summary>
        private float _MoveAngle
        {
            get
            {
                return Vector3.Angle(ContentTransform.forward, _CheckDirection);
            }
        }

        /// <summary>
        /// The direction vector between EndCheckPosition and StartCheckPosition
        /// </summary>
        private Vector3 _CheckDirection
        {
            get
            {
                return _EndCheckPosition - _StartCheckPosition;
            }
        }

        /// <summary>
        /// If there are 2 hexes left, <para/>
        /// Set EndCheckPosition to 2 hexes away, and StartCheckPosition to the current position. <para/>
        /// If there is only one hex left, the current EndCheckPosition and StartCheckPosition are still valid <para/>
        /// and are not changed.
        /// </summary>
        private void _UpdateMoveCheckPositions()
        {
            if (PathLength > 0)
            {
                _EndCheckPosition = Path.PathStack.Peek().HexTransform.position;
                _StartCheckPosition = ContentTransform.position;
            }
        }

        #endregion

        #endregion

        #endregion




        //----------------------------------------------------------------------------
        //              LookAtHexHover
        //--------------------------------------------------------------------------

        #region LookAtHexHover

        public void LookAtHexHover(IHexagon ihexagon, Transform contentTransform)
        {
            if(RotatingCoroutine != null)
            {
                StopCoroutine(RotatingCoroutine);
            }

            ContentTransform = contentTransform;
            LookedAtTransform.position = ihexagon.HexTransform.position;
            RotatingCoroutine = StartCoroutine(LookAtHexHoverCoroutine());
        }

        public IEnumerator LookAtHexHoverCoroutine()
        {
            UpdateLookingAtTransform();

            //delay in coroutine prevents crazy spinning if mouse is moving very fast
            yield return new WaitForSeconds(0.05f);

            RotatingCoroutine = StartCoroutine(RotateToLookingAtTransform(HoverRotationSpeed));
        }

        private readonly int HoverRotationSpeed = 800;


        #endregion




        //----------------------------------------------------------------------------
        //              LookingAtTransform
        //----------------------------------------------------------------------------

        #region RotateToLookingAtTransform

        public IEnumerator RotateToLookingAtTransform(int rotationSpeed)
        {
            while (_AngleToLookingAtTransform > 0.01)
            {
                ContentTransform.rotation = Quaternion.RotateTowards(ContentTransform.rotation,
                                                                     LookingAtTransform.rotation,
                                                                     Time.deltaTime * rotationSpeed);
                yield return null;
            }
        }

        /// <summary>
        /// The angle between where the character is currently looking <para/>
        /// Vs where it should be looking
        /// </summary>
        private float _AngleToLookingAtTransform
        {
            get
            {
                return Quaternion.Angle(ContentTransform.rotation, LookingAtTransform.rotation);
            }
        }


        public void UpdateLookingAtTransform()
        {
            LookingAtTransform.position = ContentTransform.position;
            LookingAtTransform.LookAt(LookedAtTransform);
        }

        #endregion


    }

}
