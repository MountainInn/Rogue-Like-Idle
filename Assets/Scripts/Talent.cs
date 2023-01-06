using System;
using UnityEngine;
using Zenject;

public partial class Unit
{
    [System.Serializable]
    abstract public class Talent
    {
        public string name {get; protected set;}
        protected Unit owner;
        public Level level {get; protected set;}
        public uint gateLevel;

        private TalentPoints talentPoints;

        [Inject]
        public void Construct(TalentPoints talentPoints, Hero hero)
        {
            this.talentPoints = talentPoints;
            this.owner = hero;
        }

        protected Talent(uint gateLevel)
        {
            this.gateLevel = gateLevel;
        }
        protected Talent(string name, uint gateLevel)
        {
            this.name = name;
            this.level = new Level(0);
            this.gateLevel = gateLevel;
        }

        public void LevelUp()
        {
            if (talentPoints <= 0)
                return;

            level.Up();

            ConcreteLevelUp();
        }

        abstract protected void ConcreteLevelUp();


        public class TalentActiveSkill
        {
            public Unit owner {get; protected set;}

            Unit.Skill skill;

            public TalentActiveSkill(Unit owner, Unit.Skill skill)
            {
                this.owner = owner;
                this.skill = skill;
            }

            public void AdvanceSkill(int level)
            {
                if (!owner.activeSkills.Contains(skill))
                    owner.activeSkills.Add(skill);

                skill.AdvanceSkill(level);
            }
        }
    }

    public class TalentMultiplier
    {
        public Unit owner {get; protected set;}

        public Ref<double> mult {get; protected set;}
        private Func<int, double> multFunc;
        private Stat stat;

        public TalentMultiplier(Unit owner, Stat stat , Func<int , double> multFunc)
        {
            this.owner = owner;
            this.stat = stat;
            this.multFunc = multFunc;

            if (!stat.Contains(mult))
                stat.Mult(mult);
        }

        public void UpdateMultiplier(int level)
        {
            mult = multFunc.Invoke(level);
        }
    }

    public class SimpleStrikeTalent : Unit.Talent
    {
        TalentActiveSkill activeSkill;

        public SimpleStrikeTalent(uint gateLevel) : base("Simple Strike", gateLevel)
        {
            activeSkill = new TalentActiveSkill(owner, new Unit.SimpleStrike());
        }

        protected override void ConcreteLevelUp()
        {
            activeSkill.AdvanceSkill(level);
        }
    }

    public class ImprovedSimpleStrikeTalent : Unit.Talent
    {
        Ref<double> mult;

        public ImprovedSimpleStrikeTalent(uint gateLevel) : base("Improved Simple Strike", gateLevel)
        {
            Unit.SimpleStrike simpleStrike = (Unit.SimpleStrike) owner.activeSkills.Find(skill => skill is SimpleStrike);

            mult = 1;
            simpleStrike.skillPower.Mult(mult);
        }

        protected override void ConcreteLevelUp()
        {
            mult = 1 + level * 0.35f;
        }
    }


    public class HealingTalent : Unit.Talent
    {
        TalentActiveSkill talentActiveSkill;

        public HealingTalent(uint gateLevel) : base("Healing", gateLevel)
        {
            talentActiveSkill = new TalentActiveSkill(owner, new Unit.Healing());
        }

        protected override void ConcreteLevelUp()
        {
            talentActiveSkill.AdvanceSkill(level);
        }
    }

    public class ChallengeTalent_Weakness : Unit.Talent
    {
        TalentMultiplier attackMult, defenseMult;

        public ChallengeTalent_Weakness(uint gateLevel) : base("Challenge: Weakness", gateLevel)
        {
            attackMult = new TalentMultiplier(owner, owner.attack, (level) => level * 0.1);
            defenseMult = new TalentMultiplier(owner, owner.defense, (level) => level * 0.1);
        }

        protected override void ConcreteLevelUp()
        {
            attackMult.UpdateMultiplier(level);
            defenseMult.UpdateMultiplier(level);
        }
    }
}
