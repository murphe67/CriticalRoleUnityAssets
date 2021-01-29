using CriticalRole.Contents;
using CriticalRole.Attacking;
using CriticalRole.Move;
using CriticalRole.Character;
using CriticalRole.Rolling;

namespace CriticalRole.Turns
{

    public interface IHasTurn
    {
        void Initialise(ITurnController turnController);

        int Index { get; set; }

        string Name { get; }

        IContents MyHexContents { get; }

        IHasSpeed MyHasSpeed { get; }

        IHasAttack MyHasAttack { get; }

        IIsVictim MyIsVictim { get; }

        IHasStats MyHasStats { get; }

        IInitiativeRollData Initiative { get; }

        void RegisterGeneralRoller(IGeneralRoller generalRoller);

        void StartTurn(ActionEnum action, BonusActionEnum bonus, ReactionEnum reaction);

        void EndTurn();

        void EndMove();

        void EndAttack();
    }
}