using UnityEngine;

namespace interfaces
{
    public abstract class Nutrient : MonoBehaviour
    {
        protected virtual int ResourceStore { get; set; }
        protected virtual int ResourceMaximum => 10;

        /// <summary>
        /// The percentage of nutrient available mapped between (0,1) 
        /// </summary>
        public float FiledAmount => Mathf.Clamp01((float) ResourceStore / ResourceMaximum);

        /// <summary>
        /// This returns the maximum resource available
        /// </summary>
        /// <param name="amountDemanded">A consumer can request an amount</param>
        public abstract int Consume(int amountDemanded);

        /// <summary>
        /// This can be used by a donor to fill the _resourceStore
        /// </summary>
        /// <param name="amountGiven">A donor can give as specified amount.
        /// This can be more than the maximum. In that case the store will not fill more</param>
        public void Replenish(int amountGiven)
        {
            ResourceStore = Mathf.Clamp(ResourceStore + amountGiven, 0, ResourceMaximum);
        }
    }
}