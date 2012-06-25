/*
  This class is intended to implement MonoBehaviour using a Lua script.
  Init() is called from the stub (see NewBehaviourScript.cs) in the Start()
  method. It finds the Lua script in the resources folder, and loads it rather
  like require() does.

  It should be fairly straightforward to implement other needed methods here.
*/

using System;
using UnityEngine;
using LuaInterface;

namespace LuaUnity
{
	public class LuaMonoBehaviour: MonoBehaviour {

		public static Lua L = null;
		public static string resourcePath = null;

		LuaFunction update, start;

		public void Init (string ScriptName) {
			if (L == null) {
				resourcePath = Application.dataPath+"/Resources/";
				L = new Lua();
				string packagePath = (string)L["package.path"];
				L["package.path"] = resourcePath+"lua/?.lua;"+packagePath;
			}

			string script = resourcePath+ScriptName+".lua";
			// will throw an exception on error...
			object[] res = L.DoFile (script);
			LuaTable env = (LuaTable)res[0];
			update = (LuaFunction)env["Update"];
			start = (LuaFunction)env["Start"];

			if (start != null)
				start.Call(this);
		}

		// Update is called once per frame
		void Update () {
			if (update != null)
				update.Call(this);
		}

	}


}

