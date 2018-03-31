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
	public bool scatter{
		get{
			return _scatter;
		}
	}
	public int lives;
	public int dotTotal;
	public int dotCount;
	public int score;
	private bool navMeshJerryRig;

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
		dotCount = 0;
		score = 0;
		dotTotal = 0;
		_scatter = false;
		navMeshJerryRig = false;	
	}

	void Update(){
		if(!this.navMeshJerryRig && maze.isReady){
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
			dotTotal = dotCount;
			this.navMeshJerryRig = true;
			Time.timeScale = 0;
		}

		if(Time.timeScale == 0 && directions.gameObject.activeSelf){
			if(Input.anyKey){
				directions.gameObject.SetActive(false);
				anyKey.gameObject.SetActive(false);
				Time.timeScale = 1;
			}
		}

		if(lives <= 0 && !directions.gameObject.activeSelf && !gameOverText.gameObject.activeSelf){
			Time.timeScale = 0;
			gameOverText.gameObject.SetActive(true);
			gameOverText.text = gameOverText.text + score.ToString();
		}
		if(score >= dotTotal && !directions.gameObject.activeSelf && !wonText.gameObject.activeSelf){
			Time.timeScale = 0;
			wonText.gameObject.SetActive(true);
		}
		dotCounter.text = "Dots: " + score.ToString() + "/" + dotTotal.ToString();
		lifeCounter.text = "Lives: " + lives;
	}

	private Vector3 SpawnPosition(GameObject prefab, Transform location){
		return new Vector3(location.position.x, prefab.transform.position.y, location.position.z);
	}

	public IEnumerator Scatter(){
		_scatter = true;
		yield return new WaitForSeconds(12f);
		_scatter = false;
	}

	
}
