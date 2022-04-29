using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

//Handles door handle behavior
public class HandleReset : XRGrabInteractable
{
    public Transform parent;
    public GameObject door_lock;
    public int handle_id;
    public string handle_parent_type;
    public AudioSource open_sound;
    private SelectEnterEventArgs selector;

    protected override void Detach() 
    {
        //When user lets go of object
        base.Detach();

        // Resets handle to original place
        transform.position = parent.transform.position;
        transform.rotation = parent.transform.rotation;
        transform.localScale = new Vector3(1f, 1f, 1f);

        // Fixes Wonky physics
        Rigidbody rb = parent.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.angularDrag = 0;

        //Enable lock
        door_lock.SetActive(true);
    }

    protected override void Grab()
    {
        //Play grab sound
        if(handle_parent_type == "window") 
        {
            if(GameManager.window_status[handle_id]== true) open_sound.Play();
        }
        else open_sound.Play();

        //User grabs object 
        base.Grab();
        //disable lock
        door_lock.SetActive(false);
        if (handle_parent_type == "door")
        {
            GameManager.door_status[handle_id] = false;
        }
    }

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        //Gets information on who is grabbing the object. 
        base.OnSelectEntering(args);
        selector = args;
    }

    public void Update()
    {
        if (Vector3.Distance(parent.position, transform.position) > 0.5f)
        {
            //If user pulls handle too far from the door, the handle is force released. 
            selector.manager.SelectExit(selector.interactorObject, this);
        }
    }

}
