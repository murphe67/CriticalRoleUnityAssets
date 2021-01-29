using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriticalRole.Attacking;
using CriticalRole.Turns;
using CriticalRole.BattleCamera;

namespace CriticalRole.Death
{
    public interface IDeathManager
    {
        IEnumerator Die(IHasTurn hasTurn);

        void AddSingleDeathEvent(IHasTurn hasTurn, IDeathEvent deathEvent);
        void AddGlobalDeathEvent(IDeathEvent deathEvent);

        void RegisterAttackManager(IAttackManager attackManager);
        void RegisterBattleCamManager(IBattleCamManager battleCamManager);
    }

    [RequireComponent(typeof(DeathManagerMarker))]
    public class DeathManager : MonoBehaviour, IDeathManager, IEndAttackEvent
    {
        public void Awake()
        {
            HasTurnMarker[] hasTurnMarkers = FindObjectsOfType<HasTurnMarker>();
            foreach(HasTurnMarker hasTurnMarker in hasTurnMarkers)
            {
                hasTurnMarker.gameObject.GetComponent<ICanDie>().RegisterDeathManager(this);
            }
        }

        public void RegisterAttackManager(IAttackManager attackManager)
        {
            MyAttackManager = attackManager;
        }

        public IAttackManager MyAttackManager;

        public void RegisterBattleCamManager(IBattleCamManager battleCamManager)
        {
            MyBattleCamManager = battleCamManager;
        }

        public IBattleCamManager MyBattleCamManager;


        //----------------------------------------------------------------------------
        //             AddSingleDeathEvent
        //----------------------------------------------------------------------------

        #region AddSingleDeathEvent

        public void AddSingleDeathEvent(IHasTurn hasTurn, IDeathEvent deathEvent)
        {
            if(!DeathEventDict.ContainsKey(hasTurn))
            {
                DeathEventDict[hasTurn] = new List<IDeathEvent>();
            }

            DeathEventDict[hasTurn].Add(deathEvent);
        }

        private Dictionary<IHasTurn, List<IDeathEvent>> _DeathEventDict;
        public Dictionary<IHasTurn, List<IDeathEvent>> DeathEventDict
        {
            get
            {
                if(_DeathEventDict == null)
                {
                    _DeathEventDict = new Dictionary<IHasTurn, List<IDeathEvent>>();
                }

                return _DeathEventDict;
            }
        }

        #endregion




        //----------------------------------------------------------------------------
        //             AddGlobalDeathEvent
        //----------------------------------------------------------------------------


        #region AddGlobalDeathEvent

        public void AddGlobalDeathEvent(IDeathEvent deathEvent)
        {
            GlobalDeathEvents.Add(deathEvent);
        }

        private List<IDeathEvent> _GlobalDeathEvents;
        List<IDeathEvent> GlobalDeathEvents
        {
            get
            {
                if(_GlobalDeathEvents == null)
                {
                    _GlobalDeathEvents = new List<IDeathEvent>();
                }

                return _GlobalDeathEvents;
            }
        }

        #endregion


        //----------------------------------------------------------------------------
        //             Die
        //----------------------------------------------------------------------------

        #region Die

        public IEnumerator Die(IHasTurn hasTurn)
        {
            DyingHasTurn = hasTurn;
            if(DeathEventDict.ContainsKey(hasTurn))
            {
                foreach (IDeathEvent deathEvent in DeathEventDict[hasTurn])
                {
                    yield return StartCoroutine(deathEvent.ReactToDeath(hasTurn));
                }
            }
            

            foreach(IDeathEvent globalDeathEvent in GlobalDeathEvents)
            {
                yield return StartCoroutine(globalDeathEvent.ReactToDeath(hasTurn));
            }

            MyAttackManager.AddEndAttackEvent(this);
        }

        public IHasTurn DyingHasTurn;

        #endregion




        public IEnumerator EndAttackEvent(IHasTurn hasTurn)
        {
            yield return new WaitForSeconds(1f);
            StartCoroutine(MyBattleCamManager.ChangeSwitchSpeed(0.5f));
            yield return StartCoroutine(MyBattleCamManager.FocusOnDeath(DyingHasTurn.MyHexContents.ContentTransform));
            StartCoroutine(MyBattleCamManager.ChangeSwitchSpeed(0.4f));
            yield return StartCoroutine(MyBattleCamManager.SwitchToScriptCamera());
            Destroy(DyingHasTurn.MyHexContents.ContentTransform.gameObject);
            yield return new WaitForSeconds(2.5f);
            MyAttackManager.RemoveEndAttackEvent(this);
            StartCoroutine(MyBattleCamManager.ChangeSwitchSpeed(0.6f));
            yield break;
        }

        public EndAttackEventType MyEndAttackEventType
        {
            get
            {
                return EndAttackEventType.CameraFocus;
            }
        }
        
    }

}
