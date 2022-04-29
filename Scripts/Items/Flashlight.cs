using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : Item
{
    private bool on = false;
    public GameObject flashlight;
    public GameObject ray_location;
    public LayerMask layer;
    public AudioSource click_sound;

    private float current_hit_distance;

    public override void Action()
    {
        base.Action();
        click_sound.Play();
        flashlight.GetComponent<Light>().enabled = !on;
        on = !on;
    }

    private void FixedUpdate() 
    {

        if (on && is_held == true)
        {
            RaycastHit hit;
            if (Physics.SphereCast(ray_location.transform.position, 3f,transform.forward, out hit, 100f, 1 << LayerMask.NameToLayer("Enemies")))
            {
                
                GameObject hit_go = hit.transform.gameObject;
                if (hit_go.tag == "Enemy") 
                {
                    current_hit_distance = hit.distance;
                    hit_go.GetComponent<Enemy>().stop_movement();
                }
            }
            else 
            {
                current_hit_distance = 100f;
            }
        }
   
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(ray_location.transform.position, ray_location.transform.position  + transform.forward * current_hit_distance);
        Gizmos.DrawSphere(ray_location.transform.position + transform.forward * current_hit_distance, 3f);
    }
}
