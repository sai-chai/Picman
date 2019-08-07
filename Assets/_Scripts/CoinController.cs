using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour {

	public float speed;
	
	void Update () {
		// Rotates the coin. Coin's local xz-plane is the game's xy-plane.
		// Hence, the use of vector 0,1,0.
		transform.Rotate(Vector3.up, speed * Time.deltaTime);
	}
}
