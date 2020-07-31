using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_procgen : MonoBehaviour {

    public GameObject[] chunks; //Array of chunk_ and chunk_house_ files that are generated into the world
    public GameObject[] obstacles; //Array of obst_ and form_ files that are spawned as obstacles
    public int MAX_LEVEL_SIZE;
    public float newChunkThreshold;

    private Dictionary<string, GameObject> chunkMap = new Dictionary<string, GameObject>(); //Hashmap of chunks
    private Dictionary<string, GameObject> obstMap = new Dictionary<string, GameObject>(); //Hashmap of obstacles

    private Vector3 chunkLoc = new Vector3(0.0f, 0.0f, 0.0f); //Keeps track of current chunk location during generation
    private Quaternion ZERO_QUAT = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);

    private float difficulty = 0;
    private int count = 0;
    private float deltaLocX;
    private float deltaLocY;
    private float deltaLocZ;
    private int amtHouse;

    void Start() {
        FillMap();

        //Determine the change in position when spawning chunks
        //This is taken from the chunk_start GameObject
        deltaLocX = chunkMap["start"].GetComponent<BoxCollider>().size.x * chunkMap["start"].GetComponent<Transform>().localScale.x;
        deltaLocY = chunkMap["start"].GetComponent<BoxCollider>().size.y * chunkMap["start"].GetComponent<Transform>().localScale.y;
        deltaLocZ = chunkMap["start"].GetComponent<BoxCollider>().size.z * chunkMap["start"].GetComponent<Transform>().localScale.z;

        chunkLoc.x -= deltaLocX;

        newChunkThreshold = chunkLoc.z - (deltaLocZ * 2);
        spawnChunkSegment();
        spawnChunkSegment();
    }

    // Update is called once per frame
    void Update() {
        //Use this method to determine when the player moves far enough on the chunk to call spawnChunk() instead of having spawnChunk() on a timer.
    }

    ///<summary>
    /// Instantiates 3 sequential chunks (grass, road, grass) along the X axis in the world
    /// Instantiates a random house on top of the grass chunks
    ///</summary>
    public void spawnChunkSegment() {
        if(count > MAX_LEVEL_SIZE + 1)
            return;

        setDifficulty();

        string chunkName = "";

        //Generate 3 chunks along the X axis
        for(int i = 0; i < 3; i++){
            chunkName = getChunkName(i); //Determine the type of chunk to spawn
            Instantiate(chunkMap[chunkName], chunkLoc, ZERO_QUAT); //Instantiate chunk
            
            //If currently on the road chunk spawn obstacles
            if(i == 1 && count != 0 && count <= MAX_LEVEL_SIZE)
                SpawnObstacles();
            
            //Determine if current chunk is a pizza chunk
            if(i == 1 && isPizzaChunk()){
                chunkLoc.z -= deltaLocZ;
                chunkLoc.y += deltaLocY;
                Instantiate(chunkMap["pizza"], chunkLoc, ZERO_QUAT); //Spawn pizza event trigger before pizza chunk
                chunkLoc.z += deltaLocZ;
                chunkLoc.y -= deltaLocY;
            }
            chunkLoc.x += deltaLocX;
        }
        chunkLoc.x -= deltaLocX * 3;

        //Spawn house on the left grass chunk
        if(count != 0 && count <= MAX_LEVEL_SIZE){
            GameObject house = chunkMap["house_" + Random.Range(0, amtHouse).ToString()];
            chunkLoc.y += (deltaLocY / 2); //+ (house.GetComponent<BoxCollider>().size.y * house.GetComponent<Transform>().localScale.y) / 2;
            Instantiate(house, chunkLoc, house.transform.rotation);
            chunkLoc.y = 0;
        }

        chunkLoc.z += deltaLocZ;
        count++;
        newChunkThreshold += deltaLocZ;
    }

    /// <summary>
    /// Populates the chunkMap Dictionary with chunk names and Transforms
    /// </summary>
    void FillMap(){

        //Iterate through the chunks array and associate a name with the chunk
        //Takes substring(6) which is whatever is after "chunk_"
        for(int i = 0; i < chunks.Length; i++){
            if(chunks[i].name.Substring(0, 6) != "chunk_")
                Debug.Log("Error loading chunk file: " + chunks[i].name + ". File named incorrectly.");
            else
                chunkMap.Add(chunks[i].name.Substring(6), chunks[i]);
        }

        //Keep a count of the house chunks
        foreach(KeyValuePair<string, GameObject> entry in chunkMap){
            if(entry.Key.Length > 6)
                if(entry.Key.Substring(0, 6) == "house_")
                    amtHouse++;
        }

        //Associate names to obstacles in the same way as chunks
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

        //Determine which chunk to spawn
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

    // Returns true for 10% of the total level size
    bool isPizzaChunk(){
        return Random.Range(0, MAX_LEVEL_SIZE) <= MAX_LEVEL_SIZE / 10;
    }

    void setDifficulty(){
        difficulty += 1/MAX_LEVEL_SIZE;
    }

    void SpawnObstacles(){
        setDifficulty();

        Vector3 spawn = new Vector3(0.0f, 0.0f, 0.0f);
        GameObject currObst;

        //Determine the bounds of the current chunk
        float left = chunkLoc.x - deltaLocX / 2;
        float right = chunkLoc.x + deltaLocX / 2;
        float front = chunkLoc.z + deltaLocZ / 2;
        float back = chunkLoc.z - deltaLocZ / 2;

        int numObst = 2; //this should be changed with difficulty
        for(int i = 0; i < numObst; i++){
            currObst = obstMap[getObstName()]; //Find an obstacle to spawn
            (float l, float r, float b) obstBounds = getObstacleBounds(currObst); //Determine the bounds of the current obstacle

            //Spawn the obstacle within the bounds of the chunk including the current obstacles dimensions
            spawn.x = Random.Range(left - obstBounds.l + 0.1f, right - obstBounds.r - 0.1f); 
            spawn.y = (deltaLocY / 2) + obstBounds.b;
            spawn.z = Random.Range(back, front);
            Instantiate(currObst, spawn, ZERO_QUAT);
        }
    }

    (float l, float r, float b) getObstacleBounds(GameObject obst){
        List<Transform> obsts = new List<Transform>();

        float left = 0;
        float right = 0;
        float bottom = 0;

        if(obst.name.Substring(0, 5) == "form_")
            for(int i = 0; i < obst.transform.childCount; i++)
                obsts.Add(obst.transform.GetChild(i).transform);
        else
            obsts.Add(obst.transform);

        obsts.ForEach(delegate(Transform t){
            if(t.position.x - (t.localScale.x / 2) < left)
                left = t.position.x - t.localScale.x / 2;
            if(t.position.x + (t.localScale.x / 2) > right)
                right = t.position.x + t.localScale.x / 2;
            if(t.position.y - (t.localScale.y / 2) < bottom)
                bottom = t.position.y - t.localScale.y / 2;
        });

        return (l: left, r: right, b: bottom);
    }

    string getObstName(){

        //Do more here with difficulty checks to determine formations vs obstacles
        return obstacles[Random.Range(0, obstacles.Length)].name.Substring(5);
    }
}
