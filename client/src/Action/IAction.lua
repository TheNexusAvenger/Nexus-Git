--[[
TheNexusAvenger

Interface representing an action.
--]]

local NexusGit = require(script.Parent.Parent):GetContext(script)
local NexusInterface = NexusGit:GetResource("NexusPluginFramework.NexusInstance.NexusInterface")

local IAction = NexusInterface:Extend()
IAction:SetClassName("IAction")



--[[
Returns the enum type of the action.
--]]
IAction:MustImplement("GetActionEnum")

--[[
Returns the display name of the action.
--]]
IAction:MustImplement("GetDisplayName")

--[[
Performs an action.
--]]
IAction:MustImplement("PerformAction")



return IAction