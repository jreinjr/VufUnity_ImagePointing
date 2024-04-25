using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceEyeCalibration : MonoBehaviour
{
    [SerializeField] Transform leftEye;
    [SerializeField] Transform rightEye;

    [SerializeField] Transform leftDisplay;
    [SerializeField] Transform rightDisplay;

    [SerializeField] float _baseIPD = .07f;
    [SerializeField] float _baseVerticalOffset = 0f;
    [SerializeField] float _baseEyeRelief = .03f;

    [SerializeField] float adjustmentSpeed = .005f;

    [SerializeField] Vector2 ipdRange = new Vector2(.03f, .1f);
    [SerializeField] Vector2 verticalOffsetRange = new Vector2(-0.1f, .01f);
    [SerializeField] Vector2 eyeReliefRange = new Vector2(0.01f, .05f);

    private float currentIPD;
    private float currentVerticalOffset;
    private float currentEyeRelief;

    // Start is called before the first frame update
    void Start()
    {
        bool hasExistingCalibration = PlayerPrefs.HasKey("calibration_IPD");

        if (hasExistingCalibration)
        {
            Debug.Log("No existing device eye calibration");
            currentIPD = _baseIPD;
            currentVerticalOffset = _baseVerticalOffset;
            currentEyeRelief = _baseEyeRelief;
        }
        else
        {
            Debug.Log("Device eye calibration found");
            currentIPD = PlayerPrefs.GetFloat("calibration_IPD");
            currentVerticalOffset = PlayerPrefs.GetFloat("calibration_verticalOffset");
            currentEyeRelief = PlayerPrefs.GetFloat("calibration_eyeRelief");
        }

        SetEyeReliefFromSlider(0.5f);
        SetIPDFromSlider(0.5f);
        SetVerticalOffsetFromSlider(0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEyePositions();
    }

    public void AdjustIPD(float amount)
    {
        currentIPD = Mathf.Clamp(currentIPD + amount * adjustmentSpeed,
            ipdRange.x, ipdRange.y);
    }

    public void AdjustVerticalOffset(float amount)
    {
        currentVerticalOffset = Mathf.Clamp(currentVerticalOffset + amount * adjustmentSpeed,
            verticalOffsetRange.x, verticalOffsetRange.y);
    }

    public void AdjustEyeRelief(float amount)
    {
        currentEyeRelief = Mathf.Clamp(currentEyeRelief + amount * adjustmentSpeed,
            eyeReliefRange.x, eyeReliefRange.y);
    }

    public void SetIPDFromSlider(float value)
    {
        currentIPD = Mathf.Lerp(ipdRange.x, ipdRange.y, value);
    }

    public void SetVerticalOffsetFromSlider(float value)
    {
        currentVerticalOffset = Mathf.Lerp(verticalOffsetRange.x, verticalOffsetRange.y, value);
    }

    public void SetEyeReliefFromSlider(float value)
    {
        currentEyeRelief = Mathf.Lerp(eyeReliefRange.x, eyeReliefRange.y, value);
    }

    public void SaveCalibration()
    {
        PlayerPrefs.SetFloat("calibration_IPD", currentIPD);
        PlayerPrefs.SetFloat("calibration_verticalOffset", currentVerticalOffset);
        PlayerPrefs.SetFloat("calibration_eyeRelief", currentEyeRelief);
    }

    private void UpdateEyePositions()
    {
        leftEye.localPosition = new Vector3(
            currentIPD * -0.5f,
            currentVerticalOffset,
            leftDisplay.localPosition.z - currentEyeRelief);

        rightEye.localPosition = new Vector3(
            currentIPD * 0.5f,
            currentVerticalOffset,
            rightDisplay.localPosition.z - currentEyeRelief);
    }

}
