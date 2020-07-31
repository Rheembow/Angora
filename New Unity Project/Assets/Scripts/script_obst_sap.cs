using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_obst_sap : MonoBehaviour
{
    private script_player_move player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<script_player_move>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player"){
            player.speed = 2;
        }
    }

    void OnCollisionExit(Collision collision){
        if(collision.gameObject.tag == "Player"){
            player.speed = 4;
        }
    }

}
