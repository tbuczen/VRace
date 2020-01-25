using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class TriggerBehav : MonoBehaviour
{
    public SteamVR_Action_Boolean TriggerClick;

    public SteamVR_Action_Single TriggerFloat;

    public SteamVR_Input_Sources handType;

    void Start()
    {
        TriggerClick.AddOnStateDownListener(TriggerDown, handType);
        TriggerClick.AddOnStateUpListener(TriggerUp, handType);
        
        TriggerFloat.AddOnAxisListener(TriggerFloatValue,handType);
    }

    public void TriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        //Debug.Log("Trigger is up " + handType);

        CustomVRController.instance.ChangeTriggerClickedStatus(false,handType);
    }

    public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        //Debug.Log("Trigger is down " + handType);
        
        CustomVRController.instance.ChangeTriggerClickedStatus(true,handType);
    }

    private void TriggerFloatValue(SteamVR_Action_Single fromAction, SteamVR_Input_Sources fromSource, float newAxis, float newDelta)
    {
        //Debug.Log(handType + " newAxis: " + newAxis + " newDelta: " + newDelta);
    }
}