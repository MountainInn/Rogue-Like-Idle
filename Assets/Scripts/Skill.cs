using System;
using Newtonsoft.Json;

public partial class Unit
{
    [JsonObjectAttribute]
    abstract public partial class Skill
    {
        [JsonPropertyAttribute] protected Unit owner;
        [JsonPropertyAttribute] protected Unit target => owner.target;

        [JsonPropertyAttribute] protected float cooldown, t;
        [JsonPropertyAttribute] protected double skillPower;

        public event Action<float> onSkillTick;
        public event Action<int> onLevelUp;
        public event Action onUseSkill;

        public Skill(){}
        public Skill(int maxLevel) : this(null, maxLevel) {}

        public Skill(Unit owner, int maxLevel)
        {
            this.owner = owner;

            this.cooldown = float.NaN;
        }

        public void Tick(float delta)
        {
            t += delta;
            onSkillTick?.Invoke(t);

            if (t < cooldown)
                return;

            t = 0;
            UseSkill();
        }

        abstract public void AdvanceSkill(int level);

        protected void UseSkill()
        {
            ConcreteUseSkill();
           
            onUseSkill?.Invoke();
        }
        abstract protected void ConcreteUseSkill();
    }

    public class SimpleStrike : Unit.Skill
    {
        public new Stat skillPower {get; protected set;}
        Ref<int> levelMult;

        public SimpleStrike()
        {
            levelMult = new Ref<int>(0);
            skillPower = new Stat(.2);
            skillPower.Mult(levelMult);
        }
        public SimpleStrike(int maxLevel) : base(maxLevel) {}

        public override void AdvanceSkill(int level)
        {
            levelMult = level;
            cooldown = MathF.Max(3,  6 - level * .5f);
        }

        protected override void ConcreteUseSkill()
        {
            owner.power += owner.attack.Base * skillPower;
        }
    }


    public class Healing : Unit.Skill
    {
        public Healing() {}

        public Healing(int maxLevel) : base(maxLevel) {}

        public override void AdvanceSkill(int level)
        {
            skillPower = level * 10;
            cooldown = MathF.Max(4, 10 - level);
        }

        protected override void ConcreteUseSkill()
        {
            owner.power += owner.defense.Base * skillPower;
        }
    }

    public class StunBaton : Unit.Skill
    {
        public StunBaton(int maxLevel) : base(maxLevel)
        {
        }

        public override void AdvanceSkill(int level)
        {
            skillPower = level * 0.05f;
            cooldown = MathF.Max(8, 15 - level);
        }

        protected override void ConcreteUseSkill()
        {
            target.defense.AddTemp(owner.attack.Base * skillPower);
            target.attack.AddTemp(owner.attack.Base * skillPower);
        }
    }

}
