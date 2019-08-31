using UnityEngine;
using UnityEngine.UI;

public class DayNightSlider : MonoBehaviour
{
    public Light directionalLight;
    public Slider slider;

    public Vector3 target;

    private float previousValue;
    private float speed = 1f;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(OnSliderChanged);
    }

    void Update()
    {
        UpdateLight();
    }
    void OnSliderChanged(float value)
    {
        target = new Vector3
        {
            x = directionalLight.transform.position.x + 1,
            y = directionalLight.transform.position.y - 1,
            z = (value - 0.5f) * 2
        };
    }

    void UpdateLight()
    {
        float step = speed * Time.deltaTime;

        Quaternion rotation = Quaternion.LookRotation(target, Vector3.up);
        directionalLight.transform.rotation = Quaternion.Slerp(directionalLight.transform.rotation, rotation, step);
    }
}
