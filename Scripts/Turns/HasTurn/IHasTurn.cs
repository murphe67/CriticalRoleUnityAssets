using CriticalRole.Turns;

public interface IHasTurn
{
    ITurnSort TurnSort { get; }

    IHexContents MyHexContents { get; }

    IHasSpeed MyHasSpeed { get; }

    void Initialise(ITurnController turnController);

    void StartTurn();

    void EndTurn();

    void EndMove();
}