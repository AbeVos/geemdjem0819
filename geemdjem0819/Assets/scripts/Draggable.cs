using System;
using interfaces;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Draggable : MonoBehaviour, IDraggable
{
    private Rigidbody _rigidBody;
    public bool IsDraggable { get; set; }

    protected void Awake()
    {
        IsDraggable = true;
        _rigidBody = GetComponent<Rigidbody>();
    }

    public void SetKinematic(bool startDragging)
    {
        _rigidBody.isKinematic = startDragging;
    }
}
