using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CriticalRole.Turns
{
    public enum ActionEnum
    {
        HasAction,
        NoAction
    }

    public enum BonusActionEnum
    {
        HasBonusAction,
        NoBonusAction
    }

    public enum ReactionEnum
    {
        HasReaction,
        NoReaction
    }

    public interface ITurnStarter
    {

    }

    public class TurnStarter : MonoBehaviour, ITurnStarter
    {
        ActionEnum MyActionEnum;
        BonusActionEnum MyBonusActionEnum;
        ReactionEnum MyReactionEnum;

        public void StartTurn(IHasTurn hasTurn)
        {
            MyActionEnum = ActionEnum.HasAction;
            MyBonusActionEnum = BonusActionEnum.HasBonusAction;
            MyReactionEnum = ReactionEnum.HasReaction;


            hasTurn.StartTurn(MyActionEnum, MyBonusActionEnum, MyReactionEnum);
            
        }

        public Dictionary<IHasTurn, IActionAlteration> _MyActionAlterations;
        public Dictionary<IHasTurn, IActionAlteration> MyActionAlterations
        {
            get
            {
                if(_MyActionAlterations == null)
                {
                    _MyActionAlterations = new Dictionary<IHasTurn, IActionAlteration>();
                }
                return _MyActionAlterations;
            }
        }
    }

        
        
        

    
}

