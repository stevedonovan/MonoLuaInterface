local unity = require 'Unity'
local log = unity.Debug.Log

log('Application data '..unity.Application.dataPath)

local delta = unity.Time.deltaTime
local transform

-- constructor for objects of this kind
local Init = function(this)
    transform = this.transform
    return { this = this }
end

local Start = function(self)
    self.this.transform:Rotate(0,20,0)
end

local Update = function (self)
    transform:Translate(0,0,0.5*delta)
end

return {Init = Init, Start = Start, Update = Update}


