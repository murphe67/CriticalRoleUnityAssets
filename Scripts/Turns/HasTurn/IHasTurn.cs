using CriticalRole.Turns;

public interface IHasTurn
{
    ITurnSort TurnSort { get; }

    IContents MyHexContents { get; }

    IHasSpeed MyHasSpeed { get; }

    void Initialise(ITurnController turnController);

    void StartTurn();

    void EndTurn();

    void EndMove();

    void EndAttack();
}