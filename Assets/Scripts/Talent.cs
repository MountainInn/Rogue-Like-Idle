using UnityEngine;

public partial class Unit
{
    [System.Serializable]
    public class Talent
    {
        [SerializeField] public Talent ancestor;
        [SerializeField] public uint ancestorLevelRequirement;

        public string name {get; protected set;}
        protected Unit owner;
        public Level level {get; protected set;}

        public Talent()
        {
        }

        public Talent(string name)
        {
            this.name = name;
            this.level = new Level(0);
        }

        public Talent ConstructRequirement(
            Unit.Talent ancestor,
            uint ancestorLevelRequirement)
        {
            this.ancestor = ancestor;
            this.ancestorLevelRequirement = ancestorLevelRequirement;
            return this;
        }
    }

    public class TestTalent1 : Talent
    {
        public TestTalent1()
        {
            this.name = "Test Talent 1";
        }
    }
    public class TestTalent2 : Talent
    {

        public TestTalent2()
        {
            this.name = "Test Talent 2";
        }
       
    }
}
