using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Item : MonoBehaviour
{
    public string item_name;
    public int id;
    public GameObject item_prefab;
    public bool is_held =false;
    private bool rightHandLastState = false;
    public bool passive_active = false;



    public InputDeviceCharacteristics controllerCharacteristics;
    private InputDevice targetDevice;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevices(devices);
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);
        if (devices.Count > 0)
        {
            targetDevice = devices[0];
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        bool buttonValue; 

        targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out buttonValue);

        if(buttonValue != rightHandLastState && is_held == true)
        {
            if (buttonValue == true)
            {
                //pressed this frame
            }
            else if (buttonValue == false) 
            {
                //released this frame
                Debug.Log("Pressing primary for " + targetDevice.name);
                Action();
            }

            rightHandLastState = buttonValue;
        }
        //false

        update_passive();
    }

    public virtual void Action() {}


    public virtual void passiveEffect()
    {
        //Here the item grants a passive effect on the room
    }

    public virtual void removePassiveEffect()
    {
        //Removes passive effects of an item
    }

    public virtual void update_passive() 
    {
        //Activate passive effect if item is not being held
        if (!is_held && !passive_active)
        {
            passive_active = true;
            passiveEffect();
        }

        //if item is held again remove passive effect
        if (is_held && passive_active)
        {
            passive_active = false;
            removePassiveEffect();
        }

    }
}
