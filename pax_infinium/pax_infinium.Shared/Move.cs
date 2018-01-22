using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using System.Linq;
using MCTS.Interfaces;

namespace pax_infinium
{
    public class Move : IMove
    {
        private String name;
        public int noneMoveBeforeMoveAfter; // 0-2
        Vector3 movePos; // irrelevant if no movement
        int nothingAttackSpecial; // 0-2
        Vector3 attackSpecialPos; // irrelevant if nothingAttackSpecial is 0

        public Move(int noneMoveBeforeMoveAfter, Vector3 movement, int nothingAttackSpecial, Vector3 attackSpecialPos)
        {
            this.noneMoveBeforeMoveAfter = noneMoveBeforeMoveAfter;
            this.movePos = movement;
            this.nothingAttackSpecial = nothingAttackSpecial;
            this.attackSpecialPos = attackSpecialPos;
            if (noneMoveBeforeMoveAfter == 0)
            {
                name = "Don't move";
            }
            else if (noneMoveBeforeMoveAfter == 1)
            {
                name = "Move to " + movement;
            }
            else
            {
                name = "Act then move to " + movement;
            }

            if (nothingAttackSpecial == 0)
            {
                name += ", do nothing";
            }
            else if (nothingAttackSpecial == 1)
            {
                name += ", attack " + attackSpecialPos;
            }
            else
            {
                name += ", use special at " + attackSpecialPos;  
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public void DoMove(Level level)
        {
            Character player = level.grid.characters.list[0];
            switch (noneMoveBeforeMoveAfter) {
                case 0:
                    NothingAttackSpecial(player, level);
                    break;
                case 1:
                    player.Move(movePos);
                    NothingAttackSpecial(player, level);
                    break;
                case 2:
                    NothingAttackSpecial(player, level);
                    player.Move(movePos);
                    break;
                default:
                    throw new NotImplementedException();
            }
            EndTurn(level);
        }

        public void DoMove(Level level, GameTime gameTime)
        {
            Character player = level.grid.characters.list[0];
            switch (noneMoveBeforeMoveAfter)
            {
                case 0:
                    NothingAttackSpecial(player, level, gameTime);
                    break;
                case 1:
                    player.Rotate(movePos);
                    player.Move(movePos);
                    NothingAttackSpecial(player, level, gameTime);
                    break;
                case 2:
                    NothingAttackSpecial(player, level, gameTime);
                    player.Rotate(movePos);
                    player.Move(movePos);
                    break;
                default:
                    throw new NotImplementedException();
            }
            EndTurn(level);
        }

        public void NothingAttackSpecial(Character player, Level level)
        {
            // USE Game1 code!
            Character character;
            Cube cube;
            switch (nothingAttackSpecial)
            {
                case 0:
                    break;
                case 1:
                    player.Rotate(attackSpecialPos);
                    Character toBeKilled;
                    character = level.grid.CharacterAtPos(attackSpecialPos);
                    toBeKilled = player.attack(character);
                    if (toBeKilled != null)
                    {
                        level.grid.characters.list.Remove(toBeKilled);
                    }
                    break;
                case 2:
                    player.Rotate(attackSpecialPos);
                    player.payForCast(8);
                    switch (player.job)
                    {
                        case 0:
                            character = level.grid.CharacterAtPos(attackSpecialPos);
                            player.SoldierSpecial(character);
                            break;
                        case 1:
                            throw new NotImplementedException();
                            //break;
                        case 2:
                            //player.MageSpecial()
                            List<Character> toBeKilledList = new List<Character>();
                            cube = level.grid.getCube(attackSpecialPos);
                            foreach (Character chara in level.grid.characters.list)
                            {
                                if (cube.isAdjacent(chara.gridPos) || chara.gridPos == cube.gridPos)
                                {
                                    Character result = player.MageSpecial(chara);
                                    if (result != null)
                                    {
                                        toBeKilledList.Add(result);
                                    }
                                }
                            }
                            level.attacked = true;
                            foreach (Character c in toBeKilledList)
                            {
                                level.grid.characters.list.Remove(c);
                            }
                            break;
                        case 3:
                            //player.HealerSpecial()
                            cube = level.grid.getCube(attackSpecialPos);
                            foreach (Character chara in level.grid.characters.list)
                            {
                                if (cube.isAdjacent(chara.gridPos) || chara.gridPos == cube.gridPos)
                                {
                                    player.HealerSpecial(chara);
                                }
                            }
                            level.attacked = true;
                            break;
                        case 4:
                            character = level.grid.CharacterAtPos(attackSpecialPos);
                            player.ThiefSpecial(character);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public void NothingAttackSpecial(Character player, Level level, GameTime gameTime)
        {
            // USE Game1 code!
            Character character;
            Cube cube;
            switch (nothingAttackSpecial)
            {
                case 0:
                    break;
                case 1:
                    player.Rotate(attackSpecialPos);
                    Character toBeKilled;
                    character = level.grid.CharacterAtPos(attackSpecialPos);
                    toBeKilled = player.attack(character, gameTime);
                    if (toBeKilled != null)
                    {
                        level.grid.characters.list.Remove(toBeKilled);
                    }
                    break;
                case 2:
                    player.Rotate(attackSpecialPos);
                    player.payForCast(8, gameTime);
                    switch (player.job)
                    {
                        case 0:
                            character = level.grid.CharacterAtPos(attackSpecialPos);
                            player.SoldierSpecial(character, gameTime);
                            break;
                        case 1:
                            throw new NotImplementedException();
                        //break;
                        case 2:
                            //player.MageSpecial()
                            List<Character> toBeKilledList = new List<Character>();
                            cube = level.grid.getCube(attackSpecialPos);
                            foreach (Character chara in level.grid.characters.list)
                            {
                                if (cube.isAdjacent(chara.gridPos) || chara.gridPos == cube.gridPos)
                                {
                                    Character result = player.MageSpecial(chara, gameTime);
                                    if (result != null)
                                    {
                                        toBeKilledList.Add(result);
                                    }
                                }
                            }
                            level.attacked = true;
                            foreach (Character c in toBeKilledList)
                            {
                                level.grid.characters.list.Remove(c);
                            }
                            break;
                        case 3:
                            //player.HealerSpecial()
                            cube = level.grid.getCube(attackSpecialPos);
                            foreach (Character chara in level.grid.characters.list)
                            {
                                if (cube.isAdjacent(chara.gridPos) || chara.gridPos == cube.gridPos)
                                {
                                    player.HealerSpecial(chara, gameTime);
                                }
                            }
                            level.attacked = true;
                            break;
                        case 4:
                            character = level.grid.CharacterAtPos(attackSpecialPos);
                            player.ThiefSpecial(character, gameTime);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void EndTurn(Level level)
        {
            level.turn++;
            if (level.grid.characters.list.Count > 0)
            {
                Character tempCharacter = level.grid.characters.list[0];
                level.grid.characters.list.Remove(tempCharacter);
                level.grid.characters.list.Add(tempCharacter);

                //level.setupTurnOrderIcons();

                Character player = level.grid.characters.list[0];

                /*float distanceToNorth = Game1.world.cubeDist(player.gridPos, new Vector3(0, 0, player.gridPos.Z));
                float distanceToEast = Game1.world.cubeDist(player.gridPos, new Vector3(level.grid.width, 0, player.gridPos.Z));
                float distanceToSouth = Game1.world.cubeDist(player.gridPos, new Vector3(level.grid.width, level.grid.depth, player.gridPos.Z));
                float distanceToWest = Game1.world.cubeDist(player.gridPos, new Vector3(0, level.grid.depth, player.gridPos.Z));

                List<float> distancesToCorners = new List<float>();
                //East right once
                //North right twice
                //West right 3 times

                distancesToCorners.Add(distanceToEast);
                distancesToCorners.Add(distanceToNorth);
                distancesToCorners.Add(distanceToWest);
                distancesToCorners.Add(distanceToSouth);

                float min = distancesToCorners.Min();
                //Console.WriteLine("e:" + distancesToCorners[0] + " n:" + distancesToCorners[1] + " w:" + distancesToCorners[2] + " s:" + distancesToCorners[3]);
                if (min != distanceToSouth)
                {
                    if (distanceToEast == min)
                    {
                        //Console.WriteLine("east");
                        level.grid.rotate(true, level);
                    }
                    else if (distanceToWest == min)
                    {
                        //Console.WriteLine("west");
                        level.grid.rotate(false, level);
                    }
                    else
                    {
                        //Console.WriteLine("north");
                        level.grid.rotate(true, level);
                        level.grid.rotate(true, level);
                    }
                }
                */

                //Console.WriteLine("turn: " + turn);
                //text.Text = turnOrder[turn % turnOrder.Length] + "'s turn:" + turn.ToString();
                level.text.Text = "Turn:" + level.turn.ToString();
                level.playerName.Text = player.name;
                String t = player.health + "              " + player.mp;
                t += "\n\n\n" + player.move + "                 " + player.jump;
                t += "\n\n\n" + player.speed + "              " + player.evasion;
                t += "\n\n\n" + player.WAttack + "              " + player.MAttack;
                t += "\n\n\n" + player.WDefense + "              " + player.MDefense;
                t += "\n\n\n" + player.weaponRange + "                 " + player.magicRange;
                level.playerStatus.Text = t;
                level.playerFace.tex = player.faceLeft;
                if (player.team == 0)
                {
                    level.playerName.color = Color.Blue;
                    level.playerStatus.color = Color.Blue;

                }
                else if (player.team == 1)
                {
                    level.playerName.color = Color.Red;
                    level.playerStatus.color = Color.Red;
                }
                /*int mpGain = 10;
                if (player.mp + mpGain <= player.maxMP)
                {
                    player.mp += mpGain;
                }
                else if (player.mp + mpGain > player.maxMP)
                {
                    player.mp = player.maxMP;
                }*/
            }

            level.moved = false;
            level.attacked = false;
            level.rotated = false;
        }
    }
}
