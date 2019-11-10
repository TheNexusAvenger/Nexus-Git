--[[
TheNexusAvenger

Class representing a window for adding files for Git.
--]]

local NexusGit = require(script.Parent.Parent.Parent.Parent):GetContext(script)
local GitAddRequest = NexusGit:GetResource("NexusGitRequest.PostRequest.GitAddRequest")
local GetGitStatusRequest = NexusGit:GetResource("NexusGitRequest.GetRequest.GetGitStatusRequest")
local Directory = NexusGit:GetResource("NexusGitRequest.File.Directory")
local GitAddView = NexusGit:GetResource("UI.View.Frame.GitAddView")
local WindowCreator = NexusGit:GetResource("UI.View.Window.WindowCreator")
local MessageWindow = NexusGit:GetResource("UI.View.Window.MessageWindow")
local ProgressBarWindow = NexusGit:GetResource("UI.View.Window.ProgressBarWindow")
local NexusEnums = NexusGit:GetResource("NexusPluginFramework.Data.Enum.NexusEnumCollection").GetBuiltInEnums()

local GitAddWindow = GitAddView:Extend()
GitAddWindow:SetClassName("GitAddWindow")



--[[
Opens a new window.
--]]
function GitAddWindow.OpenWindow(Host)
	--Create a progress bar.
	local ProgressBarWindow = ProgressBarWindow.new("Adding Files")
	ProgressBarWindow:SetText("Getting current status...")
	
	--Get the status.
	local StatusRequest = GetGitStatusRequest.new(Host)
	local Worked,Status = pcall(function()
		return StatusRequest:GetStatus()
	end)
	ProgressBarWindow:Close()
	
	--Dispaly an error message if the request failed.
	if not Worked then
		warn("Get status failed because \""..tostring(Status).."\"")
		return MessageWindow.new("Unable to get the files to add.\nIs Nexus Git running in the file system?","Error",250,36)
	end
	
	--Display a message if there are no files to add.
	local Files = Status.Files
	local FilterdFiles = Directory.FilterFiles(Files,{NexusEnums.FileStatus.Untracked})
	if #FilterdFiles == 0 then
		return MessageWindow.new("There are no untracked files.","No Untracked Files")
	end
	
	--Create the window.
	return GitAddWindow.new(FilterdFiles,Host)
end

--[[
Creates an Git add window object.
--]]
function GitAddWindow:__new(Files,Host)
	self:InitializeSuper(Files,Host)
	
	--Create the window.
	self:__SetChangedOverride("Window",function() end)
	self.Window = WindowCreator.CreateWindow(self,"Add Files",400,300,200,100)
	self.Window.Enabled = true
end

--[[
Sends add requests to the server.
--]]
function GitAddWindow:AddFiles(SelectedFiles)
	--Display a message if no files.
	if #SelectedFiles == 0 then
		MessageWindow.new("No files were selected. Nothing was added.","No Selected Files")
		return
	end
	
	--Close the current window and create a progress bar window.
	self:Close()
	self:__SetChangedOverride("ProgressBarWindow",function() end)
	self.ProgressBarWindow = ProgressBarWindow.new("Adding Files")
	self.ProgressBarWindow:SetText("Adding files...")
	
	--Start adding the files.
	self.super:AddFiles(SelectedFiles)
end

--[[
Updates the status of the add.
--]]
function GitAddWindow:UpdateAddStatus(Percent,StatusText)
	if Percent == true then
		--Display a success message.
		self.ProgressBarWindow:Close()
		MessageWindow.new(StatusText,"Add Completed")
	elseif Percent == false then
		--Display a failure message.
		self.ProgressBarWindow:Close()
		MessageWindow.new(StatusText,"Add Failed")
	else
		--Update the progress bar.
		self.ProgressBarWindow:SetProgressBarFill(Percent)
		self.ProgressBarWindow:SetText(StatusText)
	end
end

--[[
Closes the view.
--]]
function GitAddWindow:Close()
	self.super:Close()
	self.Window:Destroy()
end



return GitAddWindow