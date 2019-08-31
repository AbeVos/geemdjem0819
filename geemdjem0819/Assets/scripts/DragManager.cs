using UnityEngine;

public class DragManager : MonoBehaviour
{
    public float catchingDistance = 3f;
    public GameObject draggingObject;
    private bool isDragging { get; set; }

    protected void Start()
    {
        isDragging = false;
    }

    protected void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (!isDragging)
            {
                draggingObject = GetObjectFromMouseRaycast();
                if (draggingObject
                    && draggingObject.GetComponent<IDraggable>() != null
                    && draggingObject.GetComponent<IDraggable>().isDraggable == true)
                {
                    draggingObject.GetComponent<Rigidbody>().isKinematic = true;
                    isDragging = true;
                }
            }
            else if (draggingObject != null)
            {
                draggingObject.GetComponent<Rigidbody>().MovePosition(CalculateMouse3DVector());
            }
        }
        else
        {
            if (draggingObject != null)
            {
                draggingObject.GetComponent<Rigidbody>().isKinematic = false;
            }
            isDragging = false;
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
    public Vector3 CalculateMouse3DVector()
    {
        Vector3 v3 = Input.mousePosition;
        v3.z = catchingDistance;
        v3 = Camera.main.ScreenToWorldPoint(v3);
        //Debug.Log(v3); //Current Position of mouse in world space
        return v3;
    }
}
