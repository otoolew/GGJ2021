using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Obstacle : MonoBehaviour
{

    [SerializeField] private float obstacleSpeed;

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
        if(other.gameObject.tag == ("Spawnable") && other.gameObject.transform.position.z <= -35f)
        {
            //Debug.Log("Overlap detected - deleting");
            Destroy(this.gameObject);
        }

        if(other.gameObject.tag == ("Player") && (this.gameObject.tag == "Spawnable"))
        {
            Debug.Log("Player hit!");
            PlayerCharacter pc = other.GetComponent<PlayerCharacter>();
            if (pc)
            {
                if(pc.PlayerHitSFX.clip != null)
                {
                    pc.PlayerHitSFX.Play();
                }
            }
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }

        if (other.gameObject.tag == ("Player") && (this.gameObject.tag == "Enemy"))
        {
            Debug.Log("Player Hit Enemy " + gameObject.name);
            PlayerCharacter pc = other.GetComponent<PlayerCharacter>();
            if (pc)
            {
                if (pc.IsAttacking)
                {
                    Destroy(this.gameObject);
                    return;
                }

                if (pc.PlayerHitSFX.clip != null)
                {
                    pc.PlayerHitSFX.Play();
                }
            }
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }

        if (other.gameObject.tag == ("Player") && (this.gameObject.tag == "Soul"))
        {
            PlayerCharacter pc = other.GetComponent<PlayerCharacter>();
            if (pc)
            {
                pc.CollectSoul();
                if (pc.CollectSoulSFX.clip != null)
                {
                    pc.CollectSoulSFX.Play();
                }
            }

            Debug.Log("Soul acquired!");
            Destroy(this.gameObject);
        }
    }
}
