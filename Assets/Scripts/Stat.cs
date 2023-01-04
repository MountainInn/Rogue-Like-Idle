using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System;

public partial class Unit
{
    [JsonObjectAttribute]
    public class Stat
    {
        private double Base;
        private List<Ref<double>>
            mults, superMults, tempMults;

        public Ref<double> Result {get; private set;}

        public event Action<double> onResultChanged;

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

            onResultChanged?.Invoke(Result);
        }

        static public implicit operator double(Stat stat) => stat.Result.Value;
    }
}
