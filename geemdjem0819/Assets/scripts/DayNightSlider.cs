using UnityEngine;

public class DayNightSlider : MonoBehaviour
{
    public float timeOfDay;
    public int directionalLightMinX;
    public int directionalLightMaxX;
    public Light directionalLight;

    // The target marker.
    public Transform target;

    // Angular speed in radians per sec.
    float speed= 0.5f;

    void Update()
    {
        directionalLight.transform.LookAt(target);
    }
}
