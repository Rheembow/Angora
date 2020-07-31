using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_player_move : MonoBehaviour
{
    public script_procgen procgen;

    public Rigidbody rb;
    public float jumpHeight = 15; //jump height
    public Collider MainCollider;
    public Collider[] AllColliders;
    public MeshRenderer meshRenderer;
    public float ragdollTime;
    public float ragdollCooldown;
    float totalScore; //total points earned
    float pizzaPoints = 1;
    public float speed = 2; //speed
    public bool finished = false; // true if timer has finished
    public bool ragdoll;
    private Collider coll;
    public bool playerIsGrounded = false;


    private int nextPoint = 10; //next z point were score is increased

    //links to scripts
    public script_ui_pizzatime linkToPizzaTimeScript;
    public script_ui_score linkToScoreScript;
    public script_ui_score linkToFinalScoreScript;
    public script_ui_mainmenu linkToMenuScript;
    public script_ui_timer linkToTimerScript;


    // Start is called before the first frame update
    void Start(){
        //void Awake() { 
        Debug.Log("start move");
        MainCollider = GetComponent<Collider>();
        AllColliders = GetComponentsInChildren<Collider>(true);
        rb = gameObject.GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        DoRagdoll(false);
        ragdollTime = 0.0f;
        ragdollCooldown = 0.0f;
        Time.timeScale = 1;
        coll = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update(){
        // Only allow movement if player is not ragdolling
        if(ragdollTime <= 0.0f){
           if(ragdoll){
                Debug.Log("Stopped ragdoll");
                DoRagdoll(false);
                
           }
            this.transform.rotation = new Quaternion(0,0,0,1);

            //translate based on key inputs. Move as long as key is pressed down
            if (Input.GetKey("a")){ //move left
                transform.Translate(-1* speed * Time.deltaTime, 0, 0);
            }
            if (Input.GetKey("d")){// move right
                transform.Translate(speed * Time.deltaTime, 0, 0);

            }
            if (Input.GetKeyDown("w")){ //jump

                if (playerIsGrounded)
                {
                    rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
                }
            }
            if (Input.GetKeyUp("space")){ // Pizza time
                pizzaTime();
               
            }

            // Move forward constantly
            transform.Translate(0, 0, speed*Time.deltaTime);
        }
        // Decrement ragdoll timers by 0.1 each frame
        ragdollTime -= 1.0f;
        ragdollCooldown -= 1.0f;

        if(this.transform.position.y < 1){ //make sure above ground
            this.transform.position = new Vector3(this.transform.position.x, 1.5f, this.transform.position.z);
            //this.transform.Translate(new Vector3(0,1,0));
        }
        
        //increase score based on position
        if(this.transform.position.z >= nextPoint ){
            totalScore += 2;
            //Debug.Log("POSITION CHANGE Score = " + totalScore);
            linkToScoreScript.update(totalScore);
            nextPoint += 10;
        }

        //if the timer is done and this code hasn't been run before
        if (linkToTimerScript.getTimeDone() & !finished){
            Debug.Log("Score = " + totalScore);
            linkToFinalScoreScript.update(totalScore);
            
            //stop player moving
            speed = 0;
            linkToMenuScript.isFinished = true;
            finished = true;
        }

        if (this.transform.position.z > procgen.newChunkThreshold){
            procgen.spawnChunkSegment();
        }

        
    }

    // <summary>
    // Enters or exits ragdoll mode.
    // <param name="isRagdoll"> True if entering ragdoll mode, false if exiting ragdoll mode
    // </summary>
    public void DoRagdoll(bool isRagdoll){
        // Enable each collider if we're in ragdoll mode.
        foreach(var col in AllColliders)
            col.enabled = isRagdoll;
        MainCollider.enabled = !isRagdoll;
        rb.useGravity = !isRagdoll;
        GetComponent<Animator>().enabled = !isRagdoll;
        //rb.isKinematic = !isRagdoll;
        rb.maxDepenetrationVelocity = 0.001f;

        if(isRagdoll && ragdollTime <= 0.0f){
            //Debug.Log("Started ragdoll");
            ragdollTime = 300.0f;
            ragdollCooldown = 600.0f;
            
        }
        ragdoll = isRagdoll;
    }

    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.tag == "Obstacle" && ragdollCooldown <= 0.0f){
            Debug.Log("Entering Ragdoll");
            DoRagdoll(true);
        } else if(collision.gameObject.tag == "Ground")
        {
            playerIsGrounded = true;
        }
        
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            playerIsGrounded = false;
        }
    }

    void OnTriggerEnter(Collider collider){
        if(collider.tag == "Pizza"){
            Debug.Log("Pizza Trigger");
            pizzaTime();
        }
    }
    
    void pizzaTime(){
        totalScore += pizzaPoints * linkToPizzaTimeScript.toggle();
        Debug.Log("Score = " + totalScore);
        linkToScoreScript.update(totalScore);
    }
}
