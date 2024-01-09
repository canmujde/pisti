namespace CMCore.Contracts
{
    public interface IPlayer
    {
        void DoWaitForTurn();
        void DoPlay();
        void DoThink();
    }
}
