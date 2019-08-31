using mechanics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody)),
 RequireComponent(typeof(Draggable)),
 RequireComponent(typeof(GrowController))]
public class GrowthStarter : MonoBehaviour
{
    private Draggable _draggable;
    private Rigidbody _rigidBody;
    private GrowController _growController;

    private void Start()
    {
        _draggable = gameObject.GetComponent<Draggable>();
        _rigidBody = gameObject.GetComponent<Rigidbody>();
        _growController = gameObject.GetComponent<GrowController>();
    }

    private void MakeUndraggable()
    {
        _growController.enabled = true;
        _draggable.enabled = false;
        _rigidBody.isKinematic = true;
        Destroy(this);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Soil"))
        {
           MakeUndraggable();
        }
    }
}
