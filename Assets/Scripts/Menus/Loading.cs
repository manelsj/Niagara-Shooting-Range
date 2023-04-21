using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [SerializeField]
    private Slider loadingSlider;


    public void OnEnable()
    {
        loadingSlider.value = 0;
    }

    public void UpdateSlider(float prog)
    {
        float progress = Mathf.Clamp01(prog / .9f);
        loadingSlider.value = progress;
    }
}
