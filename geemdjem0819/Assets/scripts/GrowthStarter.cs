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
            OkDoei();
        }
    }

    private void OkDoei()
    {
        Destroy(draggable);
        rigidBody.isKinematic = true;
        Destroy(this);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Soil")
        {
            germinationTime -= Time.deltaTime * 1f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        germinationTime = 5f;
    }
}
