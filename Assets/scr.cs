using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
// 2D MoveTowards example
// Move the sprite to where the mouse is clicked
//
// Set speed to -1.0f and the sprite will move
// away from the mouse click position forever

public class scr : MonoBehaviour {

	// ------- MOVEMENT ------- //

	public float speed = 50.0f;		// Movement Speed of Arrow (Arbitrary Units)
	private Vector2 target; 		// The Target Position in Vector2 Format (x,y)
	private Vector2 position; 		// The Current Position of the Arrow in Vector2 Format (x,y)
	private GameObject player; 		// The Current Position of the Player (-> Shoot Location)
	private GameObject ENEMY; 		// The Current Position of the Enemy (-> Shoot Target)
	public Camera cam; 				// The Current Position of the Enemy (-> Shoot Target)
	private float itrvar = 0f;		// Iterable Variable (Arrow Return Delay)

	
	// ------- Storage Vars ------- //
	private bool comeback = false;
	private bool chasing = false;
	private Vector2 PlayerPosition;

	
	// ------- Arrow Collision ------- //
	public Collider2D mainarrowcollider;
	private float collided = 0f;
	private float collidedcounter = 0f;
	private bool atdestination = false;
	private float distancetmp = 0f;
	public GameObject arr;

	
	// ------- Enemy Collision ------- //
	public Collider2D[] enemycolliders;
	
	
	// ------- Floor Collision ------- //
	public Collider2D[] floorcolliders;
	private float collidedwfloor = 0f;
	private float collidedcounter_floor = 0f;
	

	// ------- GUI Setup ------- //
	public Text CollisionsGUI;
	public Text TotalShotsGUI;
	public Text AccuracyGUI;
	private float totalshots = 0.0f;

	// ------- Graphics Setup ------- //
	public Vector3 TankOffset = new Vector3(0.0f,1f,0.0f);

	void Start() {
		target = new Vector2(0.0f, 0.0f);
		position = gameObject.transform.position;
		cam = Camera.main;
		PlayerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
		CollisionsGUI.text = "Collisions: " + collidedcounter;
		TotalShotsGUI.text = "Total Shots: " + totalshots;
		AccuracyGUI.text = "Accuracy: " + 0.0f;
	}

	void Update() {
		cam = Camera.main;
		PlayerPosition = GameObject.FindGameObjectWithTag("Player").transform.position + TankOffset;
		distancetmp = GOD(transform.position,PlayerPosition);
		if (chasing) {
			itrvar += 1;
			transform.rotation = LookAtTarget(
				new Vector3(
					target.x,
					target.y,
					0.0f
				) - transform.position
			);
		}
		if (itrvar == 1000) {
			if (chasing) {
				comeback = true;
			}
			itrvar = 0;
		}
		float step = speed * Time.deltaTime;
		// move sprite towards the target location
		if (!comeback && chasing) {
			transform.position = Vector2.MoveTowards(transform.position, target, step);
			foreach (Collider2D i in enemycolliders) {
				if (ifObjectColliding(mainarrowcollider,i)) {
					collided += 1f;
				}
			}
			if (collided <= 0f) {
				foreach (Collider2D i in floorcolliders) {
					if (ifObjectColliding(mainarrowcollider,i)) {
						collidedwfloor += 1f;
					}
				}
			}
			if (new Vector2(target.x,target.y) == new Vector2(transform.position.x,transform.position.y)) {
				if (atdestination == false) {
					totalshots += 1;
					TotalShotsGUI.text = "Total Shots: " + totalshots;
				}
				atdestination = true;
			}
		} else if (comeback || !VectorEqual(transform.position,PlayerPosition)) {
			transform.position = Vector2.MoveTowards(
				transform.position,
				PlayerPosition,
				step
			);

			chasing = false;
			comeback = true;
			atdestination = false;
			if (collided > 0f) {
				collidedcounter += 1f;
				CollisionsGUI.text = "Collisions: " + collidedcounter;
				collided = 0f;
			}
			if (collidedwfloor > 0f) {
				collidedcounter_floor += 1f;
				collidedwfloor = 0f;
			}
		}
		if (VectorEqual(transform.position,PlayerPosition) || distancetmp <= 0.2f) {
			AccuracyGUI.text = "Accuracy: " + PercentAccuracy(collidedcounter, totalshots) + "%";
			GetComponent<Renderer>().enabled = false;
		} else {
			GetComponent<Renderer>().enabled = true;
		}
		
		
	}

	void OnGUI() {
		Event currentEvent = Event.current;
		Vector2 mousePos = new Vector2();
		Vector2 point = new Vector2();

		mousePos.x = currentEvent.mousePosition.x;
		mousePos.y = cam.pixelHeight - currentEvent.mousePosition.y;
		point = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0.0f));
		
		if (Input.GetMouseButtonDown(0)) {
			if (VectorEqual(transform.position,PlayerPosition) || distancetmp <= 0.2f) {
				target = point;
				chasing = true;
				comeback = false;
			}
		} else {
			
			if (collided > 1f || collidedwfloor > 1f) {
				target = new Vector2(transform.position.x,transform.position.y);
				transform.rotation = LookAtTarget(new Vector3(transform.position.x, transform.position.y, 0.0f));
			} else {
				transform.rotation = LookAtTarget(new Vector3(point.x, point.y, 0.0f));
			}
		}
	}

	public static Quaternion LookAtTarget(Vector2 r) {
		return Quaternion.Euler(0, 0, Mathf.Atan2(r.y, r.x) * Mathf.Rad2Deg);
	}

	bool ifObjectColliding(Collider2D Projectile, Collider2D Object) {
		if (Projectile.IsTouching(Object)) {
			return true;
		} else {
			return false;
		}
	}

	bool VectorEqual(Vector2 v1, Vector2 v2) {
		if (new Vector2(v1.x,v1.y) == new Vector2(v2.x,v2.y)) {
			return true;
		} else {
			return false;
		}
	}

	float GOD(Vector2 vectorA, Vector2 vectorB) {
		float b = vectorA.x - vectorB.x;
		float c = vectorA.y - vectorB.y;
		float a = Mathf.Sqrt((b * b) + (c * c));
		float nval = Mathf.Ceil(Mathf.Abs(Mathf.Log10(a)));
		float outval = Mathf.Round(Mathf.Pow(10f,nval) * a) / (Mathf.Pow(10f,nval));
		if (!(outval >= 0)) {
			outval = 0;
		}
		return outval;
	}

	float PercentAccuracy(float amount, float total) {
		if (amount > 0 && total != 0) {
			return Mathf.Round((100 * (amount/total)));
		} else {
			return 0;
		}
	}
}