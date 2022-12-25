using System.Collections.Generic;

public class Unit
{
    public double power;

    public void Tick()
    {
        activeSkills.ForEach(skill => skill.Tick(delta));
    }

    abstract public class Skill
    {
        protected Unit unit;

        abstract public void Tick(float delta);
    }
}
