--[[
TheNexusAvenger

Action representing a git add.
--]]

local NexusGit = require(script.Parent.Parent):GetContext(script)
local NexusInstance = NexusGit:GetResource("NexusPluginFramework.NexusInstance.NexusInstance")
local LocalPush = NexusGit:GetResource("Action.LocalPush")
local Settings = NexusGit:GetResource("Persistence.Settings")
local GitAddWindow = NexusGit:GetResource("UI.View.Window.GitAddWindow")
local NexusEnums = NexusGit:GetResource("NexusPluginFramework.Data.Enum.NexusEnumCollection").GetBuiltInEnums()

local GitAdd = NexusInstance:Extend()
GitAdd:SetClassName("GitAdd")
GitAdd:Implements(NexusGit:GetResource("Action.IAction"))



--[[
Returns the enum type of the action.
--]]
function GitAdd:GetActionEnum()
	return NexusEnums.NexusGitAction.GitAdd
end

--[[
Returns the display name of the action.
--]]
function GitAdd:GetDisplayName()
	return "Add Files"
end

--[[
Performs an action.
--]]
function GitAdd:PerformAction(BaseURL)
	--Get the host if not defined.
	if not BaseURL then
		BaseURL = Settings.GetURL()
	end
	
	--Perform a local push and return if it failed.
	local LocalPushWorked = LocalPush.PerformAction(BaseURL)
	if LocalPushWorked == false then
		return
	end
	
	--Open the add window.
	GitAddWindow.OpenWindow(BaseURL)
end



return GitAdd