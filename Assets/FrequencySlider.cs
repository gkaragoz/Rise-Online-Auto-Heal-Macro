using UnityEngine;
using UnityEngine.UI;

public class FrequencySlider : SingletonMonoBehaviour<FrequencySlider>
{
    [SerializeField] private Slider _slider;

    public float GetValue()
    {
        return _slider.value;
    }
}
