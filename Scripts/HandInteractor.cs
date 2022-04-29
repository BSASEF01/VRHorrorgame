using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandInteractor : XRDirectInteractor
{

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        GameObject held_object = args.interactableObject.transform.gameObject;

        if(held_object.GetComponent<Item>() != null) 
        {
            held_object.GetComponent<Item>().is_held = true;
        }

    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        GameObject held_object = args.interactableObject.transform.gameObject;

        if (held_object.GetComponent<Item>() !=null)
        {
            held_object.GetComponent<Item>().is_held = false;
        }

    }
}
