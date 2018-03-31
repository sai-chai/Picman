using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour {

	public float speed;
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.up, speed * Time.deltaTime);
	}

	void OnTriggerEnter(Collider other){
		if(other.gameObject.CompareTag("Pacman")){
			Destroy(gameObject);
		}
	}
}
