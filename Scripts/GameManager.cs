using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public float game_time = 480f;
    float current_time = 0f;

    public static Enemy[] enemies;
    public static bool[] window_status; //False = open, True = closed, Null if window is destroyed.
    public GameObject[] windows;
    public static GameObject[] static_windows;
    public static bool[] door_status; //False = open, True = closed
    public static int[,] room_effects; // Represents the effects that are currently active in the room. with i being the room number 1-4, and j =1 being light, j = 2 = sound, j=3 = tbh, j=4 = tbh


    public static Dictionary<int, Vector3> rooms = new Dictionary<int, Vector3>() {
        { 0, new Vector3(1.25f, 0, -1.25f) } ,
        { 1, new Vector3(1.25f, 0, 1.25f) } ,
        { 2, new Vector3(-1.25f, 0, 1.25f) } ,
        { 3, new Vector3(-1.25f, 0, -1.25f) }
    };

    public static int current_room = 1;


    private void Start()
    {
        enemies = new Enemy[4];
        static_windows = new GameObject[4];
        window_status = new bool[4];
        room_effects = new int[4,4];

        for(int i = 0; i < 4; i++) 
        {
            window_status[i] = true;
            static_windows[i] = windows[i];
        }
        door_status = new bool[5];

        for (int i = 0; i < 5; i++)
        {
            door_status[i] = false;
        }

        enemies[0] = GameObject.Find("Enemy_1").GetComponent<Enemy>();
        enemies[1] = GameObject.Find("Enemy_2").GetComponent<Enemy>();
        enemies[2] = GameObject.Find("Enemy_3").GetComponent<Enemy>();
        enemies[3] = GameObject.Find("Enemy_4").GetComponent<Enemy>();

       for(int i = 0; i < 4; i++) 
       {
            for(int j = 0; j<4; j++) 
            {
                room_effects[i,j] = 0;
            }
       }
           
    }

    void Update()
    {
        Debug.Log("Window 1" + window_status[0]);
        Debug.Log("Window 2" + window_status[1]);
        Debug.Log("Window 3" + window_status[2]);
        Debug.Log("Window 4" + window_status[3]);
        current_time += Time.deltaTime;
        if(current_time >= game_time) 
        {
            Debug.Log("Game Ended");
        }
        else if (current_time == 60f) //increase the aggressiveness of enemies
        {
            for(int i = 0; i < enemies.Length; i++) 
            {
                enemies[i].increase_agressiveness(10);
            }
        }
        else if (current_time == 120f)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].increase_agressiveness(10);
            }

        }
        else if (current_time == 180f)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].increase_agressiveness(15);
            }
        }
        else if(current_time == 240f) 
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].increase_agressiveness(15);
            }
        }
    }

    public static bool changeRoom(int room) 
    {
        //returns true if room was switched, else if you are in the same room or an error occurs return false;
        if (current_room == room)
        {
            Debug.Log("You cannot" + current_room);
            return false;
        }
        else
        {

            current_room = room;
            Debug.Log(current_room);
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].update_player_aggressiveness();
            }
            return true;
        } 
    }

    public static int getCurrentRoom() 
    {
        return current_room;
    }

    public static Vector3 getCurrentRoomLocalLocation() 
    {
        return rooms[current_room];
    }

    public static void update_room_effect(int type, int amount) 
    {
        room_effects[current_room, type] = amount;
        Debug.Log("updating room effect.");
        //update enemy agressiveness
        enemies[current_room].startle(amount, type); 

    }
}
