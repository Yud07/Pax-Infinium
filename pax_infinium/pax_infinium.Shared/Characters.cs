using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace pax_infinium
{
    public class Characters
    {
        public List<Character> list;

        public Characters()
        {
            list = new List<Character>();
        }
        public void Update(GameTime gameTime)
        {
            foreach (Character character in list)
            {
                character.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            foreach (Character character in list)
            {
                character.Draw(spriteBatch);
            }
        }
    }
}
