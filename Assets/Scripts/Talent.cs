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
        public void Construct(// TalentPoints talentPoints,
            Hero hero)
        {
            // this.talentPoints = talentPoints;
            hero.onHeroInitialized += (hero)=> this.owner = hero;
        }

        protected Talent() { }
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

    public class TalentMultiplier : IInitializable
    {
        public Ref<double> mult {get; protected set;}
        private Func<int, double> multFunc;
        private Stat stat;

        public TalentMultiplier(Stat stat, Func<int, double> multFunc)
        {
            this.stat = stat;
            this.multFunc = multFunc;
            this.mult = 1;
        }

        public void UpdateMultiplier(int level)
        {
            mult = multFunc.Invoke(level);
        }

        public void Initialize()
        {
            if (!stat.Contains(mult))
                stat.Mult(mult);
        }
    }

    public class SimpleStrikeTalent : Unit.Talent, IInitializable
    {
        TalentActiveSkill activeSkill;

        public SimpleStrikeTalent(uint gateLevel) : base("Simple Strike", gateLevel)
        {
        }

        public void Initialize()
        {
            activeSkill = new TalentActiveSkill(owner, new Unit.SimpleStrike());
        }

        protected override void ConcreteLevelUp()
        {
            activeSkill.AdvanceSkill(level);
        }
    }

    public class ImprovedSimpleStrikeTalent : Unit.Talent, IInitializable
    {
        Unit.SimpleStrike simpleStrike;
        TalentMultiplier multiplier;

        public ImprovedSimpleStrikeTalent(uint gateLevel) : base("Improved Simple Strike", gateLevel) {}

        public void Initialize()
        {
            simpleStrike = owner.activeSkills.Find(skill => skill is SimpleStrike) as SimpleStrike;

            multiplier = new TalentMultiplier(
                simpleStrike.skillPower,
                (level) => 1 + level * .35);
        }

        protected override void ConcreteLevelUp()
        {
            multiplier.UpdateMultiplier(level);
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

    public class ChallengeTalent_Weakness : Unit.Talent, IInitializable
    {
        TalentMultiplier attackMult, defenseMult;

        public ChallengeTalent_Weakness() : base() { }
        public ChallengeTalent_Weakness(uint gateLevel) : base("Challenge: Weakness", gateLevel) { }

        public void Initialize()
        {
            attackMult = new TalentMultiplier(
                owner.attack,
                (level) => level * 0.1
            );
            defenseMult = new TalentMultiplier(
                owner.defense,
                (level) => level * 0.1
            );
        }

        protected override void ConcreteLevelUp()
        {
            attackMult.UpdateMultiplier(level);
            defenseMult.UpdateMultiplier(level);
        }
    }
}
