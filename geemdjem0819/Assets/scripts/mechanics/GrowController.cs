using interfaces;
using UnityEngine;

namespace mechanics
{
    public class GrowController : MonoBehaviour, ITickable
    {
        [Tooltip("The total number of cycles the plant needs to reach maturity")]
        public int growCyclesTillSeed = 20;

        [Tooltip("The total number of cycles the plant needs before it starts to grow")]
        public int germinationCycles = 5;

        private Nutrient[] _nutrients;
        private int _currentGrowth;
        private int _currentHealth;
        private float _life;

        /// <summary>
        /// The health of the plant. When this is 0 the plant wil not grow anymore. Mapped between (0,1)
        /// </summary>
        public float Life
        {
            get => Mathf.Clamp01(_life);
            private set => _life = value;
        }

        /// <summary>
        /// Returns true when Life is 0.
        /// </summary>
        public bool IsDead => Mathf.Approximately(Life, 0);

        /// <summary>
        /// Send and update to the plant. It will calculate its growth based on environment inputs
        /// </summary>
        public void Tick()
        {
            if (IsDead) return;
            
            _currentGrowth++;

            const int demand = 1;
            foreach (var nutrient in _nutrients)
            {
                if (demand != nutrient.Consume(demand))
                {
                    Life -= 0.1f;
                    //Debug.Log("AAAAAH IK HEB HONGER OF DORST OF WAT PLANTEN DAN OOK HEBBEN!!!! ");
                }
            }

            if (_currentGrowth >= germinationCycles)
            {
                // TODO: Start grow visuals
            }

            if (_currentGrowth >= growCyclesTillSeed)
            {
                // TODO: SEEDS
            }
        }

        private void Awake()
        {
            _currentGrowth = 0;
            Life = 1;

            var nutrients = gameObject.GetComponents<Nutrient>();
            _nutrients = new Nutrient[nutrients.Length];
            for (var index = 0; index < nutrients.Length; index++)
            {
                _nutrients[index] = nutrients[index];
            }
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.P))
            {
                Debug.Log($"{gameObject.name} has the following stats: " +
                          $"Life: {Life}, Growth: {_currentGrowth}, IsDead: {IsDead}");
            }
        }
    }
}