using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Games.SchoolActivity {
public class Player : MovingObject {

	public SchoolActivityView view;
	public  int MOVE_X = 30;
	public  int MOVE_Y = 30;
	private bool moving;


	// Use this for initialization
	void Start () {
			base.Start ();
	}

	protected override void AttemptMove<T>(int xDir,int yDir){
		base.AttemptMove<T> (xDir, yDir);
		RaycastHit2D hit;
	}
	
	// Update is called once per frame
	void Update () {
//			if (!view.playersTurn)
//				return;
//
//			int horizontal = 0;
//			int vertical = 0;
//
//			horizontal = (int)Input.GetAxisRaw ("Horizontal");
//			vertical = (int)Input.GetAxisRaw ("Vertical");
//
//			if (horizontal != 0) {
//				horizontal = horizontal * MOVE_X;
//				vertical = 0;
//			}
//
//			if (vertical != 0) {
//				vertical = vertical * MOVE_Y;
//			}
//				
//
//			if (horizontal != 0 || vertical != 0)
//				AttemptMove<BoxCollider2D> (horizontal,vertical);
	}
		public void MoveSequence(Vector2[] instructions){
			for (int i = 0; i < instructions.Length; i++) {
				MoveSanti (instructions [i]);
				//WaitForSeconds(1.0);
			}
		}

		void MoveSanti (Vector2 vector2)
		{
			int horizontal = (int)vector2.x;
			int vertical = (int)vector2.y;

			if (horizontal != 0) {
				horizontal = horizontal * MOVE_X;
				vertical = 0;
			}

			if (vertical != 0) {
				vertical = vertical * MOVE_Y;
			}


			if (horizontal != 0 || vertical != 0)
				AttemptMove<BoxCollider2D> (horizontal,vertical);
		}

		protected override void OnCantMove<T> (T component){
			//lo que pasa cuando chocas con la pared
		}

		private void OnTriggerEnter2D(Collider2D other){
			Debug.Log ("Triggered: "+other.tag);
//			if (other.tag == "Exit") {
//				Invoke ("Restart", restartLevelDelay);
//				enabled = false;
//			} else if (other.tag=="Soda"){
//				food += pointsPerSoda;
//				other.gameObject.SetActive (false);
//			}
		}
}
}
