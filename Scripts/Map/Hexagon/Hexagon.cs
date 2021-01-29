using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CriticalRole.Contents;


namespace CriticalRole.Map
{

    //----------------------------------------------------------------------------
    //                    Class Description
    //----------------------------------------------------------------------------
    //
    // This class forms the tile system the map is based off
    // 
    // Is currently 2D, but eventually should be extended to 3D
    //
    // It is the main access point for anything map related, including any of the
    // pathfinding functions or location.
    //
    // Different interfaces depending on what access is needed
    //
    // Due to the size of the class, the way the IHexagon interacts with the UI_Input
    // is in a different class HexInteract




    public interface IHexagon
    {
        IContents Contents { get; set; }

        IHexMap MyHexMap { get; }
        /// <summary>
        /// Functions related to the hexagon reacting to actions are in Interaction
        /// </summary>
        IHexInteract Interaction { get; }
        /// <summary>
        /// The hexagon's transform <para/>
        /// Used by Movement to physically move a mesh onto the hex
        /// </summary>
        Transform HexTransform { get; }

    }

    /// <summary>
    /// This interface is only for MapGeneration
    /// Allows the neighbours to be edited
    /// </summary>
    public interface IHexagonRestricted : IHexagon
    {
        IHexMapRestricted MyHexMapRestricted { get; }

        IHexKeep MyHexKeep { get; }
    }









    [RequireComponent(typeof(IHexagonMarker))]
    public class Hexagon : MonoBehaviour, IHexagonRestricted
    {
        //----------------------------------------------------------------------------
        //                    Subclasses
        //----------------------------------------------------------------------------

        #region Subclasses

        public IHexMap MyHexMap { get; set; }
        public IHexMapRestricted MyHexMapRestricted { get; set; }
        public IHexKeep MyHexKeep { get; set; }

        public IHexInteract Interaction { get; set; }


        #endregion





        //----------------------------------------------------------------------------
        //                    Initialisation
        //----------------------------------------------------------------------------


        #region Initialisation
        /// <summary>
        /// Initialise variables, and verify coords
        /// </summary>
        public void Initialise(Vector3Int coords, int movementCost)
        {
            Debug.Assert((coords.x + coords.y + coords.z) == 0);

            MyHexMap = new HexMap(this);
            MyHexMapRestricted = (IHexMapRestricted)MyHexMap;

            MyHexMapRestricted.SetCoords(coords);
            MyHexMapRestricted.SetMovementCost(1);

            MyHexKeep = gameObject.AddComponent<HexKeep>();

            Interaction = gameObject.AddComponent<HexInteract>();
            Interaction.hex = this;
        }

        #endregion


        //----------------------------------------------------------------------------
        //                    Content
        //--------------------------------------------------------------------------

        public IContents Contents { get; set; }




        //----------------------------------------------------------------------------
        //                    Movement
        //--------------------------------------------------------------------------

        public Transform HexTransform
        {
            get
            {
                return gameObject.transform;
            }
        }





    }
}
