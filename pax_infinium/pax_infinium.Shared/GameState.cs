using System;
using System.Collections.Generic;
using System.Text;
using MCTS.V2.Interfaces;
using MCTS.Enum;

namespace pax_infinium
{
    class GameState : IGameState
    {
        public IPlayer PlayerJustMoved => throw new NotImplementedException();

        public IEnumerable<IMove> GetMoves()
        {
            throw new NotImplementedException();
        }

        public EGameFinalStatus GetResult(IPlayer player)
        {
            throw new NotImplementedException();
        }

        public IGameState PlayRandomlyUntilTheEnd()
        {
            throw new NotImplementedException();
        }
    }
}
