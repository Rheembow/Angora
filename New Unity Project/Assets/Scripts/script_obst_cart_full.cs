using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_obst_cart_full: MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision) {
        if (!this.GetComponent<Rigidbody>()){
            if (collision.gameObject.tag == "Player") {
                this.GetComponent<MeshCollider>().convex = true;
                Debug.Log(this.GetComponent<MeshCollider>().convex);
                gameObject.AddComponent<Rigidbody>();
                this.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                this.GetComponent<Rigidbody>().mass = 0.25f;
            }
        }
    }

}