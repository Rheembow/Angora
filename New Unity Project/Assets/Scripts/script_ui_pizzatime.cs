using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_ui_pizzatime : MonoBehaviour
{
    string feedback;
    float scoreMult;
    public bool pizzaTime = false;
    public float slowdownFactor = 0.3f;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    // <summary>
    // Toggles Pizza Time
    // <returns> The appropriate score multiplier if exiting Pizza Time, or zero if entering Pizza Time.
    // </summary>
    public float toggle(){
        if(!pizzaTime){
            pizzaTime = true;
            // Enable the parent object, and this object (the cursor).
            this.transform.parent.gameObject.SetActive(true);
            Debug.Log("Active =" + this.transform.parent.gameObject.activeInHierarchy);
            Time.timeScale = slowdownFactor;
            return 0.0f;
        }
        else{
            pizzaTime = false;
            setMultiplier();
            this.transform.parent.gameObject.SetActive(false);
            Time.timeScale = 1;
            return scoreMult;
        }
        
    }

    // <summary>
    // Sets the appropriate score multiplier and feedback message.
    // </summary>
    void setMultiplier(){
        switch(scoreMult){
            case 1.0f:
                feedback = "Good!";
                break;
            case 2.0f:
                feedback = "Great!";
                break;
            case 4.0f:
                feedback = "Excellent!";
                break;
            default:
                break;
        }
    }

    // <summary>
    // Changes the score.
    // <param name="mult"> The new score multiplier to use.
    // </summary>
    void score(float mult){
        scoreMult = mult;
    }
}
