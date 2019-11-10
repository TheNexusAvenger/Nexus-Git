--[[
TheNexusAvenger

Action representing performing a git pull.
--]]

local NexusGit = require(script.Parent.Parent):GetContext(script)
local NexusInstance = NexusGit:GetResource("NexusInstance.NexusInstance")
local Settings = NexusGit:GetResource("Persistence.Settings")
local GitPullWindow = NexusGit:GetResource("UI.View.Window.GitPullWindow")
local NexusEnums = NexusGit:GetResource("NexusPluginFramework.Data.Enum.NexusEnumCollection").GetBuiltInEnums()

local GitPull = NexusInstance:Extend()
GitPull:SetClassName("GitPull")
GitPull:Implements(NexusGit:GetResource("Action.IAction"))



--[[
Returns the enum type of the action.
--]]
function GitPull:GetActionEnum()
	return NexusEnums.NexusGitAction.GitPull
end

--[[
Returns the display name of the action.
--]]
function GitPull:GetDisplayName()
	return "Pull Commits"
end

--[[
Performs an action.
--]]
function GitPull:PerformAction(BaseURL)
	--Get the host if not defined.
	if not BaseURL then
		BaseURL = Settings.GetURL()
	end
	
	--Open the window.
	GitPullWindow.OpenWindow(BaseURL)
end



return GitPull