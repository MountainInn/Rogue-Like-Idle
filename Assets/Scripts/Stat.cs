using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System;

public partial class Unit
{
    [JsonObjectAttribute]
    public class Stat
    {
        public double Base {get; private set;}
        private List<Ref<double>>
            mults, superMults, tempMults;

        public Ref<double> tempAdditions;
        public Ref<double> Result {get; private set;}

        public event Action<double> onResultChanged;

        public Stat(double Base)
        {
            this.Base = Base;
            this.Result = Base;

            mults = new List<Ref<double>>();
            superMults = new List<Ref<double>>();
            tempMults = new List<Ref<double>>();
            tempAdditions = new Ref<double>(0);
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
        public void AddTemp(double add)
        {
            tempAdditions += add;
        }
        public void ResetTempMultsAndAdditions()
        {
            tempMults.Clear();
            tempAdditions = 0;
            Recalculate();
        }

        private void Recalculate()
        {
            var Product =
                mults
                .Concat(superMults)
                .Concat(tempMults)
                .Aggregate((a , b)=> a*b);

            Result = Base * Product + tempAdditions;

            onResultChanged?.Invoke(Result);
        }

        public bool Contains(Ref<double> mult)
        {
            return mults.Contains(mult) || superMults.Contains(mult) || tempMults.Contains(mult);
        }
        static public implicit operator double(Stat stat) => stat.Result.Value;
    }
}
