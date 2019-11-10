--[[
TheNexusAvenger

Class representing a view for commiting files.
--]]

local NexusGit = require(script.Parent.Parent.Parent.Parent):GetContext(script)
local GitAddRequest = NexusGit:GetResource("NexusGitRequest.PostRequest.GitAddRequest")
local GitCommitRequest = NexusGit:GetResource("NexusGitRequest.PostRequest.GitCommitRequest")
local Directory = NexusGit:GetResource("NexusGitRequest.File.Directory")
local NexusWrappedInstance = NexusGit:GetResource("NexusPluginFramework.Base.NexusWrappedInstance")
local FileSelectionFrame = NexusGit:GetResource("UI.Frame.FileSelection.FileSelectionFrame")
local NexusEnums = NexusGit:GetResource("NexusPluginFramework.Data.Enum.NexusEnumCollection").GetBuiltInEnums()

local GitCommitView = NexusWrappedInstance:Extend()
GitCommitView:SetClassName("GitCommitView")
GitCommitView:Implements(NexusGit:GetResource("UI.View.Frame.IViewFrame"))





--[[
Creates an git commit view object.
--]]
function GitCommitView:__new(Files,Host)
	self:InitializeSuper("Frame")
	
	--Store the host.
	self:__SetChangedOverride("Host",function() end)
	self.Host = Host
	
	--Create the list frame.
	local FilterdFiles = Directory.FilterFiles(Files,{NexusEnums.FileStatus.Untracked,NexusEnums.FileStatus.Created,NexusEnums.FileStatus.Deleted,NexusEnums.FileStatus.Modified,NexusEnums.FileStatus.Renamed})
	local ListFrame = FileSelectionFrame.new(FilterdFiles)
	ListFrame.BorderSizePixel = 1
	ListFrame.Size = UDim2.new(1,0,1,-120)
	ListFrame.Parent = self
	self:__SetChangedOverride("ListFrame",function() end)
	self.ListFrame = ListFrame
	
	--Create the commit message box.
	local CommitMessageBox = NexusWrappedInstance.GetInstance("TextBox")
	CommitMessageBox.Size = UDim2.new(1,-6,0,87)
	CommitMessageBox.Position = UDim2.new(0,3,1,-117)
	CommitMessageBox.TextWrapped = true
	CommitMessageBox.MultiLine = true
	CommitMessageBox.Text = ""
	CommitMessageBox.TextYAlignment = "Top"
	CommitMessageBox.PlaceholderText = "Commit message"
	CommitMessageBox.Parent = self
	self:__SetChangedOverride("CommitMessageBox",function() end)
	self.CommitMessageBox = CommitMessageBox
	
	--Create the buttons.
	local CancelButton = NexusWrappedInstance.GetInstance("TextButton")
	CancelButton.Size = UDim2.new(0,84,0,22)
	CancelButton.Position = UDim2.new(1,-88,1,-26)
	CancelButton.BackgroundColor3 = "DialogButton"
	CancelButton.TextColor3 = "DialogButtonText"
	CancelButton.Text = "Cancel"
	CancelButton.Parent = self
	self:__SetChangedOverride("CancelButton",function() end)
	self.CancelButton = CancelButton
	
	local CommitButton = NexusWrappedInstance.GetInstance("TextButton")
	CommitButton.Size = UDim2.new(0,84,0,22)
	CommitButton.Position = UDim2.new(1,-176,1,-26)
	CommitButton.BackgroundColor3 = "DialogMainButton"
	CommitButton.TextColor3 = "DialogMainButtonText"
	CommitButton.Text = "Commit"
	CommitButton.Parent = self
	self:__SetChangedOverride("CommitButton",function() end)
	self.CommitButton = CommitButton
	
	--Set up the clicked events.
	local DB = true
	CommitButton.MouseButton1Down:Connect(function()
		if DB then
			DB = false
			self.object:CommitFiles(ListFrame:GetSelectedFiles(),CommitMessageBox.Text)
		end
	end)
	
	CancelButton.MouseButton1Down:Connect(function()
		if DB then
			DB = false
			self.object:Close()
		end
	end)
end

--[[
Sends a git request to add a file.
--]]
function GitCommitView:SendAddRequest(Files)
	GitAddRequest.new(self.Host,Files):SendRequest()
end

--[[
Sends a git request to commit a file.
--]]
function GitCommitView:SendCommitRequest(Files,Message)
	GitCommitRequest.new(self.Host,Files,Message):SendRequest()
end

--[[
Sends commit requests to the server.
--]]
function GitCommitView:CommitFiles(SelectedFiles,CommitMessage)
	--Convert the files to a list.
	local FilesList = {}
	
	--[[
	Adds a file to the list.
	--]]
	local function AddFileToList(File,FileBase)
		if FileBase ~= "" then
			FileBase = FileBase.."/"
		end
		FileBase = FileBase..File:GetFileName()
		
		if File:IsA("Directory") then
			for _,SubFile in pairs(File:GetFiles()) do
				AddFileToList(SubFile,FileBase)
			end
		else
			table.insert(FilesList,FileBase)
		end
	end
	
	--Add the files.
	for _,File in pairs(SelectedFiles) do
		AddFileToList(File,"")
	end
	
	--Send the network requests to add the files.
	local Worked,Return = pcall(function()
		for i,FileString in pairs(FilesList) do
			self:UpdateCommitStatus((i - 1)/(#FilesList + 1),"Adding "..tostring(FileString).."...")
			self:SendAddRequest({FileString})
		end
	end)
	
	--Show either a failure or continue.
	if Worked then
		--Send the network request to commit the files.
		local Worked,Return = pcall(function()
			if #FilesList == 1 then
				self:UpdateCommitStatus(#FilesList/(#FilesList + 1),"Committing 1 file...")
			else
				self:UpdateCommitStatus(#FilesList/(#FilesList + 1),"Committing "..tostring(#FilesList).." files...")
			end
			self:SendCommitRequest(FilesList,CommitMessage)
		end)
		
		--Show the result.
		if Worked then
			if #FilesList == 1 then
				self:UpdateCommitStatus(true,"1 files was committed.")
			else
				self:UpdateCommitStatus(true,tostring(#FilesList).." files were committed.")
			end
		else
			self:UpdateCommitStatus(false,"Commit failed.")
			warn("Add request failed because "..tostring(Return))
		end
	else
		self:UpdateCommitStatus(false,"Add failed.")
		warn("Add request failed because "..tostring(Return))
	end
end

--[[
Updates the status of the commit.
--]]
function GitCommitView:UpdateCommitStatus(Percent,StatusText)
	if self ~= self.object then
		self.object:UpdateCommitStatus(Percent,StatusText)
	end
end

--[[
Closes the view.
--]]
function GitCommitView:Close()
	self:Destroy()
end



return GitCommitView