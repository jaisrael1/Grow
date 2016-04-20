using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Cloud : MonoBehaviour {

	public Controller c;
	public EnvironmentManager em;
	public List<Hex> cloudParts;

	public float timeSinceLastMoved;
	public const float STEP_INTERVAL = 3f;

	public int length;
	public int updates;
	public int coordY;

	public void init (EnvironmentManager em, int coordY, int length){
		this.em = em;
		c = em.c;
		this.coordY = coordY;
		this.length = length;
	}

	void Start() {
		timeSinceLastMoved = 0f;
		cloudParts = new List<Hex> ();
		updates = 0;
	}

	public void shrink(){
		foreach (Hex i in cloudParts) {
			i.removeCloudByShrinking ();
		}
		Destroy (this.gameObject);
	}

	void Update(){
		timeSinceLastMoved += Time.deltaTime;
		if (timeSinceLastMoved > STEP_INTERVAL) {
			updates++;
			List<Hex> newList = new List<Hex> ();

			foreach (Hex i in cloudParts) {
				if (i.coordX > -Controller.WORLD_WIDTH / 2) {
					if (i.coordY == this.coordY) {
						newList.Add (c.hexAt (i.coordX - 1, i.coordY + 1));
					} else {
						newList.Add (c.hexAt (i.coordX - 1, i.coordY - 1));
					}
				}
				i.removeCloud();
			}

			//stuff for adding new clouds (When the cloud is still at the far right side of the screen)
			if (updates <= length) {
				if (updates > 1 && updates < length) {
					newList.Add (c.hexAt (Controller.WORLD_WIDTH/2 - 1, coordY + 1));
				}
				newList.Add (c.hexAt (Controller.WORLD_WIDTH/2 -1, coordY));
			}
	
			foreach (Hex i in newList) {
				i.addCloud ();
			}

			cloudParts = newList;
			timeSinceLastMoved = 0f;
			if (cloudParts.Count == 0) {
				Destroy (this.gameObject);
			}
		}
	}
}
