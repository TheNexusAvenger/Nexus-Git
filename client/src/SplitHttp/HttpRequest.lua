--[[
TheNexusAvenger

Interface for an HTTP request.
--]]

local NexusGit = require(script.Parent.Parent):GetContext(script)
local NexusInterface = NexusGit:GetResource("NexusInstance.NexusInterface")

local HttpRequest = NexusInterface:Extend()
HttpRequest:SetClassName("HttpRequest")



--[[
Sends the request and returns a response.
--]]
HttpRequest:MustImplement("SendRequest")



return HttpRequest