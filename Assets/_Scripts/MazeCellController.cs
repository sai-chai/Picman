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
	
	void Start () {
		maze = transform.parent.GetComponent<MazeController>();
		global = GameObject
					.FindWithTag("GameController")
					.GetComponent<GameController>();
		visited = false;
	}

	public void SetCell(int column, int row){
		this.column = column;
		this.row = row;
	}


	private bool isCoin(int spawn){
		maze = transform.parent.GetComponent<MazeController>();
		return spawn % ((maze.rows*maze.columns)/((maze.rows+maze.columns)/3)) == 0;
	}

	// Triggers the actual spawning of coins or dots in the cell based on 
	// the spawn number it's assigned.
	public void SetSpawn(int spawn){
		global = GameObject
					.FindWithTag("GameController")
					.GetComponent<GameController>();
		if(isCoin(spawn)){
			Instantiate(
				coinPrefab, 
				new Vector3(transform.position.x, coinPrefab.transform.position.y, transform.position.z), 
				transform.rotation
			);
		} else {
			Instantiate(
				dotPrefab, 
				new Vector3(transform.position.x, dotPrefab.transform.position.y, transform.position.z), 
				transform.rotation
			);
			global.addDot();
		}
		this.spawn = spawn;
	}
	
	// Deletes an adjacent wall. Is used exclusively by the maze
	// generation algorithm.
	public void MoveUp(){
		if(walls[0] != null){
			walls[0].SetActive(false);
		}
	}

	public void MoveDown(){
		if(walls[2] != null){
			walls[2].SetActive(false);
		}
	}

	public void MoveLeft(){
		if(walls[3] != null){
			walls[3].SetActive(false);
		}
	}

	public void MoveRight(){
		if(walls[1] != null){
			walls[1].SetActive(false);
		}
	}

	// Also used exclusively by the maze generation algorithm.
	// Since it's a DFS graph algorithm, the cell's visited state
	// has to be cached for backtracking.
	public void Visit(){
		visited = true;
	}
}
