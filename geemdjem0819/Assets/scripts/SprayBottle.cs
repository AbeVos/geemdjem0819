using System.Collections.Generic;
using interfaces;
using mechanics.nutrients;
using UnityEngine;

public class SprayBottle : MonoBehaviour, ILooker, ITickable
{
    public GameObject target;
    public GameObject plant; //todo: implement this in a way that finds the plant in the scene
    private Vector3 _startTransform;
    private Rigidbody _rigidBody;
    public bool isLooking;
    private Queue<int> _waterToReplenish;

    private void Start()
    {
        _startTransform = transform.position;
        _rigidBody = GetComponent<Rigidbody>();
        _waterToReplenish = new Queue<int>();
    }

    private void Update()
    {

        if (isLooking)
        {
            transform.LookAt(target.transform);
            if (Input.GetMouseButtonDown(1) && _waterToReplenish.Count < 5)
            {
                // check if plant has water script
                _waterToReplenish.Enqueue(1);
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

    public void Tick()
    {
        if (!isActiveAndEnabled || _waterToReplenish.Count == 0) return;
        
        Debug.Log(_waterToReplenish.Peek());
        plant.GetComponent<Water>().Replenish(_waterToReplenish.Dequeue());
    }
}
