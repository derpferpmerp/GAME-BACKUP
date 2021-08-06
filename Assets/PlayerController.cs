using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public class PlayerController : MonoBehaviour {
	public float speed = 5f;
	private float xspeed = 0f;
	private float yspeed = 0f;
	private float acceleration = -5;
	private float maxv = 9.8f;
	public bool intheair = false;
	public bool falling = true;
	private float oldvelocity = 0f;
	private float timechange = 0f;
	private double distancet = 0f;
	public double rby, rbyval = 0f;
	public bool rising = false;
	public float arrowcooldown = 0f;
	public GameObject[] players;
	public List<Rigidbody2D> rigidbodies = new List<Rigidbody2D>();
	public Collider2D[] playercollider;
	public Collider2D[] climbables;
	public bool canclimb = false;
	// Use this for initialization
	void Start () {
		foreach (GameObject i in players) {
			rigidbodies.Add(i.GetComponent<Rigidbody2D> ());
		}
	}

	// Update is called once per frame
	void Update () {
		for (int i = 0; i <= playercollider.Length; i = i + 2) {
			canclimb = (ifObjectColliding(playercollider.ElementAt(i), climbables));
			rigidbodies.ElementAt(i).velocity = CalcBody(rigidbodies.ElementAt(i), canclimb);
		}
	}

	public Vector2 CalcBody(Rigidbody2D rigidBody, bool CanClimb) {
		//bool CanClimb = ifObjectColliding(obj, ladder)
		arrowcooldown = arrowcooldown + 1;
		if (arrowcooldown == 10) {
			arrowcooldown = 0;	
		}
		//this.transform.position = new Vector3(
		//	followTransform.position.x,
		//	followTransform.position.y,
		//	this.transform.position.z
		//);
		xspeed = Input.GetAxis("Horizontal");
		yspeed = Input.GetAxis("Vertical");
		if (rigidBody.velocity.y > 0) {
			intheair = true;
		}
		if (!intheair) {
			falling = false;
		}
		if (intheair == true && oldvelocity != rigidBody.velocity.y) {
			if (rigidBody.velocity.y < 0) {
				falling = true;
			} else if (Math.Round(rigidBody.velocity.y) == 0) {
				if (falling == true) {
					falling = false;
					intheair = false;
					rising = false;
				}
			} else {
				rising = true;
			}
		}
		timechange = Time.deltaTime;
		distancet = (oldvelocity * timechange) + (0.5 * maxv * (timechange * timechange));
		rby = Math.Round(((oldvelocity * oldvelocity) + (2 * maxv) + (2 * distancet)),3);
		rbyval = Math.Round(Math.Sqrt(rby),3);

		if (xspeed > 0f) {
			if (CanClimb) {
				rigidBody.velocity = new Vector2 (xspeed * speed, yspeed * speed);
			} else {
				rigidBody.velocity = new Vector2 (xspeed * speed, acceleration);
			}
		}
		else if (xspeed < 0f) {
			if (CanClimb) {
				rigidBody.velocity = new Vector2 (xspeed * speed, yspeed * speed);
			} else {
				rigidBody.velocity = new Vector2 (xspeed * speed, acceleration);
			}
		} 
		else {
			if (CanClimb) {
				rigidBody.velocity = new Vector2 (0, yspeed * speed);
			} else {
				rigidBody.velocity = new Vector2 (0, acceleration);
			}
		}
		oldvelocity = rigidBody.velocity.y;
		return GetComponent<Rigidbody2D>().velocity;
	}

	bool ifObjectColliding(Collider2D Projectile, Collider2D[] ObjectList) {
		foreach (Collider2D i in ObjectList) {
			if (Projectile.IsTouching(i)) {
				return true;
			}
		}
		return false;
	}
}
