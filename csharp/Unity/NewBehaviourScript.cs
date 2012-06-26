using UnityEngine;
using System.Collections;
using LuaInterface;
using LuaUnity;

public class NewBehaviourScript : LuaMonoBehaviour {

	public string lua = "";

	// Use this for initialization
	void Awake () {
		if (lua != "")
			Init(lua);
	}

}
