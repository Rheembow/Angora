using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_obst_cheese : MonoBehaviour
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
        this.GetComponent<Rigidbody>().AddTorque(0f, 1f, 0f, ForceMode.Force);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player"){
            this.transform.position = new Vector3(this.transform.position.x, -20f, this.transform.position.z);
            StartCoroutine(changePlayerSpeed());
        }
    }

    IEnumerator changePlayerSpeed(){
        player.speed = 6;
        yield return new WaitForSeconds(2);
        player.speed = 4;
    }
}
