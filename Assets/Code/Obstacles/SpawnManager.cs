using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    public GameObject obstacle;
    public float obstacleSpawnTime;
    public float obstacleSpawnX;
    public float obstacleSpawnY;
    public float obstacleSpawnZ;
    public int numOfObstacles;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ObstacleSpawn());
    }

    IEnumerator ObstacleSpawn()
    {
        while(true)
        {
            // arbitrary spawn values - will be based on the width of the river
            for(int i=0; i<numOfObstacles;i++)
            {
                Vector3 obstacleSpawn = new Vector3(Random.Range(-5, 5f), 0, Random.Range(-5,0f));
                Instantiate(obstacle, obstacleSpawn, Quaternion.identity);
            }
            yield return new WaitForSeconds(obstacleSpawnTime);
        }
    }
}
