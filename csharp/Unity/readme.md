## Using LuaInterface in Unity3D

(This is based on Windows 7 64-bit, but OS X should not be very different.)

1. Build appropriate lua51.dll or liblua5.1.so. This required a 32-bit build, since
Unity is a 32-bit application. The two shared native libraries (lua51 and luanet)
need to be dynamically linkable; on Windows I put them next to the Unity executable.

2. Build LuaInterface using the version of Mono that ships with Unity, and drop
LuaInterface.dll into a Unity Project's Assets folder. (The LuaInterface folder of
the project contains a MonoDevelop project for LuaInterface.)

3. LuaUnity.cs needs to be added to a subproject in MonoDevelop, with references to
UnityEngine and LuaInterface, and its output directory pointing to the Asset folder.

4. Can now create a little stub C# class NewBehaviourScript.cs, which has a
public variable `lua`, which is set to the name without extension of the Lua
script - so setting `lua` to 'NewBehaviourScript' will cause
Resources/NewBehaviourScript.lua to be loaded.

The NewBehaviourScript.cs class can be repeatedly used, with diferent values of `lua`.

5. The Lua module path is modified dynamically so that modules are looked up in
Resources/lua - in particular Unity.lua

