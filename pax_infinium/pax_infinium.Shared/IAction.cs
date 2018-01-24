using System;
using System.Collections.Generic;
using System.Text;

namespace pax_infinium
{
    interface IAction
    {
        bool canUseOnSelf();

        bool canUseOnAlly();

        bool canUseOnEnemy();

        List<Cube> areaOfEffect(Cube target);

        int chance(Character target);
    }
}
