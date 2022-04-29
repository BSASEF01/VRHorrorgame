using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles the locking of doors. 
public class HandleDoorLock : MonoBehaviour
{

    //Door object
    public GameObject door;
    private Vector3 original_position;
    private bool start = true;

    //Children of door object
    public Rigidbody handleRB;
    public Rigidbody handleChildRB;
    Rigidbody doorRB;

    public int handle_id;
    public string handle_parent_type;

    public AudioSource close_sound;
    

    void Start()
    {
        //Get default position for locked door, in this case closed. 
        original_position = door.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        //When the door lock collider interacts with our collider
        if (other.name == "door_lock")
        {
            //Play Sound
            if (start) start = false;
            else close_sound.Play();


            //Set door to defaut postion and remove all motion from the object. This prevents it from just swinging back out 
            doorRB = door.GetComponent<Rigidbody>();
            doorRB.angularVelocity = Vector3.zero;
            doorRB.velocity = Vector3.zero;
            door.transform.position = original_position;

            handleRB.angularVelocity = Vector3.zero;
            handleRB.velocity = Vector3.zero;

            handleChildRB.angularVelocity = Vector3.zero;
            handleChildRB.velocity = Vector3.zero;

            if (handle_parent_type == "window") 
            {
                GameManager.window_status[handle_id] = true;
            }
            else if (handle_parent_type == "door")
            {
                GameManager.door_status[handle_id] = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.name == "door_lock")
        {
            //Keep the door locked until user grabs it.
            //TODO find a way to keep the door in place without having to update position every frame. 
            door.transform.position = original_position;
            doorRB.angularVelocity = Vector3.zero;
            doorRB.velocity = Vector3.zero;

            handleRB.angularVelocity = Vector3.zero;
            handleRB.velocity = Vector3.zero;

            handleChildRB.angularVelocity = Vector3.zero;
            handleChildRB.velocity = Vector3.zero;
        }
    }
}
