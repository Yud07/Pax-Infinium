using System;
using System.Collections.Generic;
using System.Text;

namespace pax_infinium
{
    public class Level
    {
        Random random;
        long seed;

        public Level()
        {
            random = new Random();
            //seed = random.NextLong;
            
            //seed += random.Next(0 9).ToString();
        }
    }
}
