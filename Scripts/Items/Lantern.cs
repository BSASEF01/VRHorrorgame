using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern : Item
{
    public GameObject point_light;
    public bool on = true;
    public AudioSource off_sound;

    float on_time = 30f; // Time that the light stays on, in seconds

    public override void Action()
    {
        if (on_time <= 0) return; //cannot turn on without any time left 
        base.Action();
        point_light.GetComponent<Light>().enabled = !on;
        on = !on;
    }

    protected override void Update()
    {
        base.Update();

        if (on_time <= 0 && !is_held)
        {
            //Play fire turning off sound
            if (on) 
            {
                passive_active = false;
                removePassiveEffect();
            }
            on = false;
            off_sound.Play();
            point_light.GetComponent<Light>().enabled = false;
        } 
        else if (on)
        {
            on_time -= Time.deltaTime;
        }
    }

    public override void passiveEffect()
    {
        if (!on) return;
        Debug.Log("Activating passive");
        base.passiveEffect();
        //add 5 points to the light attribute of current room
        GameManager.update_room_effect(0, 5);
    }

    public override void removePassiveEffect()
    {
        base.removePassiveEffect();
        Debug.Log("removing passive");
        //remove 5 points to the light attribute of current room
        GameManager.update_room_effect(0, -5);
    }

    public override void update_passive()
    {
        //Activate passive effect if item is not being held
        if (!is_held && !passive_active && on)
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
