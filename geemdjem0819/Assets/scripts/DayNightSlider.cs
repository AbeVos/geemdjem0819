using UnityEngine;
using UnityEngine.UI;

public class DayNightSlider : MonoBehaviour
{
    public Light directionalLight;
    public Slider slider;

    public Vector3 target;

    private float _previousValue;
    private const float Speed = 1f;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(OnSliderChanged);
    }

    private void Update()
    {
        UpdateLight();
    }

    private void OnSliderChanged(float value)
    {
        var position = directionalLight.transform.position;
        target = new Vector3
        {
            x = position.x + 1,
            y = position.y - 1,
            z = (value - 0.5f) * 2
        };
    }

    private void UpdateLight()
    {
        var step = Speed * Time.deltaTime;

        var rotation = Quaternion.LookRotation(target, Vector3.up);

        directionalLight.transform.rotation = Quaternion.Slerp(directionalLight.transform.rotation, rotation, step);
    }
}
