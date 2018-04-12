using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using pax_infinium.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace pax_infinium
{
    public class Characters : ICloneable
    {
        public List<Character> list;
        public Character selectedCharacter;

        public Characters()
        {
            list = new List<Character>();
        }
        public void Update(GameTime gameTime)
        {
            Character[] tempList = new Character[list.Count];
            list.CopyTo(tempList);
            foreach (Character character in tempList)
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

        public void AddCharacter(EJob job, int team, EPersonality personality, Vector2 origin, GraphicsDeviceManager graphics)
        {
            Texture2D nw;
            Texture2D ne;
            Texture2D sw;
            Texture2D se;
            Texture2D fl;
            Texture2D fr;
            switch ((int) job)
            {
                case 0: // Soldier
                    if (team == 0)
                    {
                        nw = World.textureManager["Red Soldier\\Red Soldier NW"];
                        ne = World.textureManager["Red Soldier\\Red Soldier NE"];
                        sw = World.textureManager["Red Soldier\\Red Soldier SW"];
                        se = World.textureManager["Red Soldier\\Red Soldier SE"];
                        fl = World.textureManager["Red Soldier\\Red Soldier FL"];
                        fr = World.textureManager["Red Soldier\\Red Soldier FR"];
                    }
                    else
                    {
                        nw = World.textureManager["Blue Soldier\\Blue Soldier NW"];
                        ne = World.textureManager["Blue Soldier\\Blue Soldier NE"];
                        sw = World.textureManager["Blue Soldier\\Blue Soldier SW"];
                        se = World.textureManager["Blue Soldier\\Blue Soldier SE"];
                        fl = World.textureManager["Blue Soldier\\Blue Soldier FL"];
                        fr = World.textureManager["Blue Soldier\\Blue Soldier FR"];
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
            String name = "";
            if (personality == EPersonality.Aggressive)
            {
                name += "Aggressive ";
            }
            else if (personality == EPersonality.Defensive)
            {
                name += "Defensive ";
            }
            if (team == 0)
            {
                name += "Green ";
            }
            else
            {
                name += "Purple ";
            }
            if (job == EJob.Soldier)
            {
                name += "Soldier";
            }
            else if (job == EJob.Hunter)
            {
                name += "Hunter";
            }
            else if (job == EJob.Mage)
            {
                name += "Mage";
            }
            else if (job == EJob.Healer)
            {
                name += "Healer";
            }
            else if (job == EJob.Thief)
            {
                name += "Thief";
            }
            list.Add(new Character(name, team, origin, nw, ne, sw, se, fl, fr, graphics, new SpriteSheetInfo(64, 128)));

            Character newCharacter = list[list.Count - 1];
            newCharacter.job = job;
            newCharacter.personality = personality;
            newCharacter.genes = selectGenes(job, personality);

            switch ((int) job)
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
                    newCharacter.magicRange = 5;
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
            if (true)//newCharacter.team == 1)
            {
                newCharacter.health /= 2;
            }
            newCharacter.startingHealth = newCharacter.health;
            newCharacter.startingMP = newCharacter.mp;
        }
        public object Clone()
        {
            Characters clone = (Characters) this.MemberwiseClone();
            clone.list = new List<Character>();
            this.list.ForEach(a => clone.list.Add((Character)a.Clone()));
            return clone;
        }

        public int CharactersPerTeam(int team)
        {
            int result = 0;
            foreach(Character c in list)
            {
                if (c.team == team)
                {
                    result++;
                }
            }
            return result;
        }

        public int[] selectGenes(EJob job, EPersonality personality)
        {
            //return new int[7] { 1, 2, 50, 25, 1, 25, 1 }; // Default values
            int[] gene = new int[7];
            String path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Console.WriteLine("Path: " + path);
            if (!Directory.Exists(path + @"\Genes")){
                CreateGeneDirectory(path);
            }
            String filePath = path + @"\Genes";
            switch ((int)job)
            {
                case 0: // Soldier
                    filePath += @"\Soldier";
                    break;
                case 1: // Hunter
                    filePath += @"\Hunter";
                    break;
                case 2: // Black Mage
                    filePath += @"\Mage";
                    break;
                case 3: // White Mage/Healer
                    filePath += @"\Healer";
                    break;
                case 4: // Thief
                    filePath += @"\Thief";
                    break;
                default:
                    throw new NotImplementedException();
                    break;
            }
            switch ((int)personality)
            {
                case 1:
                    filePath += @"\Aggressive.txt";
                    break;
                case 2:
                    filePath += @"\Defensive.txt";
                    break;
                default:
                    filePath += @"\Default.txt";
                    break;
            }

            List<int[]> genes = new List<int[]>();
            List<float> scores = new List<float>();
            try
            {
                StreamReader sr = new StreamReader(filePath);
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    String[] entries = line.Split(' ');
                    int i = 0;
                    int[] g = new int[7];
                    if (entries.Length == 8)
                    {
                        foreach (String entry in entries)
                        {
                            if (i < 7)
                            {
                                g[i] = Int32.Parse(entry);
                            }
                            else
                            {
                                genes.Add(g);
                                scores.Add(float.Parse(entry));
                            }
                            i++;
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("StreamReader Exception during gene read");
            }

            if (genes.Count == 0)
            {
                gene = new int[7] {1, 2, 50, 25, 1, 25, 1}; // Default values
            }
            else if (genes.Count == 1)
            {
                for(int i = 0; i < 7; i++)
                {
                    if (i == 4 || i == 6)
                    {
                        gene[i] = World.Random.Next(0, 11); // 0-10
                    }
                    else
                    {
                        gene[i] = World.Random.Next(0, 101); // 0-100
                    }
                }
            }
            else
            {
                int roll = World.Random.Next(0, 100); // 0-99
                if (roll < 10) // 10 Chance new entry
                {
                    for (int i = 0; i < 7; i++)
                    {
                        if (i == 4 || i == 6)
                        {
                            gene[i] = World.Random.Next(0, 11); // 0-10
                        }
                        else
                        {
                            gene[i] = World.Random.Next(0, 101); // 0-100
                        }
                    }
                }
                else if (roll < 30) // 20 Chance random procreation
                {
                    int aIndex = World.Random.Next(0, genes.Count);
                    int bIndex = World.Random.Next(0, genes.Count);

                    int[] geneA = genes[aIndex];
                    int[] geneB = genes[bIndex];

                    for(int i = 0; i < 7; i++)
                    {
                        int r = World.Random.Next(0, 100); // 0-99
                        if (r < 10) // 10 Chance random mutation
                        {
                            if (i == 4 || i == 6)
                            {
                                gene[i] = World.Random.Next(0, 11); // 0-10
                            }
                            else
                            {
                                gene[i] = World.Random.Next(0, 101); // 0-100
                            }
                        }
                        else if (r < 55) // 45 Chance A gene
                        {
                            gene[i] = geneA[i];
                        }
                        else // 45 Chance B gene
                        {
                            gene[i] = geneB[i];
                        }
                    }
                }
                else // 70 Chance best procreate
                {
                    List<int> topTenIndex = new List<int>();

                    int i = 0;
                    int minIndex = 0;
                    float minVal = scores[0];
                    foreach (int s in scores)
                    {
                        if (topTenIndex.Count < 10)
                        {
                            if (s < minVal)
                            {
                                minVal = s;
                                minIndex = i;
                            }
                            topTenIndex.Add(i);
                        }
                        else
                        {
                            if (s > minVal)
                            {
                                topTenIndex.Remove(minIndex);
                                topTenIndex.Add(i);
                                minVal = s;
                                minIndex = i;
                                foreach (int j in topTenIndex)
                                {
                                    if (scores[j] < minVal)
                                    {
                                        minVal = scores[j];
                                        minIndex = j;
                                    }
                                }                                
                            }
                        }
                        i++;
                    }

                    // Pick 2 from top ten genes
                    int aIndex = World.Random.Next(0, topTenIndex.Count);
                    int bIndex = World.Random.Next(0, topTenIndex.Count);

                    int[] geneA = genes[aIndex];
                    int[] geneB = genes[bIndex];

                    for (int k = 0; k < 7; k++)
                    {
                        int r = World.Random.Next(0, 100); // 0-99
                        if (r < 10) // 10 Chance random mutation
                        {
                            if (i == 4 || i == 6)
                            {
                                gene[k] = World.Random.Next(0, 11); // 0-10
                            }
                            else
                            {
                                gene[k] = World.Random.Next(0, 101); // 0-100
                            }
                        }
                        else if (r < 55) // 45 Chance A gene
                        {
                            gene[k] = geneA[k];
                        }
                        else // 45 Chance B gene
                        {
                            gene[k] = geneB[k];
                        }
                    }
                }
            }

            return gene;
        }

        void CreateGeneDirectory(String path)
        {
            Directory.CreateDirectory(path + @"\Genes");

            Directory.CreateDirectory(path + @"\Genes\Soldier");
            File.Create(path + @"\Genes\Soldier\Aggressive.txt");
            File.Create(path + @"\Genes\Soldier\Defensive.txt");
            File.Create(path + @"\Genes\Soldier\Default.txt");

            Directory.CreateDirectory(path + @"\Genes\Hunter");
            File.Create(path + @"\Genes\Hunter\Aggressive.txt");
            File.Create(path + @"\Genes\Hunter\Defensive.txt");
            File.Create(path + @"\Genes\Hunter\Default.txt");

            Directory.CreateDirectory(path + @"\Genes\Mage");
            File.Create(path + @"\Genes\Mage\Aggressive.txt");
            File.Create(path + @"\Genes\Mage\Defensive.txt");
            File.Create(path + @"\Genes\Mage\Default.txt");

            Directory.CreateDirectory(path + @"\Genes\Healer");
            File.Create(path + @"\Genes\Healer\Aggressive.txt");
            File.Create(path + @"\Genes\Healer\Defensive.txt");
            File.Create(path + @"\Genes\Healer\Default.txt");

            Directory.CreateDirectory(path + @"\Genes\Thief");
            File.Create(path + @"\Genes\Thief\Aggressive.txt");
            File.Create(path + @"\Genes\Thief\Defensive.txt");
            File.Create(path + @"\Genes\Thief\Default.txt");
        }


    }
}
