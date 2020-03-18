--[[
TheNexusAvenger

Action representing performing a git push.
--]]

local NexusGit = require(script.Parent.Parent):GetContext(script)
local NexusInstance = NexusGit:GetResource("NexusPluginFramework.NexusInstance.NexusInstance")
local Settings = NexusGit:GetResource("Persistence.Settings")
local GitPushWindow = NexusGit:GetResource("UI.View.Window.GitPushWindow")
local NexusEnums = NexusGit:GetResource("NexusPluginFramework.Data.Enum.NexusEnumCollection").GetBuiltInEnums()

local GitPush = NexusInstance:Extend()
GitPush:SetClassName("GitPush")
GitPush:Implements(NexusGit:GetResource("Action.IAction"))



--[[
Returns the enum type of the action.
--]]
function GitPush:GetActionEnum()
	return NexusEnums.NexusGitAction.GitPush
end

--[[
Returns the display name of the action.
--]]
function GitPush:GetDisplayName()
	return "Push Commits"
end

--[[
Performs an action.
--]]
function GitPush:PerformAction(BaseURL)
	--Get the host if not defined.
	if not BaseURL then
		BaseURL = Settings.GetURL()
	end
	
	--Open the window.
	GitPushWindow.OpenWindow(BaseURL)
end



return GitPush