using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public static VolumeControl _volInstance;

    [SerializeField] string _volumeParamter;
    [SerializeField] AudioMixer _mixer;
    [SerializeField] UnityEngine.UI.Slider _slider;
    [SerializeField] float _multiplier = 30f;
    [SerializeField] Toggle _toggle;
    bool _disableToggleEvent;

    private void Awake()
    {
        _slider.onValueChanged.AddListener(HandleSliderValueChanged);
        _toggle.onValueChanged.AddListener(HandleToggleValueChanged);
    }

    private void HandleToggleValueChanged(bool enableSound)
    {
        if (_disableToggleEvent)
            return;

        if (enableSound)
            _slider.value = _slider.maxValue;
        else
            _slider.value = _slider.minValue;
    }

    private void OnDisable()
    {
        _mixer.SetFloat(_volumeParamter, _slider.value);
    }

    private void HandleSliderValueChanged(float value)
    {
        _mixer.SetFloat(_volumeParamter, Mathf.Log10(value) * _multiplier);
        _disableToggleEvent = true;
        _toggle.isOn = _slider.value > _slider.minValue;
        _disableToggleEvent = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        _volInstance = this;
        _slider.value = PlayerPrefs.GetFloat(_volumeParamter, _slider.value);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateVolume();
    }

    public void UpdateVolume()
    {
        MenusUi.menus.MenuMusicSource.volume = _slider.value;
        MenusUi.menus.MenuSFXSource.volume = _slider.value;
    }
}
