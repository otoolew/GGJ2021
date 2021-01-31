using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{

    public float obstacleSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        transform.Translate(0, 0, obstacleSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        //delete if we spawn overlaps
        if((other.gameObject.tag == ("Spawnable")))
        {
            Debug.Log("Overlap detected - deleting");
            Destroy(this.gameObject);
        }
    }
}
