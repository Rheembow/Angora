using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_camera_move : MonoBehaviour
{
    public float camDistFromPlayer = 2.5f; //distance camera maintins from the player
    public Transform player;

    // Start is called before the first frame update
    void Start(){
        player = GameObject.FindWithTag("Player").transform;
        camDistFromPlayer = (transform.position.z- player.position.z);
    }

    // Update is called once per frame
    void LateUpdate(){
        //update camera position based on player z position
        transform.position = new Vector3(transform.position.x, transform.position.y, player.position.z+camDistFromPlayer);

    }
}
