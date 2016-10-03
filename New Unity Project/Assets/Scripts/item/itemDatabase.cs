using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class itemDatabase : MonoBehaviour {

	private List<itemSingle> dataBase = new List <itemSingle>();
	private JsonData data;

	// Use this for initialization
	void Start () {
		data = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Items.json"));
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
public class itemSingle {

}
