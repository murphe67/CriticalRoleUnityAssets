using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------
//              Class Description
//----------------------------------------------------------------------------
//
// IHasSpeed interface is for Movement and for adding effects


/// <summary>
/// Moveable character's current movement speed in tiles <para/>
/// Grappling/Spells add a SpeedAlteration to influence this
/// </summary>
public interface IHasSpeed
{
    /// <summary>
    /// Get the final result of this character's movement speed <para/>
    /// After all effects take place
    /// </summary>
    int CurrentMovement();

    void AddAlteration(ISpeedAlteration speedAlteration);

    void UseMove(int movementCost);

    void RefreshMovement();
}

public class HasSpeed : MonoBehaviour, IHasSpeed
{

    //----------------------------------------------------------------------------
    //             Initialise
    //----------------------------------------------------------------------------

    #region Initialise

    private void Awake()
    {
        Initialise();
    }

    /// <summary>
    /// Initialise the SpeedAlterations list <para/>
    /// Also creates and adds BaseSpeed to the alterations <para/>
    /// Eventually this will be called by the character initialise after the racial traits set BaseSpeedInt
    /// </summary>
    public void Initialise()
    {
        MyBaseSpeed = new BaseSpeed(BaseSpeedInt);
        SpeedAlterationSorts = new List<SpeedAlterationSort>();

        SpeedAlterationSorts.Add(MyBaseSpeed.MySpeedAlterationSort);
    }

    /// <summary>
    /// List of all effects currently influencing movement speed <para/>
    /// Sorted by the order effects should be applied in
    /// </summary>
    List<SpeedAlterationSort> SpeedAlterationSorts;

    /// <summary>
    /// This is the variable that will be edited by the character race <para/>
    /// Temporarily readonly to stop inspector override
    /// </summary>
    public readonly int BaseSpeedInt = 7;

    /// <summary>
    /// SpeedAlteration that is called first by the SpeedAlterer <para/>
    /// Sets the base value of the movement speed
    /// </summary>
    public BaseSpeed MyBaseSpeed { get; set; }

    #endregion

    //----------------------------------------------------------------------------
    //            FinalSpeed
    //----------------------------------------------------------------------------

    /// <summary>
    /// Get the final result of this character's movement speed <para/>
    /// after all effects take place <para/>
    /// (Hopefully) applies all effects reversibly and in correct order <para/>
    /// Is the amount of moving left, rather than the final total movement
    /// </summary>
    public int CurrentMovement()
    {
        return TotalMovement() - MovementUsed;
    }

    public int MovementUsed;

    public int TotalMovement()
    {
        SpeedAlterationSorts.Sort();
        int speed = 0;
        foreach (SpeedAlterationSort speedAlterationSort in SpeedAlterationSorts)
        {
            speed = speedAlterationSort.SpeedAlteration.Alter(speed);
        }
        return speed;
    }

    public void UseMove(int movementCost)
    {
        MovementUsed += movementCost;
    }

    public void RefreshMovement()
    {
        MovementUsed = 0;
    }

    //----------------------------------------------------------------------------
    //            AddAlteration
    //----------------------------------------------------------------------------

    /// <summary>
    /// This function just anonymises the underlying dictionary <para/>
    /// No extra logic
    /// </summary>
    public void AddAlteration(ISpeedAlteration speedAlteration)
    {
        SpeedAlterationSorts.Add(speedAlteration.MySpeedAlterationSort);
    }

    /// <summary>
    /// This function just anonymises the underlying dictionary <para/>
    /// No extra logic
    /// </summary>
    public void RemoveAlteration(ISpeedAlteration speedAlteration)
    {
        SpeedAlterationSorts.Remove(speedAlteration.MySpeedAlterationSort);
    }

}
