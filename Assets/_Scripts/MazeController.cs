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

	void Start () {
		spawns = new int[columns*rows];
		for(int i = 0; i < spawns.Length; i++){
			spawns[i] = i;
		}
		directions = new int[]{1,2,3,4};
		random = new System.Random();
		Shuffle<int>(spawns);
		adj = new ArrayList[columns,rows];
		// Generation of a blank maze with no edges
		// note: z is used for columns and x for rows because those are the axes 
		// in which the two coordinates sit.
		for(int z = 0; z < adj.GetLength(0); z++){
			for(int x = 0; x < adj.GetLength(1); x++){
				adj[z,x] = new ArrayList();
				GameObject temp = Instantiate(mazeCellPrefab, new Vector3(x*2f+1f, 0f, z*2f+1f), Quaternion.identity);
				temp.transform.parent = transform;
				temp.GetComponent<MazeCellController>().SetCell(z, x);
				// Addition of the origin cell to each empty arraylist in the adjacency matrix
				adj[z,x].Insert(0, temp);
			}
		}
		// Filling of adjacency matrix. 
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
		// coins/dots are spawned in each cell
		// j is used to give each cell a different spawn number from the shuffled spawns array
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
			// NavMesh baking in the Start call caused a race condition so I moved it to update
			// Need .NET 5+ to use async/await, update to come
			NavMeshSurface surface = gameObject.GetComponent<NavMeshSurface>();
			surface.BuildNavMesh();
			isReady = true;
		}
	}

	// The actual recursive maze traversal algorithm. 
	// Adjacent cells are visited in a random order.
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

	// GetStartCell defines the starting point for the player-character
	// as one of the outermost cells 
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
