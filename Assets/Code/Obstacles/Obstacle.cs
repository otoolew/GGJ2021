using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Obstacle : MonoBehaviour
{

    [SerializeField] private float obstacleSpeed;

    [SerializeField] private GameObject slayedEffect;
    public GameObject SlayEffect { get => slayedEffect; set => slayedEffect = value; }

    private int enemiesSlayed;

    private bool gameReset;
    public bool GameOver { get => gameReset; set => gameReset = value; }

// Start is called before the first frame update
    void Start()
    {
        enemiesSlayed = 0;
        gameReset = false;
    }

    // Update is called once per frame
    private void Update()
    {
        transform.Translate(0, 0, obstacleSpeed * Time.deltaTime);
    }

    //Kati note on 2/4 - need/want to refactor this function A LOT
    private void OnTriggerEnter(Collider other)
    {
        //delete if we spawn overlaps before getting on screen
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
            GameOver = true;

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

                    //track # of enemies slayed
                    enemiesSlayed += enemiesSlayed;
                    Debug.Log("Enemies Slayed: " + enemiesSlayed);

                    return;
                }

                if (pc.PlayerHitSFX.clip != null)
                {
                    pc.PlayerHitSFX.Play();
                }
            }
            //string currentSceneName = SceneManager.GetActiveScene().name;
            //SceneManager.LoadScene(currentSceneName);
            GameOver = true;

            string endGameTime = Timer.instance.timePlayingStr;
            Timer.instance.EndTimer();

            FindObjectOfType<GameMode>().ResetGame();
        }

        if (other.gameObject.tag == ("Player") && (this.gameObject.tag == "Soul"))
        {
            PlayerCharacter pc = other.GetComponent<PlayerCharacter>();

            if(GameOver == true)
            {
                return;
            }

            if (GameOver)
            {
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
