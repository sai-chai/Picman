using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class MazeController : MonoBehaviour {
	public bool isReady;
	public System.Random random;
	public GameObject pacmanCell;
	private int[] directions;
	private int[] spawns;
	private ArrayList[,] adj;
	[SerializeField] public int columns;
	[SerializeField] public int rows;
	[SerializeField] private GameObject mazeCellPrefab;

	// Use this for initialization
	void Start () {
		spawns = new int[columns*rows];
		for(int i = 0; i < spawns.Length; i++){
			spawns[i] = i;
		}
		directions = new int[]{1,2,3,4};
		random = new System.Random();
		Shuffle<int>(spawns);
		adj = new ArrayList[columns,rows];
		for(int z = 0; z < adj.GetLength(0); z++){
			for(int x = 0; x < adj.GetLength(1); x++){
				adj[z,x] = new ArrayList();
				GameObject temp = Instantiate(mazeCellPrefab, new Vector3(x*2f+1f, 0f, z*2f+1f), Quaternion.identity);
				temp.transform.parent = transform;
				temp.GetComponent<MazeCellController>().SetCell(z, x);
				adj[z,x].Insert(0, temp);
			}
		}
		for(int z = 0; z < adj.GetLength(0); z++){
			for(int x = 0; x < adj.GetLength(1); x++){
				if(x > 0){
					adj[z,x].Add(adj[z,x-1][0]);
				}
				if(z < adj.GetLength(0)-1){
					adj[z,x].Add(adj[z+1,x][0]);
				}
				if(x < adj.GetLength(1)-1){
					adj[z,x].Add(adj[z,x+1][0]);
				}
				if(z > 0){
					adj[z,x].Add(adj[z-1,x][0]);
				}
			}
		}
		pacmanCell = GetStartCell();
		int j = 0;
		for(int z = 0; z < adj.GetLength(0); z++){
			for(int x = 0; x < adj.GetLength(1); x++){
				if((GameObject) adj[z,x][0] != pacmanCell){
					((GameObject)adj[z,x][0]).GetComponent<MazeCellController>().SetSpawn(spawns[j]);
				}
				j++;
			}
		}
		Generate(pacmanCell.GetComponent<MazeCellController>());
	}

	void Update(){
		if(!isReady){
			//bake navmesh
			NavMeshSurface surface = gameObject.GetComponent<NavMeshSurface>();
			surface.BuildNavMesh();
			isReady = true;
		}
	}

	private void Generate(MazeCellController current){
		current.Visit();
		ArrayList currentList = adj[current.column,current.row];
		int[] neighbors = (int[]) directions.Clone();
		Shuffle<int>(neighbors);
		foreach(int d in neighbors){
			if(d < currentList.Count){
				MazeCellController next = ((GameObject) currentList[d]).GetComponent<MazeCellController>();
				if(!next.visited){
					if(next.column > current.column){
						current.MoveRight();
						next.MoveLeft();
					}else if(next.column < current.column){
						current.MoveLeft();
						next.MoveRight();
					}else if(next.row > current.row){
						current.MoveDown();
						next.MoveUp();
					}else if(next.row < current.row){
						current.MoveUp();
						next.MoveDown();
					}
					Generate(next);
				}
			}
		}
	}

	public void Shuffle<T> (T[] array)
    {
        int n = array.Length;
        while (n > 1) 
        {
            int k = random.Next(n--);
            T temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }

	public int GetRandom(){
		return random.Next();
	}

	public int GetRandom(int max){
		return random.Next(max);
	}

	private GameObject GetStartCell(){
		int column = random.Next(adj.GetLength(0));
		int row = -1;
		if(column == 0 || column == adj.GetLength(0)-1){
			row = random.Next(adj.GetLength(1));
		} else {
			row = random.Next(2);
			if(row == 1){
				row = adj.GetLength(1)-1;
			}
		}
		return (GameObject) adj[column,row][0];
	}

	public GameObject ResetStartCell(){
		pacmanCell = GetStartCell();
		return pacmanCell;
	}

	public GameObject GetCellAt(int column, int row){
		return (GameObject) adj[column,row][0];
	}
}
