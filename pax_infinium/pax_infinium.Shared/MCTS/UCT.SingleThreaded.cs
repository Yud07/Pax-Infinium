﻿
namespace MCTS
{
    using System;

    using Interfaces;
    using Node;

    public static partial class UCT
    {
        public static IMove ComputeSingleThreadedUCT(IGameState gameState, int itermax, bool verbose, Action<string> printfn, float uctk, int secs)
        {
            var rootNode = new SingleThreadedNode(null, null, gameState, uctk);
            DateTime time = DateTime.Now;
            DateTime end = time.AddSeconds(secs);//30);

            for (var i = 0; i < itermax && time < end; i++)
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

                // Rollout
                state.PlayRandomlyUntilTheEnd();

                // Backpropagate
                while (node != null)
                {
                    node.Update(state.GetResult(node.PlayerJustMoved));
                    node = node.Parent;
                }
            }
            if (verbose)
            {
                //printfn(rootNode.DisplayTree(0));
                printfn(rootNode.DisplayMostVisistedChild());
            }

            return rootNode.MostVisitedMove();
        }
    }
}
