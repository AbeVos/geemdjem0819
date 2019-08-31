using interfaces;

namespace mechanics.nutrients
{
    public class SunLight : Nutrient
    {
        protected override int ResourceMaximum => 0;

        public override int Consume(int amountDemanded)
        {
            return amountDemanded;
        }
    }
}