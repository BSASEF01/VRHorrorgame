using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowLock : MonoBehaviour
{

    //Door object
    public GameObject window;
    private Vector3 new_position;

    //Children of door object
    public Rigidbody handleRB;
    public Rigidbody handleChildRB;
    public int handle_id;
    Rigidbody windowRB;

    void Start()
    {
        new_position = window.transform.position;
        new_position.y = 2f;

    }

    private void OnTriggerEnter(Collider other)
    {
        //When the door lock collider interacts with our collider
        //Make window unlocked
        if (other.name == "door_lock")
        {
            GameManager.window_status[handle_id] = false;
            windowRB = window.GetComponent<Rigidbody>();
            windowRB.angularVelocity = Vector3.zero;
            windowRB.velocity = Vector3.zero;
            window.transform.position = new_position;

            handleRB.angularVelocity = Vector3.zero;
            handleRB.velocity = Vector3.zero;

            handleChildRB.angularVelocity = Vector3.zero;
            handleChildRB.velocity = Vector3.zero;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.name == "door_lock")
        {
            
            window.transform.position = new_position;
            windowRB.angularVelocity = Vector3.zero;
            windowRB.velocity = Vector3.zero;

            handleRB.angularVelocity = Vector3.zero;
            handleRB.velocity = Vector3.zero;

            handleChildRB.angularVelocity = Vector3.zero;
            handleChildRB.velocity = Vector3.zero;
        }
    }
}