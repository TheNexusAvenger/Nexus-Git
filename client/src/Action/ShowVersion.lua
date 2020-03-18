--[[
TheNexusAvenger

Action representing displaying the version.
--]]

local NexusGit = require(script.Parent.Parent):GetContext(script)
local NexusInstance = NexusGit:GetResource("NexusPluginFramework.NexusInstance.NexusInstance")
local Settings = NexusGit:GetResource("Persistence.Settings")
local VersionWindow = NexusGit:GetResource("UI.View.Window.VersionWindow")
local NexusEnums = NexusGit:GetResource("NexusPluginFramework.Data.Enum.NexusEnumCollection").GetBuiltInEnums()

local ShowVersion = NexusInstance:Extend()
ShowVersion:SetClassName("ShowVersion")
ShowVersion:Implements(NexusGit:GetResource("Action.IAction"))



--[[
Returns the enum type of the method.
--]]
function ShowVersion:GetActionEnum()
	return NexusEnums.NexusGitAction.ShowVersion
end

--[[
Returns the display name of the action.
--]]
function ShowVersion:GetDisplayName()
	return "Show Version"
end

--[[
Performs an action.
--]]
function ShowVersion:PerformAction(BaseURL)
	--Get the host if not defined.
	if not BaseURL then
		BaseURL = Settings.GetURL()
	end
	
	--Open the window.
	VersionWindow.OpenWindow(BaseURL)
end



return ShowVersion