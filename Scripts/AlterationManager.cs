using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriticalRole.Dependancy;
using CriticalRole.Attacking;
using CriticalRole.Damage;
using CriticalRole.Move;
using CriticalRole.Turns;
using CriticalRole.Armored;
using CriticalRole.Health;
using CriticalRole.Rolling;

namespace CriticalRole.Alterations
{
    public interface IAlterationManager
    {
        void Initialise();

        IHasSpeedAlterer MyHasSpeedAlterer { get; }
        IAttackDataAlterer MyAttackDataAlterer { get; }
        IDamageDataAlterer MyDamageDataAlterer { get; }
        IACAlterer MyACAlterer { get; }
        IHealthAlterer MyHealthAlterer { get; }
        IGeneralRoller MyGeneralRoller { get; }
    }

    public class AlterationManager : MonoBehaviour, IAlterationManager, IDependantManager
    {
        public DependantManagerType MyDependantManagerType
        {
            get
            {
                return DependantManagerType.AlterationManager;
            }
        }

        public void Awake()
        {
            IBattleDependancyManager[] battleDependancyManagers = FindObjectsOfType<BattleDependancyManager>();
            foreach (IBattleDependancyManager battleDependancyManager in battleDependancyManagers)
            {
                battleDependancyManager.Register(this);
            }
        }

        public void Initialise()
        {
            MyAttackDataAlterer = new AttackDataAlterer();

            MyDamageDataAlterer = new DamageDataAlterer();

            MyHasSpeedAlterer = new HasSpeedAlterer();
            MyHasSpeedAlterer.Initialise();

            MyACAlterer = new ACAlterer();
            MyACAlterer.Initialise();

            MyHealthAlterer = new HealthAlterer();
            MyHealthAlterer.Initialise();

            MyGeneralRoller = new GeneralRoller();
            MyGeneralRoller.Initialise();

            _DependancyInjectionToAttackManager();
            _DependancyInjectionHasAttack();
        }

        private void _DependancyInjectionToAttackManager()
        {
            AttackManagerMarker[] attackManagerMarkers = FindObjectsOfType<AttackManagerMarker>();
            foreach (AttackManagerMarker attackManagerMarker in attackManagerMarkers)
            {
                attackManagerMarker.gameObject.GetComponent<IAttackManager>().RegisterAlterationManager(this);
            }
        }

        private void _DependancyInjectionHasAttack()
        {
            HasTurnMarker[] hasTurnMarkers = GameObject.FindObjectsOfType<HasTurnMarker>();
            foreach (HasTurnMarker hasTurnMarker in hasTurnMarkers)
            {
                hasTurnMarker.gameObject.GetComponent<IHasAttack>().RegisterAlterationManager(this);
            }
        }

        public IAttackDataAlterer MyAttackDataAlterer { get; set; }
        public IDamageDataAlterer MyDamageDataAlterer { get; set; }
        public IHasSpeedAlterer MyHasSpeedAlterer { get; set; }
        public IACAlterer MyACAlterer { get; set; }
        public IHealthAlterer MyHealthAlterer { get; set; }
        public IGeneralRoller MyGeneralRoller { get; set; }

       
    }

}

