using UnityEngine;

public class SimpleMover : MonoBehaviour
{
    protected void Update()
    {
        if (Input.GetButton("PanUp") && transform.position.y <= 3)
        {
            transform.Translate(0,0.03f,0);
        }
        else if (Input.GetButton("PanDown")  && transform.position.y >= 0.6)
        {
            transform.Translate(0,-0.03f,0);
        }
    }
}
