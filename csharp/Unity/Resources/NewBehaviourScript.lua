local unity = require 'Unity'
local log = unity.Debug.Log

log('Application data '..unity.Application.dataPath)

local delta = unity.Time.deltaTime

local Start = function(self)
    self.transform:Rotate(0,20,0)
end

local Update = function (self)
    self.transform:Translate(0,0,0.5*delta)
end

return {Start = Start, Update = Update}


