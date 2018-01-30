using System;
using System.Collections.Generic;
using System.Text;

namespace pax_infinium.Actions
{
    class BasicMeleeAttack
    {
        List<Cube> potentialTargets(Character activeCharacter, Level level)
        {
            List<Cube> cubeList = new List<Cube>();
            foreach (Cube cube in level.grid.cubes)
            {
                if (cube.isAdjacent(activeCharacter.gridPos))
                {
                    cubeList.Add(cube);
                }
            }
            return cubeList;
        }

        void DoAction(Character activeCharacter, Cube target, Level level)
        {
            int[] chanceDamage;
            foreach (Character character in level.grid.characters.list)
            {
                if (target.gridPos == character.gridPos)
                {
                    chanceDamage = activeCharacter.calculateAttack(character);
                    level.SetConfirmationText(chanceDamage[0], chanceDamage[1]);
                }
            }
        }
    }
}
