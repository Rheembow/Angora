using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class script_ui_score : MonoBehaviour
{
    public Text scoreText;

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
    }

    /// <summary>
    /// changes score text to given number
    /// score: new score to display
    /// </summary>
    /// <param name="score"></param>
    public void update(float score){
        //Debug.Log("Score Change = " + score);
        scoreText.text = "Score: " + score.ToString();
    }
}
