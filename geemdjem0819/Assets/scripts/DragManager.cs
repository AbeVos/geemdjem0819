using interfaces;
using UnityEngine;

public class DragManager : MonoBehaviour
{
    private Camera _camera;
    private bool _isDraggingAnObject;
    private Rigidbody _draggableRigidBody;

    private ILooker _lookScript;
    private IDraggable _dragScript;

    private void Awake()
    {
        _camera = Camera.main;
    }

    protected void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (!_isDraggingAnObject)
            {
                var foundDraggableRigidBody = CheckForDraggableRigidBody(out _draggableRigidBody);
                if (!foundDraggableRigidBody) return;
                
                StartDragging();
            }
        }
        if (Input.GetMouseButton(0) && _isDraggingAnObject)
        {
            Drag();
        }

        if (Input.GetMouseButtonUp(0))
        {
           StopDragging();
        }
    }

    private bool CheckForDraggableRigidBody(out Rigidbody foundRb)
    {
        foundRb = null;
        var hit = Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out var hitInfo);

        if (hit && hitInfo.rigidbody )
        {
            _dragScript = hitInfo.transform.GetComponent<IDraggable>();
            if (_dragScript != null && _dragScript.IsDraggable)
            {
                foundRb = hitInfo.rigidbody; 
                return true;
            }
        }
        return false;
    }

    private Vector3 GetMouseWorldSpacePosition()
    {
        var mouseWorldPosition = Input.mousePosition;
        mouseWorldPosition.z = 4; //todo: design non-crappy solution
        mouseWorldPosition = _camera.ScreenToWorldPoint(mouseWorldPosition);
        return mouseWorldPosition;
    }
    
    
    private void StartDragging()
    {
        _isDraggingAnObject = true;
        _dragScript.SetKinematic(true);
        _lookScript = _draggableRigidBody.GetComponent<ILooker>();
        _lookScript?.StartLooking();
    }

    private void Drag()
    {
        _draggableRigidBody.MovePosition(GetMouseWorldSpacePosition());
    }

    private void StopDragging()
    {
        if (_draggableRigidBody == null) return;
        
        _dragScript.SetKinematic(false);
        _lookScript?.StopLooking();

        _draggableRigidBody = null;
        _isDraggingAnObject = false;
    }
}
