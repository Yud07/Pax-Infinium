using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace pax_infinium.AStar
{
    class Node : IComparable<Node>
    {
        public Cube cube;
        public Vector3 pos;
        public List<Node> neighbors;
        public int costSoFar;
        public int costToGo;
        public Node prev;

        public Node (Cube cube)
        {
            this.cube = cube;
            pos = cube.gridPos;
            this.neighbors = new List<Node>();
            this.costSoFar = int.MaxValue;
        }
        
        public int CompareTo(Node other)
        {
            float thisScore = getScore();
            float thatScore = other.getScore();

            if (thisScore < thatScore)
            {
                return -1;
            }
            else if (thisScore > thatScore)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public float getScore()
        {
            return costSoFar + costToGo;
        }

        public void SetCostToGo(Vector3 goal)
        {
            this.costToGo = Game1.world.cubeDist(pos, goal);
        }
    }
}
