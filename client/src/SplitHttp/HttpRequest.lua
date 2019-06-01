--[[
TheNexusAvenger

Interface for an HTTP request.
--]]

local Root = script.Parent.Parent
local NexusInterface = require(Root:WaitForChild("NexusInstance"):WaitForChild("NexusInterface"))

local HttpRequest = NexusInterface:Extend()
HttpRequest:SetClassName("HttpRequest")



--[[
Sends the request and returns a response.
--]]
HttpRequest:MustImplement("SendRequest")



return HttpRequest