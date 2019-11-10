--[[
TheNexusAvenger

Class representing a window for pushing files to remote repositories.
--]]

local NexusGit = require(script.Parent.Parent.Parent.Parent):GetContext(script)
local ListRemoteBranchesRequest = NexusGit:GetResource("NexusGitRequest.GetRequest.ListRemoteBranchesRequest")
local ListRemotesRequest = NexusGit:GetResource("NexusGitRequest.GetRequest.ListRemotesRequest")
local GitPushView = NexusGit:GetResource("UI.View.Frame.GitPushView")
local WindowCreator = NexusGit:GetResource("UI.View.Window.WindowCreator")
local MessageWindow = NexusGit:GetResource("UI.View.Window.MessageWindow")
local ProgressBarWindow = NexusGit:GetResource("UI.View.Window.ProgressBarWindow")
local NexusEnums = NexusGit:GetResource("NexusPluginFramework.Data.Enum.NexusEnumCollection").GetBuiltInEnums()

local GitPushWindow = GitPushView:Extend()
GitPushWindow:SetClassName("GitPushWindow")



--[[
Opens a new window.
--]]
function GitPushWindow.OpenWindow(Host)
	--Create a progress bar.
	local ProgressBarWindow = ProgressBarWindow.new("Pushing Commits")
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
	return GitPushWindow.new(Host,RemotesToBranches,DefaultRemote,DefaultBranch)
end

--[[
Creates an Git add window object.
--]]
function GitPushWindow:__new(Host,RemotesToBranches,DefaultRemote,DefaultBranch)
	self:InitializeSuper(Host,RemotesToBranches,DefaultRemote,DefaultBranch)
	
	--Create the window.
	self:__SetChangedOverride("Window",function() end)
	self.Window = WindowCreator.CreateWindow(self,"Push Commits",400,300,200,100)
	self.Window.Enabled = true
end

--[[
Sends add requests to the server.
--]]
function GitPushWindow:PushCommits(RemoteRepo,RemoteBranch)
	--Close the current window and create a progress bar window.
	self:Close()
	self:__SetChangedOverride("ProgressBarWindow",function() end)
	self.ProgressBarWindow = ProgressBarWindow.new("Pushing Commits")
	self.ProgressBarWindow:SetText("Pushing commits...")
	
	--Start adding the files.
	self.super:PushCommits(RemoteRepo,RemoteBranch)
end

--[[
Updates the status of the add.
--]]
function GitPushWindow:UpdatePushStatus(Percent,StatusText)
	if Percent == true then
		--Display a success message.
		self.ProgressBarWindow:Close()
		MessageWindow.new(StatusText,"Push Completed")
	elseif Percent == false then
		--Display a failure message.
		self.ProgressBarWindow:Close()
		MessageWindow.new(StatusText,"Push Failed")
	else
		--Update the progress bar.
		self.ProgressBarWindow:SetProgressBarFill(Percent)
		self.ProgressBarWindow:SetText(StatusText)
	end
end

--[[
Closes the view.
--]]
function GitPushWindow:Close()
	self.super:Close()
	self.Window:Destroy()
end



return GitPushWindow