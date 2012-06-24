-- Unity.lua
require 'CLRPackage'
luanet.load_assembly 'UnityEngine'
local unity = luanet.namespace 'UnityEngine'

function luanet.module()
    local env = {
        this = __THIS__,
        _G = _G
    }
    package.loaded[__SCRIPT__] = env
    setfenv(2,env)
end

return unity

