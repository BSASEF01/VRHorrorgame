using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandRay : MonoBehaviour
{
    public InputDeviceCharacteristics controllerCharacteristics;
    public float line_size;

    private bool is_hit;
    private LineRenderer lineRenderer;
    private RaycastHit latest_hit;
    private InputDevice targetDevice;

   
    void Start()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.endWidth = line_size;
        lineRenderer.startWidth = 0;

        //get controller
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevices(devices);
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);
        if (devices.Count > 0)
        {
            targetDevice = devices[0];
        }
    }


    //Special Interactables
    private void FixedUpdate()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 50)) 
        {
            if(hit.transform.gameObject.tag == "SpecialInteractable") 
            {
                create_line(hit);
            }
            else if (is_hit)
            {
                //if touching other object that is not an interactable
                disableSpecialLine();
            }
        }
        else if (is_hit)
        {
            //if not touching anything
            disableSpecialLine();
        }
    }

    private void create_line(RaycastHit hit) 
    {

        if (!is_hit)
        {
            is_hit = true;
            lineRenderer.enabled = true;

            //outline object
            Outline outline = hit.transform.gameObject.GetComponent<Outline>();
            outline.enabled = true;
            latest_hit = hit;
        }

        targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue);
        if (triggerValue > 0.1f)
        {
            latest_hit.transform.gameObject.GetComponent<SpecialInteractable>().action();
        }


        Vector3 hit_location = hit.point;
        Vector3 hand_position = gameObject.transform.position;
        Vector3 mid_position = (hit_location + hand_position) / 2;
        lineRenderer.SetPosition(0, mid_position);
        lineRenderer.SetPosition(1, hit_location);
    }

    private void disableSpecialLine() 
    {
        is_hit = false;
        lineRenderer.enabled = false;
        if (latest_hit.transform != null)
        {
            Outline outline = latest_hit.transform.gameObject.GetComponent<Outline>();
            outline.enabled = false;
        }
    }

    //Regular Interactables
    private GameObject latest;

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Interactable")
        {
            is_hit = true;
            Outline outline = col.gameObject.GetComponent<Outline>();
            outline.enabled = true;
            latest = col.gameObject;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if(col.gameObject.tag == "Interactable") 
        {
            is_hit = false;
            disableOutline(col.gameObject);
        }
    }

    private void disableOutline(GameObject latest) 
    {
        Outline outline = latest.transform.gameObject.GetComponent<Outline>();
        outline.enabled = false;
    }
}
