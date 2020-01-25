using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Valve.VR;

public class CustomVRController : MonoBehaviour
{
    public static CustomVRController instance;
    
    [SerializeField] private Transform _leftHandTransform;
    [SerializeField] private Transform _rightHandTransform;

    [SerializeField] private float minDifference;
    [SerializeField] private float maxDifference;

    public float rotateValue = 0;

    public bool bothTriggersClicked;

    private bool leftTriggerClicked;
    private bool rightTriggerClicked;
    
    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (CheckDifference(_leftHandTransform.localPosition.z, _rightHandTransform.localPosition.z))
        {
            //Debug.Log("Value: " + DifferenceValue(_leftHandTransform.localPosition.z, _rightHandTransform.localPosition.z));

            rotateValue = DifferenceValue(_leftHandTransform.localPosition.z, _rightHandTransform.localPosition.z);
        }
        else
        {
            //Debug.Log("Value: 0");
            rotateValue = 0;
        }
    }

    private bool CheckDifference(float a, float b)
    {
        return (a - b > minDifference) || (b - a > minDifference);
    }

    private float DifferenceValue(float a, float b)
    {
        return (Mathf.Clamp(a - b, -maxDifference, maxDifference)) / maxDifference;
    }

    public void ChangeTriggerClickedStatus(bool status, SteamVR_Input_Sources handType)
    {
        if (handType == SteamVR_Input_Sources.LeftHand)
        {
            leftTriggerClicked = status;
        }
        else if (handType == SteamVR_Input_Sources.RightHand)
        {
            rightTriggerClicked = status;
        }

        if (rightTriggerClicked && leftTriggerClicked)
        {
            bothTriggersClicked = true;
        }
        else
        {
            bothTriggersClicked = false;
        }
    }
}
