using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class playerInventory : MonoBehaviour {

	int[,] inventoryList = new int[16, 4];							//2D arrays! First dimension holds items, second dimension holds their properties
	int coin;														//holds money amount
	int i, j, k;													//for loops!
	bool inventoryShow = false;										//if your inventory is activated (visible on GUI)

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (inventoryShow == true) {

		}
	}

	void pickUp (int drop) {
		for (i = 0; i < inventoryList.GetLength (0); i++) {
			if (inventoryList [i, 0] == 0) {
				inventoryList [i, 0] = drop;
				Debug.Log ("Picked up item " + drop + ", at space " + i);
				return;
			}
		}

		Debug.Log ("Inventory is full");
	}
}
