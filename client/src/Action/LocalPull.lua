--[[
TheNexusAvenger

Action representing a local pull.
--]]

local NexusGit = require(script.Parent.Parent):GetContext(script)
local NexusInstance = NexusGit:GetResource("NexusPluginFramework.NexusInstance.NexusInstance")
local Settings = NexusGit:GetResource("Persistence.Settings")
local ProgressBarWindow = NexusGit:GetResource("UI.View.Window.ProgressBarWindow")
local ConfirmationWindow = NexusGit:GetResource("UI.View.Window.ConfirmationWindow")
local MessageWindow = NexusGit:GetResource("UI.View.Window.MessageWindow")
local LocalPullRequest = NexusGit:GetResource("NexusGitRequest.GetRequest.LocalPullRequest")
local NexusEnums = NexusGit:GetResource("NexusPluginFramework.Data.Enum.NexusEnumCollection").GetBuiltInEnums()

local ChangeHistoryService = game:GetService("ChangeHistoryService")

local LocalPull = NexusInstance:Extend()
LocalPull:SetClassName("LocalPull")
LocalPull:Implements(NexusGit:GetResource("Action.IAction"))



--[[
Returns the enum type of the action.
--]]
function LocalPull:GetActionEnum()
	return NexusEnums.NexusGitAction.LocalPull
end

--[[
Returns the display name of the action.
--]]
function LocalPull:GetDisplayName()
	return "Pull from File System"
end

--[[
Performs an action.
Returns:
nil - Didn't run
true - Completed successfully
false - Failed to complete
--]]
function LocalPull:PerformAction(BaseURL)
	--Get the host if not defined.
	if not BaseURL then
		BaseURL = Settings.GetURL()
	end
	
	--Show a confirmation window.
	local Window = ConfirmationWindow.new("Pull Roblox Instances from the file system?","Local Pull","Yes","No")
	local Confirmed = Window:YieldForClose()
	
	--Perform the request.
	if Confirmed then
		--Create a progress bar.
		local ProgressBar = ProgressBarWindow.new("Performing Local Pull")
		ProgressBar:SetText("Creating waypoint...")
		
		--Create a waypoint.
		ChangeHistoryService:SetWaypoint("Nexus Git Local Pull")
		ProgressBar:SetProgressBarFill(0.5)
		ProgressBar:SetText("Performing local pull...")
		
		--Perform the local pull.
		local Worked,Return = pcall(function()
			local Request = LocalPullRequest.new(BaseURL)
			Request:PerformLocalPull()
		end)
		
		--Close the progress bar.
		ProgressBar:Close()
		
		--Create a message window.
		if Worked then
			local FinalMessage = MessageWindow.new("Roblox Instances successfully pulled from the file system.","Local Pull Completed")
			FinalMessage:YieldForClose()
		else
			warn("Local pull failed because \""..tostring(Return).."\"")
			local FinalMessage = MessageWindow.new("Failed to pull Roblox Instances from the file system.\nIs Nexus Git running in the file system?","Local Pull Failed",nil,36)
			FinalMessage:YieldForClose()
		end
	end
end



return LocalPull