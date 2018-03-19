namespace MCTS.Interfaces
{
    using Enum;
    using System.Collections.Generic;

    public interface IGameState
    {
        IEnumerable<IMove> GetMoves();

        void Simulate(int maxPlayout, int maxRollout);

        void DoMove(IMove move);

        EGameFinalStatus GetResult(IPlayer player);

        IPlayer PlayerJustMoved { get; }

        IGameState Clone();
    }
}
