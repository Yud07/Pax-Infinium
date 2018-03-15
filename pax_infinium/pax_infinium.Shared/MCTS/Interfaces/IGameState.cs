namespace MCTS.Interfaces
{
    using Enum;
    using System.Collections.Generic;

    public interface IGameState
    {
        IEnumerable<IMove> GetMoves();

        void PlayRandomlyUntilTheEnd(int maxMoves);

        void DoMove(IMove move);

        EGameFinalStatus GetResult(IPlayer player);

        IPlayer PlayerJustMoved { get; }

        IGameState Clone();
    }
}
