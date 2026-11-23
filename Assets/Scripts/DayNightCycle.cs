using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private float _dayDuration = 120;
    [SerializeField] private float _maxIntensity = 1;
    [SerializeField] private Gradient _lightColor;

    private Light _light;

    void Awake()
    {
        _light = GetComponent<Light>();
    }

    void Update()
    {
        transform.Rotate(Vector3.right * (360 / _dayDuration) * Time.deltaTime, Space.World);

        float sunHeight = -transform.forward.y;

        _light.intensity = Mathf.Clamp01(sunHeight) * _maxIntensity;
        _light.color = _lightColor.Evaluate(Mathf.Clamp01(sunHeight));
        RenderSettings.ambientIntensity = Mathf.Clamp(sunHeight, 0.1f, 1);
    }
}