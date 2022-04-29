using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{

    public int enemy_id;
    public Vector3[] enemy_locations; // Starts at 0 and n-1 is the last position 
    public int window_location; //This is the location in the array in which the enemy reaches the window. During this, the enemy will check if 
    public int enemy_area;
    public int agressiveness = 10; //10 is the base aggressiveness, with 100 being the max
    public int enemy_type; // 1 is weak against light, 2 is weak against sound, 3 tbh, 4tbh
    public bool stopped = false;

    public int startle_meter = 0;
    public int startle_sen = 30;
    private bool is_startled;

    private int current_location = 0;
    private bool final_position = false;
    private int current_effects = 0;

    



    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("move_forward", 3f, 3f); //Function called every 3 seconds
        InvokeRepeating("increase_startle", 2f, 2f); //Function called every 2 seconds
    }

    // Update is called once per frame
    void Update()
    {
        
        if (final_position) check_attack_opportunity();
        Debug.Log(name + " startle level " + startle_meter);
    }


    public void move_forward()
    {
        if (stopped)
        {
            stopped = false;
            return;
        }

        Debug.Log(name + " agressiveness " + agressiveness);
        //Determines where if and when the enemy moves, and where it moves.
        //We draw a random number and see if the enemy can move.
        int num = Random.Range(0, 100);//10% chance to move every 1 second
        if (num < agressiveness)
        {
            //The enemy is able to move to next location
            //If enemy hits certain location do something.
            if (current_location + 1 < enemy_locations.Length)
            {
                if(current_location == window_location) 
                {
                    if (GameManager.window_status[enemy_area] == true)
                    {
                        //break window if closed and go three steps back
                        GameManager.window_status[enemy_area] = false; // TODO Distintion between broken and open needed.
                        GameObject window = GameManager.static_windows[enemy_area];

                        //Disable glass in window and play sound effect
                        window.transform.Find("sound_effect").gameObject.GetComponent<PlaySound>().playSound();
                        window.transform.Find("window_glass").gameObject.SetActive(false);

                        current_location = 0;
                        transform.position = enemy_locations[current_location];
                    }
                    else 
                    {
                        //window is open, let enemy get in house
                        //Check every frame if door is open, if it is then jump scare enemy
                        current_location = current_location + 1;
                        transform.position = enemy_locations[current_location];
                    }

                }
                else
                {
                    current_location = current_location + 1;
                    transform.position = enemy_locations[current_location];
                }
            }
            else
            {
                Debug.Log("Enemy is in final position");
                final_position = true;
            }
        }
    }

    public void check_attack_opportunity()
    {
        //if user is in current area, defeat player
        if(enemy_area == GameManager.current_room) 
        {
            Debug.Log("You have lost! Killed by " +  enemy_id);
            return;
        }

        switch (enemy_area) 
        {
            case 0:
                if (!GameManager.door_status[0]) //Door is open 
                {
                    //Check if user is in room 1, jumpscare
                    Debug.Log("You have lost! Killed by " + enemy_id);
                }
                break;

            case 1:
                //Check doors 0 and 1
                if (!GameManager.door_status[0]) //Door is open 
                {
                    //Check if user is in room 0, jumpscare
                    Debug.Log("You have lost! Killed by " + enemy_id);
                }
                else if (!GameManager.door_status[1])
                {
                    //Check if user is in room 2, jumpscare
                    Debug.Log("You have lost! Killed by " + enemy_id);
                }
                break;

            case 2:
                //check door 1 and 2 and 3 
                if (!GameManager.door_status[1]) //Door is open 
                {
                    //Check if user is in room 1, jumpscare
                    Debug.Log("You have lost! Killed by " + enemy_id);
                }
                else if (!GameManager.door_status[2])
                {
                    //Check if user is in room 3, jumpscare
                    Debug.Log("You have lost! Killed by " + enemy_id);
                }
                else if (!GameManager.door_status[3])
                {
                    //Check if user is in room 4, jumpscare
                    Debug.Log("You have lost! Killed by " + enemy_id);
                }
                break;

            case 3:
                //check door 2
                if (!GameManager.door_status[2]) //Door is open 
                {
                    //Check if user is in room 3, jumpscare
                    Debug.Log("You have lost! Killed by " + enemy_id);
                }
                break;
        }   
    }

    public void retreat(int steps) 
    {
        //move back a step
        if(current_location - steps >= 0)
        {
            current_location = current_location - steps;
            transform.position = enemy_locations[current_location];
        }
       
    }

    public void increase_agressiveness(int amount)
    {
        agressiveness = agressiveness + amount;
    }

    public void decrease_agressiveness(int amount)
    {
        if (agressiveness - amount < 0)
        {
            agressiveness = 0;
        }
        else 
        {
            agressiveness = agressiveness - amount;
        }
    }

    public void set_agressiveness(int a) 
    {
        agressiveness = a;
    }

    public void stop_movement() 
    {
        stopped = true;
    }


    //Add function for enemies to switch locations

    public virtual void startle(int amount, int type) {
        //first check if the amount is decreasing or increasing, decreasing means items are being removed
        if(amount < 0 && type == enemy_type) 
        {
            startle_meter = startle_meter + amount;
            increase_agressiveness(-1* amount);
            current_effects = current_effects - 1;
            if(current_effects == 0) // disable startle timer 
            {
                is_startled = false;
            }

            return;
        }
        else if(type == enemy_type)//Check if type of effect is effective against current enemy 
        {
            is_startled = true;
            current_effects = current_effects + 1;
            decrease_agressiveness(amount);
            startle_meter = startle_meter + amount;
        }
    }

    public void increase_startle() 
    {
        if (!is_startled) return;

        startle_meter = startle_meter + 1;

        //Check if the enemy is too startled, if it is then swap with another enemy
        if(startle_meter > startle_sen) 
        {
            Debug.Log("I'm swapping with someone" +  gameObject.name);
        }
    }

    public void update_player_aggressiveness()
    {
        //returns true if player is outside the current room
        if (enemy_area == GameManager.current_room)
        {
            decrease_agressiveness(10);
        }
        else
        {
            increase_agressiveness(10);
        }
    }

    public void swap_enemy() 
    {
        //Swap with another enemy, this includes all they stats and everything. 
    }
}
