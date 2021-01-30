using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    public GameObject obstacle;
    public GameObject enemy;
    public float obstacleSpawnTime;
    public int numOfObstacles;

    public float spawnWidthLeft;
    public float spawnWidthRight;

    public float despawnPoint;

    private GameObject[] spawnableList = new GameObject[2];

    // Start is called before the first frame update
    private void Start()
    {
        spawnableList[0] = obstacle;
        spawnableList[1] = enemy;
        StartCoroutine(ObstacleSpawn());
    }

    IEnumerator ObstacleSpawn()
    {
        while(true)
        {
            // arbitrary spawn values - will be based on the width of the river
            for(int i=0; i<numOfObstacles;i++)
            {
                Vector3 obstacleSpawn = new Vector3(Random.Range(spawnWidthLeft, spawnWidthRight), 0, Random.Range(-40,-80f));
                Instantiate(spawnableList[Random.Range(0,2)], obstacleSpawn, Quaternion.identity);
            }
            yield return new WaitForSeconds(obstacleSpawnTime);
        }
    }

    private void Update()
    {
        destroyObstacle();
    }

    private void destroyObstacle()
    {
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Spawnable"))
        {
            if(obj.transform.position.z >= despawnPoint)
            {
                Destroy(obj);
            }
        }
    }
}
