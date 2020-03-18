--[[
TheNexusAvenger

Interface representing a view frame.
--]]

local NexusGit = require(script.Parent.Parent.Parent.Parent):GetContext(script)
local NexusInterface = NexusGit:GetResource("NexusPluginFramework.NexusInstance.NexusInterface")

local IViewFrame = NexusInterface:Extend()
IViewFrame:SetClassName("IViewFrame")



--[[
Closes the view.
--]]
IViewFrame:MustImplement("Close")



return IViewFrame