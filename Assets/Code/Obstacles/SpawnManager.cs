using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField] private GameObject[] obstacleList = new GameObject[3];
    [SerializeField] private GameObject obstacle;
    [SerializeField] private float obstacleSpawnTime;
    [SerializeField] private int numOfObstacles;

    [SerializeField] private float spawnWidthLeft;
    [SerializeField] private float spawnWidthRight;
    [SerializeField] private float despawnPoint;

    // Start is called before the first frame update
    private void Start()
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
                obstacle = obstacleList[Random.Range(0, 3)];
                Vector3 obstacleSpawn = new Vector3(Random.Range(spawnWidthLeft, spawnWidthRight), -3f, Random.Range(-40,-80f));
                Instantiate(obstacle, obstacleSpawn, Quaternion.identity);
                //obstacle.transform.Rotate(0, Random.Range(-45, 45), 0, Space.Self);
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
