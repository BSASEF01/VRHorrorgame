using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFog : MonoBehaviour
{
    private Vector3 current_room_location;
    private Renderer rend;
    
    public GameObject fade_plane;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = GameManager.getCurrentRoomLocalLocation();
        rend = fade_plane.GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player") 
        {
            //fade out
            Fade(1, 0);
        }

    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {

            Debug.Log("Im on the outside!"); 
            Debug.Log(col.gameObject.tag);
            //fade in and darken
            Fade(0, 1);
        }

    }

    public void changeRooms() 
    {
        transform.position = GameManager.getCurrentRoomLocalLocation();
    }

    public void Fade(float alphaIn, float alphaOut)
    {
        StartCoroutine(FadeRoutine(alphaIn, alphaOut));
    }

    public IEnumerator FadeRoutine(float alphaIn, float alphaOut)
    {
        float timer = 0;
        while (timer <= .25f)
        {
            Color new_color = Color.black;
            new_color.a = Mathf.Lerp(alphaIn, alphaOut, timer / .25f);

            rend.material.SetColor("_Color", new_color);
            timer += Time.deltaTime;
            yield return null;
        }

        Color final_color = Color.black;
        final_color.a = alphaOut;
        Debug.Log(Color.black + "" + alphaOut);

        rend.material.SetColor("_Color", final_color);
    }
}
