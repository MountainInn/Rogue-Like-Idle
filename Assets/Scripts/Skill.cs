using System;
using Newtonsoft.Json;

public partial class Unit
{
    [JsonObjectAttribute]
    abstract public partial class Skill
    {
        [JsonPropertyAttribute] protected Unit owner;

        [JsonPropertyAttribute] protected float cooldown, t;
        [JsonPropertyAttribute] protected int level, maxLevel;
        [JsonPropertyAttribute] protected double skillPower;

        public event Action<float> onSkillTick;
        public event Action<int> onLevelUp;

        public Skill(int maxLevel) : this(null, maxLevel) {}

        public Skill(Unit owner, int maxLevel)
        {
            this.owner = owner;
            this.level = 1;
            this.maxLevel = maxLevel;

            this.cooldown = float.NaN;
            AdvanceSkill();
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

        public void Levelup()
        {
            level++;
            AdvanceSkill();
            onLevelUp?.Invoke(level);
        }

        abstract protected void AdvanceSkill();

        abstract protected void UseSkill();
    }

    public class SimpleStrike : Unit.Skill
    {
        public SimpleStrike(int maxLevel) : base(maxLevel)
        {
        }

        protected override void AdvanceSkill()
        {
            skillPower = level * 0.1f;
            cooldown = MathF.Max(3,  6 - level);
        }

        protected override void UseSkill()
        {
            // owner.attack += owner.baseDamage * skillPower;
        }
    }

    public class ImprovedSimpleStrike : Unit.Skill
    {
        Unit.Skill previousVersion;

        public ImprovedSimpleStrike(int maxLevel) : base(maxLevel)
        {
        }

        protected override void AdvanceSkill()
        {
            skillPower = level * 0.1f;

            var simpleStrike = owner.activeSkills.Find(skill => skill is SimpleStrike);
            // simpleStrike.skillPower = simpleStrike.skillPower + skillPower;
        }

        protected override void UseSkill()
        {
        }
    }

    public class Healing : Unit.Skill
    {
        private Ref<double> mult;

        public Healing(int maxLevel) : base(maxLevel)
        {
            // owner.defense.
        }

        protected override void AdvanceSkill()
        {
            mult = skillPower = level * 10;
            cooldown = MathF.Max(4, 10 - level);
        }

        protected override void UseSkill()
        {
            // owner.defense += owner.baseDefense * skillPower;
        }
    }

    public class StunBaton : Unit.Skill
    {
        public StunBaton(int maxLevel) : base(maxLevel)
        {
        }

        protected override void AdvanceSkill()
        {
            skillPower = level * 0.05f;
            cooldown = MathF.Max(8, 15 - level);
        }

        protected override void UseSkill()
        {
            // owner.target.defense
        }
    }


    public class ChallengeSkill_Weakness : Unit.Skill
    {
        private Ref<double> multiplier;

        public ChallengeSkill_Weakness(int maxLevel) : base(maxLevel)
        {
        }

        protected override void AdvanceSkill()
        {
            skillPower = 0.05 * level;
            multiplier = skillPower;
        }

        protected override void UseSkill()
        {
            owner.defense.MultSuper(multiplier);
            owner.attack.MultSuper(multiplier);
        }
    }

}
