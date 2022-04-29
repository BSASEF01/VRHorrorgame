using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//In charge of moving handle when user grabs handle
public class HandlePhysics : MonoBehaviour
{
    public Transform target;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        //moves gameobject to parents position
        rb.MovePosition(target.transform.position);
    }
}
