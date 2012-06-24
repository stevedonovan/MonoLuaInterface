## Using LuaInterface in Unity3D

(This is based on Windows 7 64-bit, but OS X should not be very different.)

1. Build appropriate lua51.dll or liblua5.1.so. This required a 32-bit build, since Unity is a 32-bit application. The two shared native libraries (lua51 and luanet) need to be dynamically linkable; on Windows I put them next to the Unity executable.

2. Build LuaInterface using the version of Mono that ships with Unity, and drop LuaInterface.dll into a Unity Project's Assets folder.

3. LuaUnity.cs needs to be added to a subproject in MonoDevelop, with references to UnityEngine and LuaInterface, and its output directory pointing to the Asset folder.

4. Can now create little stub C# files (see NewBehaviourScript.cs) which refers to the actual script (see Resources/NewBehaviourScript.lua).

5. The Lua module path is modified dynamically so that modules are looked up in Resources/lua - in particular Unity.lua

