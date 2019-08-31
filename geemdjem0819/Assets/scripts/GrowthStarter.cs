using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GrowthStarter : MonoBehaviour
{
    public float germinationTime = 5f;
    private Draggable _draggable;
    private Rigidbody _rigidBody;

    private void Start()
    {
        _draggable = gameObject.GetComponent<Draggable>();
        _rigidBody = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (germinationTime <= 0)
        {
            MakeUndraggable();
        }
    }

    private void MakeUndraggable()
    {
        Destroy(_draggable);
        _rigidBody.isKinematic = true;
        Destroy(this);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Soil"))
        {
            germinationTime -= Time.deltaTime * 1f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        germinationTime = 5f; //todo: attach to gameTick
    }
}
