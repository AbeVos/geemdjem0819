using UnityEngine;

public class SimpleMover : MonoBehaviour
{
    protected void Update()
    {
        const float smoothFactor = 1.2f;

        var transformPosition = transform.position;
        if (Input.GetButton("PanUp") && transformPosition.y <= 3)
        {
            var targetPosition = new Vector3(transformPosition.x,transformPosition.y + 0.5f,transformPosition.z);
            transform.position = Vector3.Lerp(transformPosition, targetPosition, Time.deltaTime * smoothFactor);
        }
        else if (Input.GetButton("PanDown")  && transform.position.y >= 0.6)
        {
            var targetPosition = new Vector3(transformPosition.x,transformPosition.y - 0.5f,transformPosition.z);
            transform.position = Vector3.Lerp(transformPosition, targetPosition, Time.deltaTime * smoothFactor);
        }
    }
}
