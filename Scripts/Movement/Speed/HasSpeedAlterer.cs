using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CriticalRole.Move
{
    public interface IHasSpeedAlterer
    {
        /// <summary>
        /// Dependancy injection to HasSpeeds
        /// </summary>
        void Initialise();

        void AddAlteration(IHasSpeed hasSpeed, ISpeedAlteration speedAlteration);

        void RemoveAlteration(IHasSpeed hasSpeed, ISpeedAlteration speedAlteration);

        int GetTotalMoveSpeed(IHasSpeed hasSpeed);
    }

    public class HasSpeedAlterer : IHasSpeedAlterer
    {
        public void Initialise()
        {
            IHasSpeed[] hasSpeeds = GameObject.FindObjectsOfType<HasSpeed>();
            foreach (IHasSpeed hasSpeed in hasSpeeds)
            {
                hasSpeed.RegisterHasSpeedAlterer(this);
            }
        }

        private Dictionary<IHasSpeed, List<ISpeedAlteration>> _MySpeedAlterationsDict;
        public Dictionary<IHasSpeed, List<ISpeedAlteration>> MySpeedAlterationsDict
        {
            get
            {
                if(_MySpeedAlterationsDict == null)
                {
                    _MySpeedAlterationsDict = new Dictionary<IHasSpeed, List<ISpeedAlteration>>();
                }
                return _MySpeedAlterationsDict;
            }
        }

        public int GetTotalMoveSpeed(IHasSpeed hasSpeed)
        {
            if(!MySpeedAlterationsDict.ContainsKey(hasSpeed))
            {
                Debug.LogError("HasSpeed did not register in the dictionary.");
            }

            List<ISpeedAlteration> speedAlterations = MySpeedAlterationsDict[hasSpeed];

            speedAlterations.Sort(new SpeedAlterationSort());
            float speed = 0;
            foreach(ISpeedAlteration speedAlteration in speedAlterations)
            {
                speed = speedAlteration.Alter(speed);
            }
            return Mathf.FloorToInt(speed);
        }

        public void AddAlteration(IHasSpeed hasSpeed, ISpeedAlteration speedAlteration)
        {
            if(!MySpeedAlterationsDict.ContainsKey(hasSpeed))
            {
                MySpeedAlterationsDict[hasSpeed] = new List<ISpeedAlteration>();
            }
            MySpeedAlterationsDict[hasSpeed].Add(speedAlteration);
        }

        public void RemoveAlteration(IHasSpeed hasSpeed, ISpeedAlteration speedAlteration)
        {
            if (!MySpeedAlterationsDict.ContainsKey(hasSpeed))
            {
                Debug.LogError("Attempting to remove speed alteration from a HasSpeed that is not registered.");
            }

            MySpeedAlterationsDict[hasSpeed].Remove(speedAlteration);
        }
    }

}

