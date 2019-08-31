using UnityEngine;

public class DragManager : MonoBehaviour
{
    public float catchingDistance = 3f;
    public GameObject draggingObject;
    private bool isDraggingAnObject { get; set; }

    protected void Start()
    {
        isDraggingAnObject = false;
    }

    protected void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (!isDraggingAnObject)
            {
                draggingObject = GetObjectFromMouseRaycast();
                if (draggingObject && draggingObject?.GetComponent<IDraggable>().isDraggable == true)
                {
                    draggingObject.GetComponent<Rigidbody>().isKinematic = true;
                    isDraggingAnObject = true;
                }
            }
            else if (draggingObject != null)
            {
                draggingObject.GetComponent<Rigidbody>().MovePosition(GetMouseWorldSpacePosition());
            }
        }
        else
        {
            if (draggingObject != null)
            {
                draggingObject.GetComponent<Rigidbody>().isKinematic = false;
            }
            isDraggingAnObject = false;
        }
    }

    public GameObject GetObjectFromMouseRaycast()
    {
        GameObject gameObject = null;
        bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo);
        if (hit)
        {
            if (hitInfo.collider.gameObject.GetComponent<Rigidbody>() &&
                Vector3.Distance(hitInfo.collider.gameObject.transform.position,
                transform.position) <= catchingDistance)
            {
                gameObject = hitInfo.collider.gameObject;
            }
        }
        return gameObject;
    }
    public Vector3 GetMouseWorldSpacePosition()
    {
        Vector3 mouseWorldPosition = Input.mousePosition;
        mouseWorldPosition.z = catchingDistance;
        mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseWorldPosition);
        return mouseWorldPosition;
    }
}
