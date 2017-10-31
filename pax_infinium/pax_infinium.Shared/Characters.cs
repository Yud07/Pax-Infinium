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
        public Character selectedCharacter;

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

        public void AddCharacter(string name, int job, int team, Vector2 origin, Vector3 position, GraphicsDeviceManager graphics)
        {
            Texture2D image;
            switch (job)
            {
                case 0: // Soldier
                    if (team == 0)
                    {
                        image = World.textureManager["Soldier"];
                    }
                    else
                    {
                        image = World.textureManager["Soldier2"];
                    }
                    break;
                case 1: // Hunter
                    if (team == 0)
                    {
                        image = World.textureManager["Hunter"];
                    }
                    else
                    {
                        image = World.textureManager["Hunter2"];
                    }
                    break;
                case 2: // Black Mage
                    if (team == 0)
                    {
                        image = World.textureManager["Black Mage"];
                    }
                    else
                    {
                        image = World.textureManager["Black Mage2"];
                    }
                    break;
                default:
                    image = Game1.world.textureConverter.GenRectangle(64, 128, Color.Blue);
                    break;
            }
            list.Add(new Character(name, team, origin, position, image, graphics, new SpriteSheetInfo(64, 128)));

            Character newCharacter = list[list.Count - 1];

            switch (job)
            {
                case 0: // Soldier
                    newCharacter.move = 4;
                    newCharacter.health = 322;
                    newCharacter.mp = 44;
                    newCharacter.WAttack = 112;                    
                    newCharacter.WDefense = 107;
                    newCharacter.MAttack = 59;
                    newCharacter.MDefense = 71;
                    newCharacter.jump = 2;
                    newCharacter.evasion = 0;
                    newCharacter.speed = 72;
                    break;
                case 1: // Hunter
                    newCharacter.move = 4;
                    newCharacter.health = 258;                    
                    newCharacter.mp = 113;
                    newCharacter.WAttack = 102;
                    newCharacter.WDefense = 84;
                    newCharacter.MAttack = 68;
                    newCharacter.MDefense = 83;
                    newCharacter.jump = 2;
                    newCharacter.evasion = 5;
                    newCharacter.speed = 80;
                    break;
                case 2: // Black Mage
                    newCharacter.move = 3;
                    newCharacter.health = 224;
                    newCharacter.mp = 183;
                    newCharacter.WAttack = 77;
                    newCharacter.WDefense = 78;
                    newCharacter.MAttack = 97;
                    newCharacter.MDefense = 129;
                    newCharacter.jump = 2;
                    newCharacter.evasion = 0;
                    newCharacter.speed = 69;
                    break;
                default:

                    break;
            }
        }
    }
}
