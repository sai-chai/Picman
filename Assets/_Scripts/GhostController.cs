using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostController : MonoBehaviour {
	private NavMeshAgent agent;
	private GameController global;
	public Transform target;
	public float m_stoppingDistance;
	[SerializeField] private GameObject body;
	[SerializeField] private Material[] colors;
	private int colorIndex;
	private Vector3 originalPosition;

	private const float DefaultSpeed = 1.6F;
	private const float ScatterSpeed = 1.1F;

	void Start () {
		originalPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
		agent = gameObject.GetComponent<NavMeshAgent>();
		agent.stoppingDistance = m_stoppingDistance;
		global = GameObject.FindWithTag("GameController").GetComponent<GameController>();
		target = GameObject.FindWithTag("Pacman").transform;
	}
	
	void Update () {
		if(!global.scatter){
			agent.destination = target.position;
		} else{
			if(!body.GetComponent<MeshRenderer>().material.name.Equals("GhostScatter")){
				StartCoroutine(Scatter()); 
			}
			agent.destination = originalPosition;
		}
	}

	// Entirely separate from the GameController's Scatter coroutine
	// This one sets the ghost's color
	private IEnumerator Scatter(){
		SetColor(4);
		agent.speed = ScatterSpeed;
		yield return new WaitForSeconds(12.5f);
		agent.speed = DefaultSpeed;
		SetColor(colorIndex);
	}

	public void SetColor(int index){
		body.GetComponent<MeshRenderer>().material = colors[index];
		if(index < 4 && index >= 0){
			colorIndex = index;
		}
	}
}
