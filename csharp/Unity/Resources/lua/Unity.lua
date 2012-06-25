-- Unity.lua
-- basic Lua support for Unity Lua scripts
require 'CLRPackage'
luanet.load_assembly 'UnityEngine'
return luanet.namespace 'UnityEngine'

