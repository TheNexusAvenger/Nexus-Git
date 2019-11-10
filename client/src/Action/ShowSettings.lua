--[[
TheNexusAvenger

Action representing displaying the settings.
--]]

local NexusGit = require(script.Parent.Parent):GetContext(script)
local NexusInstance = NexusGit:GetResource("NexusInstance.NexusInstance")
local Settings = NexusGit:GetResource("Persistence.Settings")
local SettingsWindow = NexusGit:GetResource("UI.View.Window.SettingsWindow")
local NexusEnums = NexusGit:GetResource("NexusPluginFramework.Data.Enum.NexusEnumCollection").GetBuiltInEnums()

local ShowSettings = NexusInstance:Extend()
ShowSettings:SetClassName("ShowSettings")
ShowSettings:Implements(NexusGit:GetResource("Action.IAction"))



--[[
Returns the enum type of the action.
--]]
function ShowSettings:GetActionEnum()
	return NexusEnums.NexusGitAction.ShowSettings
end

--[[
Returns the display name of the action.
--]]
function ShowSettings:GetDisplayName()
	return "Open Settings"
end

--[[
Performs an action.
--]]
function ShowSettings:PerformAction()
	SettingsWindow.OpenWindow()
end



return ShowSettings