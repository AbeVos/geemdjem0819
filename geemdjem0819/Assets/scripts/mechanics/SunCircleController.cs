using UnityEngine;
using UnityEngine.Serialization;

namespace mechanics
{
    public class SunCircleController : MonoBehaviour
    {
        public float TimeOfday
        {
            get => Mathf.Clamp(_timeOfday, 0, 365);
            set => _timeOfday = Mathf.Clamp(value, 0, 365);
        }
        
        private float _timeOfday = 0;
        [SerializeField] private float sunOffset;
        private float _timeFactor = 0.6f;

        private Quaternion CalculateNewRotation()
        {
            var dayRotation = Quaternion.AngleAxis(_timeOfday, Vector3.right);
            var offsetRotation = Quaternion.AngleAxis(sunOffset, Vector3.forward);
            
            return  offsetRotation * dayRotation;
        }

        private void Update()
        {
            switch (TimeOfday)
            {
                case float n when (n >= 0 && n <= 180):
                    TimeOfday += _timeFactor*4;
                    break;

                case float n when (n >= 180 && n <= 360):
                    TimeOfday += _timeFactor;
                    break;

                case float n when (n >= 360):
                    TimeOfday = 0;
                    break;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, CalculateNewRotation(), Time.deltaTime);

        }
    }
}
