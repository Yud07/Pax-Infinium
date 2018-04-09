namespace MCTS.V2.UCT
{
    using System;

    using Interfaces;
    using Node;
    using pax_infinium;
    using System.Linq;

    public static class SingleThreaded
    {
        public static IMove ComputeSingleThreadedUCT(IGameState gameState, bool verbose, Action<string> printfn, float uctk, int secs)
        {
            var rootNode = new SingleThreadedNode(null, null, gameState, uctk);
            DateTime time = DateTime.Now;
            DateTime end = time.AddSeconds(secs);
            bool exitEarlyCheck = true;

            int i = 0;
            for (; time < end; i++)
            {
                time = DateTime.Now;

                INode node = rootNode;

                // Exit early if we're not learning anything --> Go straight to tiebreak/playout
                if (exitEarlyCheck && ((SingleThreadedNode)node).Childs.Count() >= ((SingleThreadedNode)node).numMoves)
                {
                    bool atLeastOneWin = false;
                    foreach (INode c in ((SingleThreadedNode)node).Childs)
                    {
                        if (c.Wins > 0)
                        {
                            atLeastOneWin = true;
                            break;
                        }
                    }
                    if (!atLeastOneWin)
                    {
                        Console.WriteLine("No Wins, Quit Early With this long left: " + (end - time).ToString());
                        break;
                    }
                    else
                    {
                        exitEarlyCheck = false;
                    }
                }

                var state = ((Level)gameState).Clone();

                // Select
                while (node.NodeIsFullyExpandedAndNonterminal)
                {
                    //if (verbose)
                    //{
                    //    printfn(node.DisplayUTC());
                    //}
                    node = node.UCTSelectChild();
                    state = node.Move.DoMove(state);
                }

                // Expand
                var result = node.GetRandomMoveOrIsFalse();
                if (result.Item1)
                {
                    var move = result.Item2;
                    state = move.DoMove(state);
                    Func<INode> constructor = () => new SingleThreadedNode(node, move, state, node.UCTK);
                    node = node.AddChild(constructor);
                }

                // Rollout
                state.PlayRandomlyUntilTheEnd();

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
            return rootNode.MostVisitedMoveTieBreakWithPlayoutHybrid();
        }
    }
}
