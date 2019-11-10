--[[
TheNexusAvenger

Class representing a window for pulling files from remote repositories.
--]]

local NexusGit = require(script.Parent.Parent.Parent.Parent):GetContext(script)
local ListRemoteBranchesRequest = NexusGit:GetResource("NexusGitRequest.GetRequest.ListRemoteBranchesRequest")
local ListRemotesRequest = NexusGit:GetResource("NexusGitRequest.GetRequest.ListRemotesRequest")
local LocalPullRequest = NexusGit:GetResource("NexusGitRequest.GetRequest.LocalPullRequest")
local GitPullView = NexusGit:GetResource("UI.View.Frame.GitPullView")
local ConfirmationWindow = NexusGit:GetResource("UI.View.Window.ConfirmationWindow")
local WindowCreator = NexusGit:GetResource("UI.View.Window.WindowCreator")
local MessageWindow = NexusGit:GetResource("UI.View.Window.MessageWindow")
local ProgressBarWindow = NexusGit:GetResource("UI.View.Window.ProgressBarWindow")
local NexusEnums = NexusGit:GetResource("NexusPluginFramework.Data.Enum.NexusEnumCollection").GetBuiltInEnums()

local ChangeHistoryService = game:GetService("ChangeHistoryService")

local GitPullWindow = GitPullView:Extend()
GitPullWindow:SetClassName("GitPullWindow")



--[[
Opens a new window.
--]]
function GitPullWindow.OpenWindow(Host)
	--Create a progress bar.
	local ProgressBarWindow = ProgressBarWindow.new("Pulling Commits")
	ProgressBarWindow:SetText("Getting remotes...")
	
	--Get the remote branches.
	local RemotesToBranches = {}
	local DefaultRemote,DefaultBranch
	local Worked,Return = pcall(function()
		local RemoteBranches = ListRemoteBranchesRequest.new(Host):GetRemotes()
		for _,Branch in pairs(RemoteBranches) do
			local SplitBranch = string.split(Branch,"/")
			local Remote,Branch = SplitBranch[1],SplitBranch[2]
			
			--Set the default remote and branch if needed.
			if string.sub(Remote,1,2) == "* " then
				Remote = string.sub(Remote,3)
				DefaultRemote,DefaultBranch = Remote,Branch
			elseif not DefaultBranch and not DefaultBranch then
				DefaultRemote,DefaultBranch = Remote,Branch
			end
			
			--Add the remote.
			if not RemotesToBranches[Remote] then
				RemotesToBranches[Remote] = {}
			end
			
			--Add the branch.
			table.insert(RemotesToBranches[Remote],Branch)
		end
	end)
	ProgressBarWindow:Close()
	
	--Dispaly an error message if the request failed.
	if not Worked then
		warn("Get remote branches failed because \""..tostring(Return).."\"")
		return MessageWindow.new("Unable to get the remote branches.\nIs Nexus Git running in the file system?","Error",56)
	end
	
	--Create the window.
	return GitPullWindow.new(Host,RemotesToBranches,DefaultRemote,DefaultBranch)
end

--[[
Creates an Git pull window object.
--]]
function GitPullWindow:__new(Host,RemotesToBranches,DefaultRemote,DefaultBranch)
	self:InitializeSuper(Host,RemotesToBranches,DefaultRemote,DefaultBranch)
	
	--Create the window.
	self:__SetChangedOverride("Window",function() end)
	self.Window = WindowCreator.CreateWindow(self,"Pull Commits",400,300,200,100)
	self.Window.Enabled = true
end

--[[
Prompts to pull the files.
--]]
function GitPullWindow:PromptPull(StatusText)
	--Create a confirmation window.
	local ConfirmWindow = ConfirmationWindow.new(StatusText.." Pull from the file system?","Pull Completed")
	local PerformPull = ConfirmWindow:YieldForClose()
	
	--Open the pull window if confirmed.
	if PerformPull then
		--Create a progress bar.
		local ProgressBar = ProgressBarWindow.new("Performing Local Pull")
		ProgressBar:SetText("Creating waypoint...")
		
		--Create a waypoint.
		ChangeHistoryService:SetWaypoint("Nexus Git Local Pull")
		ProgressBar:SetProgressBarFill(0.5)
		ProgressBar:SetText("Performing local pull...")
		
		--Perform the local pull.
		local Worked,Return = pcall(function()
			local Request = LocalPullRequest.new(self.Host)
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

--[[
Sends pull requests to the server.
--]]
function GitPullWindow:PullCommits(RemoteRepo,RemoteBranch)
	--Close the current window and create a progress bar window.
	self:Close()
	self:__SetChangedOverride("ProgressBarWindow",function() end)
	self.ProgressBarWindow = ProgressBarWindow.new("Pulling Commits")
	self.ProgressBarWindow:SetText("Pulling commits...")
	
	--Start adding the files.
	self.super:PullCommits(RemoteRepo,RemoteBranch)
end

--[[
Updates the status of the pull.
--]]
function GitPullWindow:UpdatePullStatus(Percent,StatusText)
	if Percent == true then
		--Display a success message.
		self.ProgressBarWindow:Close()
		self:PromptPull(StatusText)
	elseif Percent == false then
		--Display a failure message.
		self.ProgressBarWindow:Close()
		MessageWindow.new(StatusText,"Pull Failed")
	else
		--Update the progress bar.
		self.ProgressBarWindow:SetProgressBarFill(Percent)
		self.ProgressBarWindow:SetText(StatusText)
	end
end

--[[
Closes the view.
--]]
function GitPullWindow:Close()
	self.super:Close()
	self.Window:Destroy()
end



return GitPullWindow