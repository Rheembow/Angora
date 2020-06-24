﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_procgen : MonoBehaviour {

    public GameObject[] chunks;
    public GameObject[] obstacles;
    public int MAX_LEVEL_SIZE;
    private Dictionary<string, GameObject> chunkMap = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> obstMap = new Dictionary<string, GameObject>();
    private Vector3 chunkLoc = new Vector3(0.0f, 0.0f, 0.0f);
    private Quaternion ZERO_QUAT = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
    private float difficulty = 0;
    private int count = 0;
    private float deltaLocX;
    private float deltaLocY;
    private float deltaLocZ;
    private int amtHouse;

    // Start is called before the first frame update
    void Start() {
        FillMap();

        deltaLocX = chunkMap["start"].GetComponent<BoxCollider>().size.x * chunkMap["start"].GetComponent<Transform>().localScale.x;
        deltaLocY = chunkMap["start"].GetComponent<BoxCollider>().size.y * chunkMap["start"].GetComponent<Transform>().localScale.y;
        deltaLocZ = chunkMap["start"].GetComponent<BoxCollider>().size.z * chunkMap["start"].GetComponent<Transform>().localScale.z;
        chunkLoc.x -= deltaLocX;

        StartCoroutine(spawnChunkSegment());
    }

    // Update is called once per frame
    void Update() {
        //Use this method to determine when the player moves far enough on the chunk to call spawnChunk() instead of having spawnChunk() on a timer.
    }

    IEnumerator spawnChunkSegment() {
        string chunkName = "";
        for(int i = 0; i < 3; i++){
            chunkName = getChunkName(i);
            Instantiate(chunkMap[chunkName], chunkLoc, ZERO_QUAT);
            if(i == 1 && count != 0 && count <= MAX_LEVEL_SIZE)
                SpawnObstacles();
            chunkLoc.x += deltaLocX;
        }
        chunkLoc.x -= deltaLocX * 3;
        chunkLoc.y += deltaLocY;

        if(count != 0 && count <= MAX_LEVEL_SIZE)
            Instantiate(chunkMap["house_" + Random.Range(0, amtHouse).ToString()], chunkLoc, ZERO_QUAT);

        chunkLoc.y -= deltaLocY;
        chunkLoc.z += deltaLocZ;
        count++;
        
        yield return new WaitForSeconds(0.25f);
        if(count <= MAX_LEVEL_SIZE + 1)
            StartCoroutine(spawnChunkSegment());
    }

    /// <summary>
    /// Populates the chunkMap Dictionary with chunk names and Transforms
    /// </summary>
    void FillMap(){
        for(int i = 0; i < chunks.Length; i++){
            if(chunks[i].name.Substring(0, 6) != "chunk_")
                Debug.Log("Error loading chunk file: " + chunks[i].name + ". File named incorrectly.");
            else
                chunkMap.Add(chunks[i].name.Substring(6), chunks[i]);
        }

        foreach(KeyValuePair<string, GameObject> entry in chunkMap){
            if(entry.Key.Length > 6)
                if(entry.Key.Substring(0, 6) == "house_")
                    amtHouse++;
        }

        for(int i = 0; i < obstacles.Length; i++){
            if(obstacles[i].name.Substring(0, 5) == "form_" || obstacles[i].name.Substring(0, 5) == "obst_")
                obstMap.Add(obstacles[i].name.Substring(5), obstacles[i]);
            else
                Debug.Log("Error loading obstacle file: " + obstacles[i].name + ". File named incorrectly.");
        }
    }

    /// <summary>
    /// Determines the next chunk to be Instantiated in the chunk array that isnt the start or finish chunk
    /// </summary>
    string getChunkName(int i){

        string chunkName;

        if(i == 1){
            chunkName = "road";
            if(count == 0)
                chunkName = "start";
            else if(count > MAX_LEVEL_SIZE)
                chunkName = "finish";
        }
        else
            chunkName = "grass";

        return chunkName;
    }

    void setDifficulty(){
        difficulty += 1/MAX_LEVEL_SIZE; //difficulty needs to be calculated better
    }

    void SpawnObstacles(){
        setDifficulty();

        //determine chunk borders
        Vector3 spawn = new Vector3(0.0f, 0.0f, 0.0f);
        float left = chunkLoc.x - deltaLocX/2;
        float right = chunkLoc.x + deltaLocX/2;
        float front = chunkLoc.z + deltaLocZ/2;
        float back = chunkLoc.z - deltaLocZ/2;

        int numObst = 2; //this should be changed with difficulty
        for(int i = 0; i < numObst; i++){
            spawn.x = Random.Range(left, right);
            spawn.y = chunkLoc.y + deltaLocY;
            spawn.z = Random.Range(back, front);
            Instantiate(obstMap[getObstName()], spawn, ZERO_QUAT);
        }
    }

    string getObstName(){

        //Do more here with difficulty checks to determine formations vs obstacles
        return obstacles[Random.Range(0, obstacles.Length)].name.Substring(5);
    }
}