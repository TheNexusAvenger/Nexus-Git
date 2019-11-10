--[[
TheNexusAvenger

Class representing a window for adding files for Git.
--]]

local NexusGit = require(script.Parent.Parent.Parent.Parent):GetContext(script)
local GitAddRequest = NexusGit:GetResource("NexusGitRequest.PostRequest.GitAddRequest")
local GetGitStatusRequest = NexusGit:GetResource("NexusGitRequest.GetRequest.GetGitStatusRequest")
local Directory = NexusGit:GetResource("NexusGitRequest.File.Directory")
local GitCommitView = NexusGit:GetResource("UI.View.Frame.GitCommitView")
local WindowCreator = NexusGit:GetResource("UI.View.Window.WindowCreator")
local ConfirmationWindow = NexusGit:GetResource("UI.View.Window.ConfirmationWindow")
local GitPushWindow = NexusGit:GetResource("UI.View.Window.GitPushWindow")
local MessageWindow = NexusGit:GetResource("UI.View.Window.MessageWindow")
local ProgressBarWindow = NexusGit:GetResource("UI.View.Window.ProgressBarWindow")
local NexusEnums = NexusGit:GetResource("NexusPluginFramework.Data.Enum.NexusEnumCollection").GetBuiltInEnums()

local GitCommitWindow = GitCommitView:Extend()
GitCommitWindow:SetClassName("GitCommitWindow")



--[[
Opens a new window.
--]]
--
function GitCommitWindow.OpenWindow(Host)
	--Create a progress bar.
	local ProgressBarWindow = ProgressBarWindow.new("Commiting Files")
	ProgressBarWindow:SetText("Getting current status...")
	
	--Get the status.
	local StatusRequest = GetGitStatusRequest.new(Host)
	local Worked,Status = pcall(function()
		return StatusRequest:GetStatus()
	end)
	ProgressBarWindow:Close()
	
	--Display an error message if the request failed.
	if not Worked then
		warn("Get status failed because \""..tostring(Status).."\"")
		return MessageWindow.new("Unable to get the files to ocommit (is Nexus Git set up?)","Error",300)
	end
	
	--Display a message if there are no files to commit.
	local Files = Status.Files
	local FilterdFiles = Directory.FilterFiles(Files,{NexusEnums.FileStatus.Untracked,NexusEnums.FileStatus.Created,NexusEnums.FileStatus.Deleted,NexusEnums.FileStatus.Modified,NexusEnums.FileStatus.Renamed})
	if #FilterdFiles == 0 then
		return MessageWindow.new("There are no uncommitted files.","No Uncommitted Files",250)
	end
	
	--Create the window.
	return GitCommitWindow.new(FilterdFiles,Host)
end

--[[
Creates an Git add window object.
--]]
function GitCommitWindow:__new(Files,Host)
	self:InitializeSuper(Files,Host)
	
	--Create the window.
	self:__SetChangedOverride("Window",function() end)
	self.Window = WindowCreator.CreateWindow(self,"Commit Files",400,400,200,300)
	self.Window.Enabled = true
end

--[[
Sends commit requests to the server.
--]]
function GitCommitWindow:CommitFiles(SelectedFiles,CommitMessage)
	--Display a message if no files.
	if #SelectedFiles == 0 then
		MessageWindow.new("No files were selected. Nothing was commited.","No Selected Files")
		return
	end
	
	--Close the current window and create a progress bar window.
	self:Close()
	self:__SetChangedOverride("ProgressBarWindow",function() end)
	self.ProgressBarWindow = ProgressBarWindow.new("Commiting Files")
	self.ProgressBarWindow:SetText("Commiting files...")
	
	--Start commiting the files.
	self.super:CommitFiles(SelectedFiles,CommitMessage)
end

--[[
Prompts to push the files.
--]]
function GitCommitWindow:PromptPush(StatusText)
	--Create a confirmation window.
	local ConfirmWindow = ConfirmationWindow.new(StatusText.." Push to remote repository?","Commit Completed")
	local PerformPush = ConfirmWindow:YieldForClose()
	
	--Open the push window if confirmed.
	if PerformPush then
		GitPushWindow.OpenWindow(self.Host)
	end
end

--[[
Updates the status of the commit.
--]]
function GitCommitWindow:UpdateCommitStatus(Percent,StatusText)
	if Percent == true then
		--Display a success message.
		self.ProgressBarWindow:Close()
		self:PromptPush(StatusText)
	elseif Percent == false then
		--Display a failure message.
		self.ProgressBarWindow:Close()
		MessageWindow.new(StatusText,"Commit Failed")
	else
		--Update the progress bar.
		self.ProgressBarWindow:SetProgressBarFill(Percent)
		self.ProgressBarWindow:SetText(StatusText)
	end
end

--[[
Closes the view.
--]]
function GitCommitWindow:Close()
	self.super:Close()
	self.Window:Destroy()
end



return GitCommitWindow