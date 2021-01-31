using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField] private GameObject[] toSpawnlist = new GameObject[4];

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
                //obstacle = obstacleList[Random.Range(0, 3)];
                Vector3 obstacleSpawn = new Vector3(Random.Range(spawnWidthLeft, spawnWidthRight), 2f, Random.Range(-40,-80f));
                Instantiate(toSpawnlist[Random.Range(0,4)], obstacleSpawn, Quaternion.identity);
            }
            yield return new WaitForSeconds(obstacleSpawnTime);
        }
    }

    private void Update()
    {
        destroyObstacle();
    }

    //determine which object overlapped and then do the thing

    private void destroyObstacle()
    {
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Spawnable"))
        {
            if(obj.transform.position.z >= despawnPoint)
            {
                Destroy(obj);
            }
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Soul"))
        {
            if (obj.transform.position.z >= despawnPoint)
            {
                Destroy(obj);
            }
        }
    }
}
