using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	Vector3 dir, nextDir;
	float speed, speedDecay = 2f, minSpeed = 0.1f, startSpeed = 20;
	bool recalling, disappearing;
	Transform recallTarget;
	public SpriteRenderer bulletRend;
	public void Setup (Vector3 _dir) { 
		dir = _dir; //passed in from player
		speed = startSpeed; //start moving
	}
	void FixedUpdate () {
		Move(); //move the bullet
		CheckDisappear(); //get rid of bullet after it stops moving
	}
	void Move(){
		speed -= speedDecay * speed * Time.fixedDeltaTime; //slow down the bullet over time
		if (speed < minSpeed){
			speed = 0; //clamp down speed so it doesnt take too long to stop
		}
		Vector3 tempPos = transform.position; //capture current position
		tempPos += dir * speed * Time.fixedDeltaTime; //find new position
		transform.position = tempPos; //update position
	}
	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Wall")){
			speed = 0; //stop if it hits a wall
		}
	}
	void CheckDisappear(){
		if (speed == 0 && !disappearing){ //disappear and destroy when stopped
			disappearing = true; //so we dont continuelly call the coroutine
			StartCoroutine(Disappear());
		}
	}
	IEnumerator Disappear(){
		float curAlpha = 1; //start at full alpha
		float disSpeed = 3f; //take 1/3 seconds to disappear
		Color disColor = bulletRend.color; //capture color to edit its alpha
		do{
			curAlpha -= disSpeed * Time.deltaTime; //find new alpha
			disColor.a = curAlpha; //apply alpha to color
			bulletRend.color = disColor; // apply color to bullet
			yield return null;
		}while (curAlpha > 0); //end when the bullet is transparent
		Destroy(gameObject); //get rid of bullet now that it can't be seen
	}
}