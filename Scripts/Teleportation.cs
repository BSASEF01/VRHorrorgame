using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportation : SpecialInteractable {
    //make a generic class for special interactions

    public GameObject player;
    public float fade_duration;
    public GameObject fade_plane;
    public int room_number;
    public GameObject currentArea;

    private Color fade_color = Color.black;
    private Renderer rend;
    private GameObject telelocation;


    // Start is called before the first frame update
    void Start()
    {
        rend = fade_plane.GetComponent<Renderer>();
        telelocation = GetComponentInChildren<Transform>().gameObject;
    }   

    // Update is called once per frame
    void Update()
    {

    }

    public void FadeIn() 
    {
        Fade(0, 1);
    }

    public void FadeOut()
    {
        Fade(1, 0);
    }


    public void Fade(float alphaIn, float alphaOut)
    {
        StartCoroutine(FadeRoutine(alphaIn, alphaOut));
    }

    public IEnumerator FadeRoutine(float alphaIn, float alphaOut)
    {
        float timer = 0;
        while(timer <= fade_duration)
        {
            Color new_color = fade_color;
            new_color.a = Mathf.Lerp(alphaIn, alphaOut, timer / fade_duration);

            rend.material.SetColor("_Color", new_color);
            timer += Time.deltaTime;
            yield return null;
        }

        Color final_color = fade_color;
        final_color.a = alphaOut;

        rend.material.SetColor("_Color", final_color);
    }

    public override void action()
    {
        base.action();
        if (GameManager.changeRoom(room_number))
        {
            FadeIn();
            //teleport player to location
            player.transform.position = telelocation.transform.position;
            currentArea.GetComponent<PlayerFog>().changeRooms();
            FadeOut();
        }
        else
        {
            Debug.Log("Already in this room");
        }
    }
}
