using UnityEngine;
using UnityEngine.UI;

public class script_ui_timer : MonoBehaviour
{
    public Text timeText;
    private float gameTime = 0; //time ellapsed during game
    private float startTime = 0; //time the game was started
    private float initTime = 60; //initial amount of time on the timer

    private bool timeDone = false;
    //public script_ui_mainmenu linkToMenuScript;

    // Start is called before the first frame update
    void Start(){
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update(){
        //update the amount of time left
        gameTime = Time.time- startTime;
        gameTime = initTime - gameTime; 

        timeText.text = "Time:"+gameTime.ToString("0"); //update timer UI widget

        //game is finished
        if (gameTime <= 0){
            //linkToMenuScript.isFinished = true;
            timeDone = true;
        }
    }

    public bool getTimeDone() { return timeDone; }

    /// <summary>
    /// adds time by changing the start time
    /// addition: the amount of time added
    /// </summary>
    /// <param name="addition"></param>
    public void addTime(float addition) {
        startTime = startTime + addition;
    }
}
