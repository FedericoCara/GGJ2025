using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Transform _fishFace;
    [SerializeField] private float _fishFaceBaseScale;
    [SerializeField] private float _fishFaceMaxScaleIncrease;
    [SerializeField] private Image _oxygenBar;
    [SerializeField] private Transform _oxygenArrow;
    [SerializeField] private float _oxygenArrowEmptyAngle;
    [SerializeField] private float _oxygenArrowFullAngle;
    [SerializeField] private TextMeshProUGUI _keysAmountText;

    private void OnEnable()
    {
        PlayerStats.OnOxygenChanged += UpdateOxygen;
        PlayerStats.OnKeysChanged += UpdateKeys;
    }

    private void OnDisable()
    {
        PlayerStats.OnOxygenChanged -= UpdateOxygen;
        PlayerStats.OnKeysChanged -= UpdateKeys;
    }

    private void Start()
    {
        UpdateOxygen(0);
        UpdateKeys(0);
    }

    private void UpdateOxygen(float newOxygenPercentage)
    {
        _fishFace.localScale = (_fishFaceBaseScale + newOxygenPercentage * _fishFaceMaxScaleIncrease) * Vector3.one;
        float zRotation = Mathf.Lerp(_oxygenArrowEmptyAngle, _oxygenArrowFullAngle, newOxygenPercentage);
        _oxygenArrow.rotation = Quaternion.Euler(0, 0, zRotation);

    }

    private void UpdateKeys(int newKeysCount)
    {
        _keysAmountText.text = newKeysCount.ToString();
    }

}
