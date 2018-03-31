using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MazeCellController : MonoBehaviour {

	public int column; // z-coordinate
	public int row; // x-coordinate
	public bool visited;
	private int spawn;
	private GameController global;
	private MazeController maze;
	[SerializeField] private GameObject coinPrefab;
	[SerializeField] private GameObject dotPrefab;
	[SerializeField] private GameObject[] walls;
	// 0 = upper, 1 = right, 2 = lower, 3 = left

	// Use this for initialization
	void Start () {
		maze = transform.parent.GetComponent<MazeController>();
		global = GameObject.FindWithTag("GameController").GetComponent<GameController>();
		visited = false;
		if(spawn % ((maze.rows*maze.columns)/((maze.rows+maze.columns)/3)) != 0){
			global.dotCount++;
		}
	}

	public void SetCell(int column, int row){
		this.column = column;
		this.row = row;
	}

	public void SetSpawn(int spawn){
		maze = transform.parent.GetComponent<MazeController>();
		if(spawn % ((maze.rows*maze.columns)/((maze.rows+maze.columns)/3)) == 0){
			Instantiate(coinPrefab, new Vector3(transform.position.x, coinPrefab.transform.position.y, transform.position.z), transform.rotation);
		} else {
			Instantiate(dotPrefab, new Vector3(transform.position.x, dotPrefab.transform.position.y, transform.position.z), transform.rotation);
		}
		this.spawn = spawn;
	}

	public void MoveUp(){
		if(walls[0] != null){
			//Destroy(walls[0]);
			walls[0].SetActive(false);
		}
	}

	public void MoveDown(){
		if(walls[2] != null){
			//Destroy(walls[2]);
			walls[2].SetActive(false);
		}
	}

	public void MoveLeft(){
		if(walls[3] != null){
			//Destroy(walls[3]);
			walls[3].SetActive(false);
		}
	}

	public void MoveRight(){
		if(walls[1] != null){
			//Destroy(walls[1]);
			walls[1].SetActive(false);
		}
	}

	public void Visit(){
		visited = true;
	}
}
