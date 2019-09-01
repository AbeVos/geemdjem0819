using UnityEngine;

namespace mechanics.nutrients
{
    public class Water : Nutrient
    {
        protected override int ResourceMaximum => 50;

        public override int Consume(int amountDemanded)
        {
            var given = Mathf.Min(ResourceStore, amountDemanded);
            ResourceStore = Mathf.Clamp(ResourceStore - given, 0, ResourceMaximum);
            return given;
        }

        private void Awake()
        {
            ResourceStore = 10;
        }
    }
}