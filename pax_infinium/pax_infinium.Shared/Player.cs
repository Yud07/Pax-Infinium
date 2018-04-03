using System;
using System.Collections.Generic;
using System.Text;
using MCTS.V2.Interfaces;

namespace pax_infinium
{
    public class Player : IPlayer
    {
        private String name;

        public Player(String name)
        {
            this.name = name;
        }

        public string Name
        {
            get
            {
                return name;
            }
        }
    }
}
