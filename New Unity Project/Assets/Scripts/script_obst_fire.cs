using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_obst_fire : MonoBehaviour{
    private script_player_move player;
    public script_ui_timer timeScript; 

    // Start is called before the first frame update
    void Start(){
        player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<script_player_move>();
        timeScript = GameObject.FindGameObjectsWithTag("Timer")[0].GetComponent<script_ui_timer>();

    }

    // Update is called once per frame
    void Update(){
    }

    void OnCollisionEnter(Collision collision){
        if (collision.gameObject.tag == "Player"){
            //make fire object disappear
            this.transform.position = new Vector3(this.transform.position.x, -20f, this.transform.position.z);
            changePlayerTime();
        }
    }


    /// <summary>
    /// adds 10 seconds to the amount of time the player has left
    /// </summary>
    void changePlayerTime(){
        Debug.Log("trying to change time");
        timeScript.addTime(10);
    }
}

