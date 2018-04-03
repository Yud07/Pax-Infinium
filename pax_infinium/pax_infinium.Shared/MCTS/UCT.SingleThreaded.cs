
namespace MCTS
{
    using System;

    using Interfaces;
    using Node;
    using pax_infinium;

    public static partial class UCT
    {
        public static IMove ComputeSingleThreadedUCT(IGameState gameState, bool verbose, Action<string> printfn, float uctk, int secs, int maxPlayout, int maxRollout)
        {
            var rootNode = new SingleThreadedNode(null, null, gameState, uctk);
            DateTime time = DateTime.Now;
            DateTime end = time.AddSeconds(secs);

            int i = 0;
            for (; time < end; i++)
            {
                time = DateTime.Now;

                INode node = rootNode;
                var state = gameState.Clone();

                // Select
                while (node.NodeIsFullyExpandedAndNonterminal)
                {
                    //if (verbose)
                    //{
                    //    printfn(node.DisplayUTC());
                    //}
                    node = node.UCTSelectChild();
                    state.DoMove(node.Move);
                }

                // Expand
                var result = node.GetRandomMoveOrIsFalse();
                if (result.Item1)
                {
                    var move = result.Item2;
                    state.DoMove(move);
                    Func<float, INode> constructor = (f) => new SingleThreadedNode(node, move, state, f);
                    node = node.AddChild(constructor);
                }

                // Playout + Rollout
                state.Simulate(maxPlayout, maxRollout);


                // Backpropagate
                while (node != null)
                {
                    node.Update(state.GetResult(node.PlayerJustMoved));
                    node = node.Parent;
                }
            }
            Game1.world.level.IterationsPerTurn.Add(i);

            if (verbose)
            {
                //printfn(rootNode.DisplayTree(0));
                printfn(rootNode.DisplayMostVisistedChild());
            }

            //return rootNode.MostVisitedMove();
            //return rootNode.MostVisitedMoveWithTieBreaks();
            return rootNode.MostVisitedMoveTieBreakWithPlayoutHybrid();
        }
    }
}
