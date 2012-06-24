local unity = require 'Unity'
local log = unity.Debug.Log

luanet.module()

log('Lua path '.._G.package.path)
log('Application data '..unity.Application.dataPath)

local delta = unity.Time.deltaTime
local transform = this.transform

function Update()
    transform:Translate(0,0,2*delta)
end

