--[[
TheNexusAvenger

Action representing a git commit.
--]]

local NexusGit = require(script.Parent.Parent):GetContext(script)
local NexusInstance = NexusGit:GetResource("NexusPluginFramework.NexusInstance.NexusInstance")
local LocalPush = NexusGit:GetResource("Action.LocalPush")
local Settings = NexusGit:GetResource("Persistence.Settings")
local GitCommitWindow = NexusGit:GetResource("UI.View.Window.GitCommitWindow")
local NexusEnums = NexusGit:GetResource("NexusPluginFramework.Data.Enum.NexusEnumCollection").GetBuiltInEnums()

local GitCommit = NexusInstance:Extend()
GitCommit:SetClassName("GitCommit")
GitCommit:Implements(NexusGit:GetResource("Action.IAction"))



--[[
Returns the enum type of the action.
--]]
function GitCommit:GetActionEnum()
	return NexusEnums.NexusGitAction.GitCommit
end

--[[
Returns the display name of the action.
--]]
function GitCommit:GetDisplayName()
	return "Commit Files"
end

--[[
Performs an action.
--]]
function GitCommit:PerformAction(BaseURL)
	--Get the host if not defined.
	if not BaseURL then
		BaseURL = Settings.GetURL()
	end
	
	--Perform a local push and return if it failed.
	local LocalPushWorked = LocalPush.PerformAction(BaseURL)
	if LocalPushWorked == false then
		return
	end
	
	--Open the commit window.
	GitCommitWindow.OpenWindow(BaseURL)
end



return GitCommit