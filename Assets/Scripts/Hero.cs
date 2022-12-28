using System;
using System.Collections.Generic;

public class Hero : Unit
{
    Expiriense expiriense;

    public Hero(Expiriense expiriense, Level level)
    {
        this.expiriense = expiriense;
        this.level = level;
    }

}
