using interfaces;
using UnityEngine;

namespace managers
{
    public class DragManager : MonoBehaviour
    {
        public float catchingDistance = 3f;
        private Camera _camera;
        private bool IsDraggingAnObject { get; set; }

        private Rigidbody _draggingRigidBody;
        private bool _foundRigidBody;

        private void Awake()
        {
            _camera = Camera.main;
        }

        protected void Start()
        {
            IsDraggingAnObject = false;
        }

        protected void Update()
        {
            if (Input.GetMouseButton(0))
            {
                if (!IsDraggingAnObject)
                {
                    _foundRigidBody = GetObjectFromMouseRaycast(out _draggingRigidBody);
                    if (!_foundRigidBody) return;
                
                
                    var isDraggable = _draggingRigidBody.GetComponent<IDraggable>();
                    if (isDraggable != null && isDraggable.isDraggable)
                    {
                        _draggingRigidBody.isKinematic = true;
                        IsDraggingAnObject = true;
                    }
                }
                else if (_foundRigidBody)
                {
                    _draggingRigidBody.MovePosition(GetMouseWorldSpacePosition());
                }
            }
            else
            {
                if (_foundRigidBody)
                {
                    _draggingRigidBody.isKinematic = false;
                }
                IsDraggingAnObject = false;
                _foundRigidBody = false;
            }
        }

        private bool GetObjectFromMouseRaycast(out Rigidbody foundRb)
        {
            foundRb = null;
            var hit = Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out var hitInfo);
        
            if (hit && hitInfo.rigidbody && Vector3.Distance(hitInfo.point, transform.position) <= catchingDistance)
            {
                foundRb = hitInfo.rigidbody;
                return true;
            }
            return false;
        }

        private Vector3 GetMouseWorldSpacePosition()
        {
            var mouseWorldPosition = Input.mousePosition;
            mouseWorldPosition.z = catchingDistance;
            mouseWorldPosition = _camera.ScreenToWorldPoint(mouseWorldPosition);
            return mouseWorldPosition;
        }
    }
}
