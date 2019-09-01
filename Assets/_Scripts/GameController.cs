using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class GameController : MonoBehaviour {
	[SerializeField] private MazeController maze;
	[SerializeField] private GameObject pacmanPrefab;
	[SerializeField] private GameObject ghostPrefab;
	[SerializeField] private Text lifeCounter;
	[SerializeField] private Text dotCounter;
	[SerializeField] private Text gameOverText;
	[SerializeField] private Text wonText;
	[SerializeField] private Text directions;
	[SerializeField] private Text anyKey;

	private static GameController singleton;
	private bool _scatter;
	private int _lives;
	private int dotTotal;
	private int _dotCount;
	private int _score;
	private bool isFirstUpdate;
	private const int StartingLives = 5;

	public bool scatter{
		get{
			return _scatter;
		}
	}

	public int lives{
		get{
			return lives;
		}
	}

	public int dotCount{
		get{
			return _dotCount;
		}
	}

	public int score{
		get{
			return _score;
		}
	}

	// Implementation of singleton pattern
	private void Awake () {
		if (singleton == null)
		{
			DontDestroyOnLoad(gameObject);
			singleton = this;
		}
		else if (singleton != this)
		{
			Destroy (gameObject);
		}
	}

	void Start(){
		_dotCount = 0;
		_score = 0;
		dotTotal = 0;
		_lives = StartingLives;
		_scatter = false;
		/* 
		 * the nonexistence of the NavMesh for the ghosts at the Start phase
		 * forced me to move the code to the Update phase. Hence, isFirstUpdate?
		 */  
		isFirstUpdate = true;	
	}

	void Update(){
		// On first update, Pacman is spawned on one of the outer cells
		// Ghosts are spawned on the four centermost cells
		if(this.isFirstUpdate && maze.isReady){
			Instantiate(pacmanPrefab, 
						SpawnPosition(pacmanPrefab, maze.pacmanCell.transform),
						Quaternion.identity);
			int c_quarter = maze.columns/4;
			int r_quarter = maze.rows/4;
			GameObject ghost = Instantiate(ghostPrefab, 
						SpawnPosition(ghostPrefab, maze.GetCellAt(c_quarter,r_quarter).transform),
						Quaternion.identity);
			ghost.GetComponent<GhostController>().SetColor(0);
			ghost = Instantiate(ghostPrefab, 
						SpawnPosition(ghostPrefab, maze.GetCellAt(c_quarter, r_quarter*3).transform),
						Quaternion.identity);
			ghost.GetComponent<GhostController>().SetColor(1);
			ghost = Instantiate(ghostPrefab, 
						SpawnPosition(ghostPrefab, maze.GetCellAt(c_quarter*3,r_quarter).transform),
						Quaternion.identity);
			ghost.GetComponent<GhostController>().SetColor(2);
			ghost = Instantiate(ghostPrefab, 
						SpawnPosition(ghostPrefab, maze.GetCellAt(c_quarter*3,r_quarter*3).transform),
						Quaternion.identity);
			ghost.GetComponent<GhostController>().SetColor(3);
			dotTotal = _dotCount;
			this.isFirstUpdate = false;
			Time.timeScale = 0;
		}

		// Unpause if any key is pressed on the pause screen.
		if(Time.timeScale == 0 && directions.gameObject.activeSelf){
			if(Input.anyKey){
				directions.gameObject.SetActive(false);
				anyKey.gameObject.SetActive(false);
				Time.timeScale = 1;
				return;
			}
		}

		// If you're out of lives and the game directions and gameover messages 
		// aren't already shown, display the game over text along with the score.
		if(_lives <= 0 && !directions.gameObject.activeSelf && !gameOverText.gameObject.activeSelf){
			Time.timeScale = 0;
			gameOverText.gameObject.SetActive(true);
			gameOverText.text = gameOverText.text + _score.ToString();
		}

		// If you've beat the game and all the dots are collected, show the game won text.
		if(_score >= dotTotal && !directions.gameObject.activeSelf && !wonText.gameObject.activeSelf){
			Time.timeScale = 0;
			wonText.gameObject.SetActive(true);
		}

		// Update dot and life counters
		dotCounter.text = "Dots: " + _score.ToString() + "/" + dotTotal.ToString();
		lifeCounter.text = "Lives: " + _lives;
	}

	// The local y-positions of prefabs are already set to the proper height
	// All that's needed are the xz-coordinates
	private Vector3 SpawnPosition(GameObject prefab, Transform location){
		return new Vector3(location.position.x, prefab.transform.position.y, location.position.z);
	}

	public IEnumerator Scatter(){
		_scatter = true;
		yield return new WaitForSeconds(12f);
		_scatter = false;
	}

	// Methods that limit setting of private fields to incrementing and decrementing
	public void subtractLife() {
		_lives--;
	}

	public void addDot() {
		_dotCount++;
	}
	
	public void subtractDot() {
		_dotCount--;
	}

	public void addPoint() {
		_score++;
	}

	public void addCoinPoints() {
		_score += 5;
	}

	public void addGhostPoints() {
		_score += 10;
	}

	public void addLife() {
		_lives++;
	}

}
