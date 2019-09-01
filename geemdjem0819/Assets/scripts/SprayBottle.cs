using interfaces;
using UnityEngine;

public class SprayBottle : MonoBehaviour, ILooker
{
    public GameObject target;
    private Vector3 _startTransform;
    private Rigidbody _rigidBody;
    public bool isLooking;

    void Start()
    {
        _startTransform = transform.position;
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (isLooking)
        {
            transform.LookAt(target.transform);
            if (Input.GetMouseButton(1))
            {
                // water the plant
            }
        }
        else if (!isLooking)
        {
            var myTransform = transform;
            transform.position = Vector3.Lerp(myTransform.position, 
                                                _startTransform,
                                                Time.deltaTime * 5);
            

            var transformRotation = myTransform.rotation;
           myTransform.rotation = new Quaternion(transformRotation.x,
                                              180,
                                              transformRotation.z,
                                              0);
        }
    }
    
    public void StartLooking()
    {
        isLooking = true;
        _rigidBody.isKinematic = true;
    }

    public void StopLooking()
    {
        isLooking = false;
        _rigidBody.isKinematic = true;
    }
}
