using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR;
using UnityEngine;

//Manages user hand animation
public class HandPresense : MonoBehaviour
{
    private InputDevice targetDevice;
    public InputDeviceCharacteristics controllerCharacteristics;
    public GameObject handModelPrefab;

    private GameObject spawnedHandModel;
    private Animator handAnimator;
    private GameObject player;

    void Start()
    {
        player = GameObject.Find("VRig");
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevices(devices);
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);


        foreach(var i in devices) {
            Debug.Log(i.name + i.characteristics);
        }
        if (devices.Count > 0)
        {   
            targetDevice = devices[0];

            spawnedHandModel = Instantiate(handModelPrefab, transform);
            handAnimator = spawnedHandModel.GetComponent<Animator>();
        }
    }

    void UpdateAnimation() 
    {
        if(targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue)) 
        {
            handAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            handAnimator.SetFloat("Trigger", 0);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);
        //if (primaryButtonValue)
        //{
        //    Debug.Log("Pressing primary for " + targetDevice.name);
        //}

        //targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue);
        //if (triggerValue > 0.1f) 
        //{
        //    Debug.Log("Trigger Value: " + triggerValue + " for " + targetDevice.name);
        //}

        //targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxis);

        //if (primary2DAxis != Vector2.zero) 
        //{
        //    Debug.Log("Moving joystick for" + targetDevice.name);
        //}

        UpdateAnimation();
    }
}
