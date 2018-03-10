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
                    player.Rotate(movePos, false);
                    player.Move(movePos, false);
                    NothingAttackSpecial(player, level);
                    break;
                case 2:
                    NothingAttackSpecial(player, level);
                    player.Rotate(movePos, false);
                    player.Move(movePos, false);
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
                    EndTurn(level, gameTime);
                    break;
                case 1:
                    //player.Rotate(movePos);
                    player.Move(movePos, level);
                    //NothingAttackSpecial(player, level, gameTime);
                    break;
                case 2:
                    NothingAttackSpecial(player, level, gameTime);
                    //player.Rotate(movePos);
                    player.Move(movePos, level);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public void PostMove(Level level, GameTime gameTime)
        {
            Character player = level.grid.characters.list[0];
            if (noneMoveBeforeMoveAfter == 1)
            {
                NothingAttackSpecial(player, level, gameTime);
            }
            EndTurn(level, gameTime);
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
                    player.Rotate(attackSpecialPos, false);
                    Character toBeKilled;
                    character = level.grid.CharacterAtPos(attackSpecialPos);
                    toBeKilled = player.attack(character);
                    if (toBeKilled != null)
                    {
                        level.grid.characters.list.Remove(toBeKilled);
                    }
                    break;
                case 2:
                    player.Rotate(attackSpecialPos, false);
                    player.payForCast(8);
                    switch ((int)player.job)
                    {
                        case 0:
                            character = level.grid.CharacterAtPos(attackSpecialPos);
                            player.SoldierSpecial(character);
                            break;
                        case 1:
                            character = level.grid.CharacterAtPos(attackSpecialPos);
                            player.HunterSpecial(character);
                            break;
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
                    player.Rotate(attackSpecialPos, true);
                    Character tBKill;
                    character = level.grid.CharacterAtPos(attackSpecialPos);
                    tBKill = player.attack(character, gameTime);
                    if (tBKill != null)
                    {
                        level.toBeKilled.Add(tBKill);
                    }
                    break;
                case 2:
                    player.Rotate(attackSpecialPos, true);
                    player.payForCast(8, gameTime);
                    switch ((int) player.job)
                    {
                        case 0:
                            character = level.grid.CharacterAtPos(attackSpecialPos);
                            player.SoldierSpecial(character, gameTime);
                            break;
                        case 1:
                            character = level.grid.CharacterAtPos(attackSpecialPos);
                            player.HunterSpecial(character, gameTime);
                            break;
                        case 2:
                            //player.MageSpecial()
                            cube = level.grid.getCube(attackSpecialPos);
                            foreach (Character chara in level.grid.characters.list)
                            {
                                if (cube.isAdjacent(chara.gridPos) || chara.gridPos == cube.gridPos)
                                {
                                    Character result = player.MageSpecial(chara, gameTime);
                                    if (result != null)
                                    {
                                        level.toBeKilled.Add(result);
                                    }
                                }
                            }
                            level.attacked = true;
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

                level.CalcValidMoveSpaces();

                Character player = level.grid.characters.list[0];
                
            }

            level.moved = false;
            level.attacked = false;
            level.rotated = false;
        }

        private void EndTurn(Level level, GameTime gameTime)
        {
            level.turn++;

            if (level.grid.characters.list.Count > 0)
            {
                Character tempCharacter = level.grid.characters.list[0];
                level.grid.characters.list.Remove(tempCharacter);
                level.grid.characters.list.Add(tempCharacter);

                level.recalcTeamHealthBar();
                /*foreach (Character c in level.grid.characters.list)
                {
                    c.recalcPos();
                }
                level.grid.onCharacterMoved(level);*/

                level.CalcValidMoveSpaces();
                level.setupTurnOrderIcons();

                Character player = level.grid.characters.list[0];

                level.RotateTo = player; 

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
                    level.playerName.position = new Vector2(260, 970);
                    level.playerName.color = Color.Green;
                    level.playerStatus.color = Color.Green;

                }
                else if (player.team == 1)
                {
                    level.playerName.position = new Vector2(260, 750);
                    level.playerName.color = Color.Purple;
                    level.playerStatus.color = Color.Purple;
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
                if (player.team == 0)
                {
                    level.thoughtBubble.position = player.position;
                    level.thoughtBubble.position.X += 10;
                    level.thoughtBubble.position.Y -= 50;
                    Game1.world.triggerAIBool = true;
                }
            }

            /*level.thoughtBubble.position = Vector2.Zero;
            level.drewThoughtBubble = false;*/

            level.moved = false;
            level.attacked = false;
            level.rotated = false;
        }
    }
}
