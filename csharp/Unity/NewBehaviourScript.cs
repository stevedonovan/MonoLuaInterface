/*
  Any Lua behaviour script is launched using this kind of stub
  (see Resources/NewBehaviourScript.lua)
*/
using UnityEngine;
using LuaUnity;

public class NewBehaviourScript : LuaMonoBehaviour {

	// Use this for initialization
	void Start () {
		Init("NewBehaviourScript");
	}

}
