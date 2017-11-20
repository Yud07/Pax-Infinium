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

        public void AddCharacter(string name, int job, int team, Vector2 origin, Vector3 position, String direction, GraphicsDeviceManager graphics)
        {
            Texture2D nw;
            Texture2D ne;
            Texture2D sw;
            Texture2D se;
            Texture2D fl;
            Texture2D fr;
            switch (job)
            {
                case 0: // Soldier
                    if (team == 0)
                    {
                        nw = World.textureManager["Blue Soldier\\Blue Soldier NW"];
                        ne = World.textureManager["Blue Soldier\\Blue Soldier NE"];
                        sw = World.textureManager["Blue Soldier\\Blue Soldier SW"];
                        se = World.textureManager["Blue Soldier\\Blue Soldier SE"];
                        fl = World.textureManager["Blue Soldier\\Blue Soldier FL"];
                        fr = World.textureManager["Blue Soldier\\Blue Soldier FR"];
                    }
                    else
                    {
                        nw = World.textureManager["Red Soldier\\Red Soldier NW"];
                        ne = World.textureManager["Red Soldier\\Red Soldier NE"];
                        sw = World.textureManager["Red Soldier\\Red Soldier SW"];
                        se = World.textureManager["Red Soldier\\Red Soldier SE"];
                        fl = World.textureManager["Red Soldier\\Red Soldier FL"];
                        fr = World.textureManager["Red Soldier\\Red Soldier FR"];
                    }
                    break;
                case 1: // Hunter
                    if (team == 0)
                    {
                        nw = World.textureManager["Blue Hunter\\Blue Hunter NW"];
                        ne = World.textureManager["Blue Hunter\\Blue Hunter NE"];
                        sw = World.textureManager["Blue Hunter\\Blue Hunter SW"];
                        se = World.textureManager["Blue Hunter\\Blue Hunter SE"];
                        fl = World.textureManager["Blue Hunter\\Blue Hunter FL"];
                        fr = World.textureManager["Blue Hunter\\Blue Hunter FR"];
                    }
                    else
                    {
                        nw = World.textureManager["Red Hunter\\Red Hunter NW"];
                        ne = World.textureManager["Red Hunter\\Red Hunter NE"];
                        sw = World.textureManager["Red Hunter\\Red Hunter SW"];
                        se = World.textureManager["Red Hunter\\Red Hunter SE"];
                        fl = World.textureManager["Red Hunter\\Red Hunter FL"];
                        fr = World.textureManager["Red Hunter\\Red Hunter FR"];
                    }
                    break;
                case 2: // (Black Mage) Mage
                    if (team == 0)
                    {
                        nw = World.textureManager["Blue Mage\\Blue Mage NW"];
                        ne = World.textureManager["Blue Mage\\Blue Mage NE"];
                        sw = World.textureManager["Blue Mage\\Blue Mage SW"];
                        se = World.textureManager["Blue Mage\\Blue Mage SE"];
                        fl = World.textureManager["Blue Mage\\Blue Mage FL"];
                        fr = World.textureManager["Blue Mage\\Blue Mage FR"];
                    }
                    else
                    {
                        nw = World.textureManager["Red Mage\\Red Mage NW"];
                        ne = World.textureManager["Red Mage\\Red Mage NE"];
                        sw = World.textureManager["Red Mage\\Red Mage SW"];
                        se = World.textureManager["Red Mage\\Red Mage SE"];
                        fl = World.textureManager["Red Mage\\Red Mage FL"];
                        fr = World.textureManager["Red Mage\\Red Mage FR"];
                    }
                    break;
                case 3: // (White Mage) Healer
                    if (team == 0)
                    {
                        nw = World.textureManager["Blue Healer\\Blue Healer NW"];
                        ne = World.textureManager["Blue Healer\\Blue Healer NE"];
                        sw = World.textureManager["Blue Healer\\Blue Healer SW"];
                        se = World.textureManager["Blue Healer\\Blue Healer SE"];
                        fl = World.textureManager["Blue Healer\\Blue Healer FL"];
                        fr = World.textureManager["Blue Healer\\Blue Healer FR"];
                    }
                    else
                    {
                        nw = World.textureManager["Red Healer\\Red Healer NW"];
                        ne = World.textureManager["Red Healer\\Red Healer NE"];
                        sw = World.textureManager["Red Healer\\Red Healer SW"];
                        se = World.textureManager["Red Healer\\Red Healer SE"];
                        fl = World.textureManager["Red Healer\\Red Healer FL"];
                        fr = World.textureManager["Red Healer\\Red Healer FR"];
                    }
                    break;
                case 4: // Thief
                    if (team == 0)
                    {
                        nw = World.textureManager["Blue Thief\\Blue Thief NW"];
                        ne = World.textureManager["Blue Thief\\Blue Thief NE"];
                        sw = World.textureManager["Blue Thief\\Blue Thief SW"];
                        se = World.textureManager["Blue Thief\\Blue Thief SE"];
                        fl = World.textureManager["Blue Thief\\Blue Thief FL"];
                        fr = World.textureManager["Blue Thief\\Blue Thief FR"];
                    }
                    else
                    {
                        nw = World.textureManager["Red Thief\\Red Thief NW"];
                        ne = World.textureManager["Red Thief\\Red Thief NE"];
                        sw = World.textureManager["Red Thief\\Red Thief SW"];
                        se = World.textureManager["Red Thief\\Red Thief SE"];
                        fl = World.textureManager["Red Thief\\Red Thief FL"];
                        fr = World.textureManager["Red Thief\\Red Thief FR"];
                    }
                    break;
                default:
                    nw = ne = sw = se = fl = fr = Game1.world.textureConverter.GenRectangle(64, 128, Color.Blue);
                    break;
            }
            list.Add(new Character(name, team, origin, position, direction, nw, ne, sw, se, fl, fr, graphics, new SpriteSheetInfo(64, 128)));

            Character newCharacter = list[list.Count - 1];
            newCharacter.job = job;

            switch (job)
            {
                case 0: // Soldier
                    newCharacter.move = 4;
                    newCharacter.health = 322;
                    newCharacter.mp = newCharacter.maxMP = 44;
                    newCharacter.WAttack = 112;                    
                    newCharacter.WDefense = 107;
                    newCharacter.MAttack = 59;
                    newCharacter.MDefense = 71;
                    newCharacter.jump = 2;
                    newCharacter.evasion = 0;
                    newCharacter.speed = 72;
                    newCharacter.weaponRange = 1;
                    newCharacter.magicRange = 1;
                    break;
                case 1: // Hunter
                    newCharacter.move = 4;
                    newCharacter.health = 258;                    
                    newCharacter.mp = newCharacter.maxMP = 113;
                    newCharacter.WAttack = 102;
                    newCharacter.WDefense = 84;
                    newCharacter.MAttack = 68;
                    newCharacter.MDefense = 83;
                    newCharacter.jump = 2;
                    newCharacter.evasion = 5;
                    newCharacter.speed = 80;
                    newCharacter.weaponRange = 5;
                    break;
                case 2: // Black Mage
                    newCharacter.move = 3;
                    newCharacter.health = 224;
                    newCharacter.mp = newCharacter.maxMP = 183;
                    newCharacter.WAttack = 77;
                    newCharacter.WDefense = 78;
                    newCharacter.MAttack = 97;
                    newCharacter.MDefense = 129;
                    newCharacter.jump = 2;
                    newCharacter.evasion = 0;
                    newCharacter.speed = 69;
                    newCharacter.weaponRange = 1;
                    newCharacter.magicRange = 3;
                    break;
                case 3: // White Mage/Healer
                    newCharacter.move = 3;
                    newCharacter.health = 258;
                    newCharacter.mp = newCharacter.maxMP = 152;
                    newCharacter.WAttack = 78;
                    newCharacter.WDefense = 86;
                    newCharacter.MAttack = 79;
                    newCharacter.MDefense = 110;
                    newCharacter.jump = 2;
                    newCharacter.evasion = 0;
                    newCharacter.speed = 73;
                    newCharacter.weaponRange = 1;
                    newCharacter.magicRange = 3;
                    break;
                case 4: // Thief
                    newCharacter.move = 4;
                    newCharacter.health = 257;
                    newCharacter.mp = newCharacter.maxMP = 44;
                    newCharacter.WAttack = 97;
                    newCharacter.WDefense = 93;
                    newCharacter.MAttack = 70;
                    newCharacter.MDefense = 64;
                    newCharacter.jump = 3;
                    newCharacter.evasion = 5;
                    newCharacter.speed = 80;
                    newCharacter.weaponRange = 1;
                    newCharacter.magicRange = 1;
                    break;
                default:

                    break;
            }
        }
    }
}
