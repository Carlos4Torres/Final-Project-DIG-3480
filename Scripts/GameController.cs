using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public GameObject[] hazards;
	public Vector3 spawnValues;
	public int hazardCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;

	public GUIText scoreText;
	public GUIText restartText;
	public GUIText gameOverText;

    public GUIText hardModeText; //ref to hard mode UI
    public BGScroller bg; //ref to background
    public ParticleSystem ps_slow; // ref to particle systems
    public ParticleSystem ps_slow_dist;
    public ParticleSystem ps_fast;
    public ParticleSystem ps_fast_dist;
    public AudioSource audioSource;
    public AudioClip winMusic;
    public AudioClip loseMusic;

    private int score;
	private bool gameOver;
	private bool restart;
    private bool hard; // hard mode
    private bool musicPlaying = false;

    void Start() {
        ps_fast.Pause();// pause unwanted particles
        ps_fast_dist.Pause();
        hard = false; // set hard to false

        gameOver = false;
		restart = false;
		restartText.text = "";
		gameOverText.text = "";
		score = 0;
		UpdateScore ();
		StartCoroutine(SpawnWaves());
	}

	void Update() {
		if (restart) {
			if (Input.GetKeyDown(KeyCode.P)) {
                SceneManager.LoadScene("Main");
            }            
        }

        if (Input.GetKey("escape"))
            Application.Quit();

        MusicChange();
        HardMode();
    }

    void HardMode()
    {
        if (Input.GetKeyDown(KeyCode.H)) //change hard mode
        {
            hard = !hard;
            if (hard)
            {
                hardModeText.text = "Hard Mode: On";
            }
            else
            {
                hardModeText.text = "Hard Mode: Off";
            }
        }
    }

    void UpdateScore()
    {
        scoreText.text = "Points: " + score;
        if (score >= 100)
        {
            //game won
            gameOverText.text = "GAME CREATED BY CARLOS TORRES";
            gameOver = true;
            restart = true;

            bg.scrollSpeed = -25f;//change background scrolling speed
            ps_fast.Play();//particle systems
            ps_fast_dist.Play();
            Destroy(ps_slow);
            Destroy(ps_slow_dist);
        }
    }

    IEnumerator SpawnWaves() {
		yield return new WaitForSeconds(startWait);
		while(!restart) {
			for (int i = 0; i < hazardCount; ++i) {

                int enemy = Random.Range(0, hazards.Length);

                GameObject hazard = hazards[enemy];
                Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				GameObject clone = Instantiate (hazard, spawnPosition, spawnRotation);

                if (hard && enemy < 3) //hard mode modifier
                {
                    clone.GetComponent<Mover>().speed = -10;
                }

				yield return new WaitForSeconds(spawnWait);
			}
			yield return new WaitForSeconds(waveWait);

			if (gameOver) {
				restartText.text = "Press 'P' for restart";
				restart = true;
			}
		}
	}

	public void AddScore(int newScoreValue) {
		score += newScoreValue;
		UpdateScore ();
	}


	public void GameOver() {
		gameOver = true;
		gameOverText.text = "GAME CREATED BY CARLOS TORRES";
    }

    void MusicChange() //change music when game over
    {
        if (gameOver && !musicPlaying)
        {
            if (score >= 100)
            {
                audioSource.clip = winMusic;
                audioSource.Play();
            }
            else
            {
                audioSource.clip = loseMusic;
                audioSource.Play();
            }
            musicPlaying = true;
        }
    }

}
