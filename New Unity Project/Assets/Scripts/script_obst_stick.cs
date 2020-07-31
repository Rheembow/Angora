using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_obst_stick: MonoBehaviour
{
    public float thrust;
    private bool collided;
    
    // Start is called before the first frame update
    void Start()
    {
        collided = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!collided)
            this.GetComponent<Rigidbody>().AddForce(0f, 0f, thrust * -1f, ForceMode.Force);
    }
    
    void OnCollisionEnter(Collision collision)
    {
        string name = collision.gameObject.name;
        if(collision.gameObject.tag == "Obstacle" || collision.gameObject.tag == "Player")
            if(name.Substring(5) != "sap" && name.Substring(5) != "cheese")
                collided = true;
    }
}
