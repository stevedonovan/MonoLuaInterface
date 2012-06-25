using UnityEngine;
using System.Collections;
using LuaInterface;
using LuaUnity;

public class NewBehaviourScript : LuaMonoBehaviour {
	
	public string lua = "";
	
	// Use this for initialization
	void Start () {
		if (lua != "") 
			Init(lua);
	}
	
}
