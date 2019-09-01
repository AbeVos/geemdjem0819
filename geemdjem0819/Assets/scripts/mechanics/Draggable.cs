using interfaces;
using UnityEngine;

namespace mechanics
{
    public class Draggable : MonoBehaviour, IDraggable
    {
        public bool isDraggable { get; set; }

        protected void Awake()
        {
            isDraggable = true;
        }
    }
}
