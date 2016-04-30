using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Cloud : MonoBehaviour {

	public Controller c;
	public EnvironmentManager em;
	public List<Hex> cloudParts;

	public float timeSinceLastMoved;
	public const float STEP_INTERVAL = 2f;

	public int length;
	public int updates;
	public int coordY;

	public bool isRain;
	public float timeSinceLastRained;
	public const float RAIN_INTERVAL = 0.25f;

	public void init (EnvironmentManager em, int coordY, int length, bool isRain){
		this.em = em;
		c = em.c;
		this.coordY = coordY;
		this.length = length;
		this.isRain = isRain;
	}

	void Start() {
		timeSinceLastMoved = 0f;
		timeSinceLastRained = 0f;
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
		timeSinceLastMoved  += Time.deltaTime;
		timeSinceLastRained += Time.deltaTime;

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
				i.addCloud (isRain);
			}

			cloudParts = newList;
			timeSinceLastMoved = 0f;
			if (cloudParts.Count == 0) {
				if (isRain) {
					em.rainclouds--;
					if (em.rainclouds <= 0) {
						em.changeWeather (EnvironmentManager.NORMAL_WEATHER);
					}
				}
				Destroy (this.gameObject);
			}
		}

		if (timeSinceLastRained > RAIN_INTERVAL && isRain) {
			foreach (Hex i in cloudParts) {
				if (UnityEngine.Random.Range (0, 10) == 0) {
					em.createSun (i.realX, i.realY, EnvironmentManager.RAINY_WEATHER);
				}
			}
			timeSinceLastRained = 0f;
		}
	}
}
