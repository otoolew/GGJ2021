using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Applies force to object
public class BlastForce : MonoBehaviour
{
    [SerializeField]
    Vector2 forceXRange;

    [SerializeField]
    Vector2 forceYRange;

    [SerializeField]
    Vector2 forceTorqueRange;

    Rigidbody2D rb2D;

    private void Start()
    {
        Vector2 randomForceDirection = new Vector2(Random.Range(forceXRange.x, forceXRange.y), Random.Range(forceYRange.x, forceYRange.y));
        float randomTorque = Random.Range(forceTorqueRange.x, forceTorqueRange.y);

        rb2D = GetComponent<Rigidbody2D>();
        rb2D.AddForce(randomForceDirection);
        rb2D.AddTorque(randomTorque);
    }

    void Update()
    {
        //transform.LookAt(Camera.main.transform.position, -Vector3.up);
        //transform.LookAt(Camera.main.transform.position);
    }
}