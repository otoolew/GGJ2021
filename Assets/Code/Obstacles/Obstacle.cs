using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Obstacle : MonoBehaviour
{

    [SerializeField] private float obstacleSpeed;

    [SerializeField] private GameObject slayedEffect;
    public GameObject SlayEffect { get => slayedEffect; set => slayedEffect = value; }

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

            //OLD game reset
            //string currentSceneName = SceneManager.GetActiveScene().name;
            //SceneManager.LoadScene(currentSceneName);

            //stop timer and capture mark
            string endGameTime = Timer.instance.timePlayingStr;
            Timer.instance.EndTimer();

            FindObjectOfType<GameMode>().ResetGame();
        }

        if (other.gameObject.tag == ("Player") && (this.gameObject.tag == "Enemy"))
        {
            Debug.Log("Player Hit Enemy " + gameObject.name);
            PlayerCharacter pc = other.GetComponent<PlayerCharacter>();
            if (pc)
            {
                if (pc.IsAttacking)
                {
                    // Coroutine to do Slay FX
                    StartCoroutine(SlayedRoutine());

                    return;
                }

                if (pc.PlayerHitSFX.clip != null)
                {
                    pc.PlayerHitSFX.Play();
                }
            }
            //string currentSceneName = SceneManager.GetActiveScene().name;
            //SceneManager.LoadScene(currentSceneName);

            string endGameTime = Timer.instance.timePlayingStr;
            Timer.instance.EndTimer();

            FindObjectOfType<GameMode>().ResetGame();
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

        IEnumerator SlayedRoutine()
        {
            if (slayedEffect != null) 
            {
                slayedEffect.SetActive(true);

                yield return new WaitForSeconds(0.25f);
                
                slayedEffect.SetActive(false);

                PlayerCharacter pc = other.GetComponent<PlayerCharacter>();

                pc.CollectSoul();
                
                if (pc.CollectSoulSFX.clip != null)
                {
                    pc.CollectSoulSFX.Play();
                }

                Destroy(this.gameObject);
            }
        }
    }
}
