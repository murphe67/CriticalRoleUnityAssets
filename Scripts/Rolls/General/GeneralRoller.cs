using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriticalRole.Turns;

namespace CriticalRole.Rolling
{
    public enum RollType
    {
        Initiative,
        Strength,
        Dexterity,
        Constitution,
        Intelligence,
        Wisdom,
        Charisma,
        StrSave,
        DexSave,
        ConSave,
        IntSave,
        WisSave,
        ChaSave,
        ConCheck,
        Athletics,
        Acrobatics,
        SleightOfHand,
        Stealth,
        Arcana,
        History,
        Investigation,
        Nature,
        Religion,
        AnimalHandling,
        Insight,
        Medicine,
        Perception,
        Survival,
        Deception,
        Intimidation,
        Performance,
        Persuasion
    }

    public interface IGeneralRoller
    {
        void Roll(RollType rollType, IHasTurn hasTurn, IRollData rollData);
        void Initialise();

        void AddAlteration(RollType rollType, IHasTurn hasTurn, IRollDataAlteration alteration);
    }

    public class GeneralRoller : IGeneralRoller
    {
        

        public void Initialise()
        {
            MyAdvantageRoller = new AdvantageRoller();

            HasTurnMarker[] hasTurnMarkers = GameObject.FindObjectsOfType<HasTurnMarker>();
            foreach(HasTurnMarker hasTurnMarker in hasTurnMarkers)
            {
                hasTurnMarker.gameObject.GetComponent<IHasTurn>().RegisterGeneralRoller(this);
            }
        }

        AdvantageRoller MyAdvantageRoller;

        public Dictionary<RollDictID, List<IRollDataAlteration>> _RollAlterationDict;
        public Dictionary<RollDictID, List<IRollDataAlteration>> RollAlterationDict
        {
            get
            {
                if (_RollAlterationDict == null)
                {
                    _RollAlterationDict = new Dictionary<RollDictID, List<IRollDataAlteration>>(new RollDictIDComparer());
                }
                return _RollAlterationDict;
            }
        }

        public void Roll(RollType rollType, IHasTurn hasTurn, IRollData rollData)
        {
            RollDictID rollDictID = new RollDictID(hasTurn, rollType);
            if(RollAlterationDict.ContainsKey(rollDictID))
            {
                foreach (IRollDataAlteration rollDataAlteration in RollAlterationDict[rollDictID])
                {
                    rollDataAlteration.Alter(rollData);
                }
            }

            int rollInt = MyAdvantageRoller.Roll(rollData.MyAdvantageSetter);
            rollData.SetRoll(rollInt);
        }

        public void AddAlteration(RollType rollType, IHasTurn hasTurn, IRollDataAlteration alteration)
        {
            RollDictID rollDictID = new RollDictID(hasTurn, rollType);
            if(!RollAlterationDict.ContainsKey(rollDictID))
            {
                RollAlterationDict[rollDictID] = new List<IRollDataAlteration>();
            }

            RollAlterationDict[rollDictID].Add(alteration);
        }

    }
}


