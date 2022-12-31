using System;
using System.Collections.Generic;
using MountainInn;
using Newtonsoft.Json;
using System.Linq;

[JsonObjectAttribute]
public partial class Unit
{
    [JsonPropertyAttribute] public Level level;
    [JsonPropertyAttribute] public double power;
    [JsonPropertyAttribute] public Stat defense, attack;

    [JsonPropertyAttribute] public Team enemyTeam;
    [JsonPropertyAttribute] public Unit target;

    [JsonPropertyAttribute] private List<Skill> activeSkills;
    public event Action<Unit> onUnitDied;

    public Unit(double baseDefense, double baseDamage, List<Skill> activeSkills = null)
    {
        this.baseDefense = baseDefense;
        this.baseDamage = baseDamage;
        this.activeSkills = activeSkills;
    }

    public void Attack(float delta)
    {
        target.power -= damage * delta;
    }
    public void Defend(float delta)
    {
        power += defense * delta;
    }
    public void UseSkills(float delta)
    {
        activeSkills.ForEach(skill => skill.Tick(delta));
    }

    public void SelectTarget()
    {
        target = enemyTeam.units.GetRandom();
    }

    public void CheckYourCondition()
    {
        if (power <= 0)
        {
            power = 0;
            onUnitDied?.Invoke(this);
        }
    }


    abstract public partial class Skill
    {
    }

    [JsonObjectAttribute]
    public class Stat
    {
        private double Base;
        private List<Ref<double>>
            mults, superMults, tempMults;

        public Ref<double> Result {get; private set;}

        public Stat(double Base)
        {
            this.Base = Base;
            this.Result = Base;
        }

        public void Mult(double mult)
        {
            mults.Add(mult);
            Recalculate();
        }

        public void MultSuper(double mult)
        {
            superMults.Add(mult);
            Recalculate();
        }

        public void MultTemp(double mult)
        {
            tempMults.Add(mult);
            Recalculate();
        }
        public void ResetTempMults()
        {
            tempMults.Clear();
            Recalculate();
        }

        private void Recalculate()
        {
            var Product =
                mults
                .Concat(superMults)
                .Concat(tempMults)
                .Aggregate((a , b)=> a*b);

            Result = Base * Product;
        }

        static public implicit operator double(Stat stat) => stat.Result.Value;
    }
}

public class Ref<T>
    where T : unmanaged
{

    public T Value {get ; private set; }

    public Ref(T value)
    {
        Value = value;
    }

    static public implicit operator T(Ref<T> reft) => reft.Value;
    static public implicit operator Ref<T>(T t)
    {
        return new Ref<T>(t);
    }
}
