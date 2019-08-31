using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GrowthStarter : MonoBehaviour
{
    public float germinationTime = 5f;
    public Draggable draggable;
    public Rigidbody rigidBody;

    private void Start()
    {
        draggable = gameObject.GetComponent<Draggable>();
        rigidBody = gameObject.GetComponent<Rigidbody>();
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
        Destroy(draggable);
        rigidBody.isKinematic = true;
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
