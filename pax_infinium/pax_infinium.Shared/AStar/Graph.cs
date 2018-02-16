using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace pax_infinium.AStar
{
    class Graph
    {
        public Vector3 start;
        public List<Node> nodes;
        public int jumpHeight;
        //jumpWidth -------------------------- Future

        public Graph(Character character, Grid grid)
        {
            jumpHeight = character.jump;
            start = character.gridPos;
            nodes = new List<Node>();
            bool[,,] binMat = grid.getBinMatrix();

            for (int x = 0; x < grid.width - 1; x++)
            {
                for (int y = 0; y < grid.depth - 1; y++)
                {
                    for (int z = 0; z < grid.height - 1; z++)
                    {
                        if (binMat[x, y, z])
                        {
                            Cube cube = grid.getCube(new Vector3(x, y, z));
                            if (grid.TopExposed(cube.gridPos))
                            {
                                Character charAtPos = grid.CharacterAtPos(cube.gridPos);
                                if (charAtPos == null || charAtPos.team == character.team)
                                {
                                    nodes.Add(new Node(cube));
                                }
                            }
                        }
                    }
                }
            }

            foreach (Node node in nodes)
            {
                foreach (Node n in nodes)
                {
                    if (node != n)
                    {
                        if (!node.neighbors.Contains(n) && node.cube.isAdjacent(n.pos, jumpHeight))
                        {
                            node.neighbors.Add(n);
                            n.neighbors.Add(node);
                        }
                    }
                }
            }
        }

        public Node GetNode(int x, int y, int z)
        {
            return GetNode(new Vector3(x, y, z));
        }

        public Node GetNode(Vector3 pos)
        {
            foreach (Node node in nodes)
            {
                if (node.pos == pos)
                {
                    return node;
                }
            }

            return null;
        }

        public void ClearNodeData()
        {
            foreach (Node n in nodes)
            {
                n.costSoFar = int.MaxValue;
                n.costToGo = int.MaxValue;
                n.prev = null;
            }
        }

        public List<Vector3> AStar(Vector3 goal)
        {
            ClearNodeData();
            foreach (Node n in nodes)
            {
                n.SetCostToGo(goal);
            }
            Node startN = GetNode(start);
            Node endN = GetNode(goal);
            if (startN == null || endN == null) { return null; }

            var leafsPQueue = new C5.IntervalHeap<Node>(); // Priority Queue
            startN.costSoFar = 0;
            leafsPQueue.Add(startN);

            while (!leafsPQueue.IsEmpty)
            {
                Node leaf = leafsPQueue.FindMin();
                leafsPQueue.DeleteMin();

                if (leaf == endN) { break; }
                foreach (Node neighbor in leaf.neighbors)
                {
                    int newCostSoFar = leaf.costSoFar + Game1.world.cubeDist(leaf.pos, neighbor.pos);
                    if (neighbor.costSoFar > newCostSoFar)
                    {
                        neighbor.prev = leaf;
                        neighbor.costSoFar = newCostSoFar;
                        leafsPQueue.Add(neighbor);
                    }
                }
            }

            if (endN.prev != null)
            {
                List<Vector3> result = new List<Vector3>();
                Node node = endN;
                while(node.prev != null)
                {
                    result.Add(node.prev.cube.gridPos);
                    node = node.prev;
                }
                result.Reverse();
                return result;
            }
            else
            {
                return null;
            }         
        }
    }
}
