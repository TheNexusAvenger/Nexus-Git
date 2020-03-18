--[[
TheNexusAvenger

Action representing a local push.
--]]

local NexusGit = require(script.Parent.Parent):GetContext(script)
local NexusInstance = NexusGit:GetResource("NexusPluginFramework.NexusInstance.NexusInstance")
local Settings = NexusGit:GetResource("Persistence.Settings")
local ProgressBarWindow = NexusGit:GetResource("UI.View.Window.ProgressBarWindow")
local ConfirmationWindow = NexusGit:GetResource("UI.View.Window.ConfirmationWindow")
local MessageWindow = NexusGit:GetResource("UI.View.Window.MessageWindow")
local LocalPushRequest = NexusGit:GetResource("NexusGitRequest.PostRequest.LocalPushRequest")
local NexusEnums = NexusGit:GetResource("NexusPluginFramework.Data.Enum.NexusEnumCollection").GetBuiltInEnums()

local LocalPush = NexusInstance:Extend()
LocalPush:SetClassName("LocalPush")
LocalPush:Implements(NexusGit:GetResource("Action.IAction"))



--[[
Returns the enum type of the action.
--]]
function LocalPush:GetActionEnum()
	return NexusEnums.NexusGitAction.LocalPush
end

--[[
Returns the display name of the action.
--]]
function LocalPush:GetDisplayName()
	return "Push to File System"
end

--[[
Performs an action.
Returns:
nil - Didn't run
true - Completed successfully
false - Failed to complete
--]]
function LocalPush:PerformAction(BaseURL)
	--Get the host if not defined.
	if not BaseURL then
		BaseURL = Settings.GetURL()
	end
	
	--Show a confirmation window.
	local Window = ConfirmationWindow.new("Push Roblox Instances to the file system?","Local Push","Yes","No")
	local Confirmed = Window:YieldForClose()
	 
	--Perform the request.
	if Confirmed then
		--Create a progress bar.
		local ProgressBar = ProgressBarWindow.new("Performing Local Push")
		ProgressBar:SetText("Performing local push...")
		
		--Perform the local push.
		local Worked,Return = pcall(function()
			local Request = LocalPushRequest.new(BaseURL)
			Request:PerformLocalPush()
		end)
		
		--Close the progress bar.
		ProgressBar:Close()
		
		--Create a message window.
		if Worked then
			local FinalMessage = MessageWindow.new("Roblox Instances successfully pushed to the file system.","Local Push Completed")
			FinalMessage:YieldForClose()
			return true
		else
			warn("Local push failed because \""..tostring(Return).."\"")
			local FinalMessage = MessageWindow.new("Failed to push Roblox Instances to the file system.\nIs Nexus Git running in the file system?","Local Push Failed",nil,36)
			FinalMessage:YieldForClose()
			return false
		end
	end
end



return LocalPush