--[[
TheNexusAvenger

Class representing a view for pushing to remote repositories.
--]]

local NexusGit = require(script.Parent.Parent.Parent.Parent):GetContext(script)
local ListCommitsRequest = NexusGit:GetResource("NexusGitRequest.GetRequest.ListCommitsRequest")
local RemotePushRequest = NexusGit:GetResource("NexusGitRequest.PostRequest.RemotePushRequest")
local Directory = NexusGit:GetResource("NexusGitRequest.File.Directory")
local NexusWrappedInstance = NexusGit:GetResource("NexusPluginFramework.Base.NexusWrappedInstance")
local NexusScrollingFrame = NexusGit:GetResource("NexusPluginFramework.UI.Scroll.NexusScrollingFrame")
local NexusCollapsableListFrame = NexusGit:GetResource("NexusPluginFramework.UI.CollapsableList.NexusCollapsableListFrame")
local NexusEnums = NexusGit:GetResource("NexusPluginFramework.Data.Enum.NexusEnumCollection").GetBuiltInEnums()

local GitPushView = NexusWrappedInstance:Extend()
GitPushView:SetClassName("GitPushView")
GitPushView:Implements(NexusGit:GetResource("UI.View.Frame.IViewFrame"))





--[[
Creates an git push view object.
--]]
function GitPushView:__new(Host,RemotesToBranches,DefaultRemote,DefaultBranch)
	self:InitializeSuper("Frame")

	--Correct the defaults.
	if not DefaultRemote then DefaultRemote = "" end
	if not DefaultBranch then DefaultBranch = "" end
	
	--Store the host.
	self:__SetChangedOverride("Host",function() end)
	self.Host = Host
	
	--Create the input buttons.
	local BranchLabel = NexusWrappedInstance.GetInstance("TextLabel")
	BranchLabel.Size = UDim2.new(0,84,0,22)
	BranchLabel.Position = UDim2.new(0,4,0,4)
	BranchLabel.Text = "Remote:"
	BranchLabel.Parent = self
	
	local RemoteTextBox = NexusWrappedInstance.GetInstance("TextBox")
	RemoteTextBox.Size = UDim2.new(0,84,0,22)
	RemoteTextBox.Position = UDim2.new(0,52,0,4)
	RemoteTextBox.TextXAlignment = "Left"
	RemoteTextBox.Text = DefaultRemote
	RemoteTextBox.Parent = self
	
	local SlashLabel = NexusWrappedInstance.GetInstance("TextLabel")
	SlashLabel.Size = UDim2.new(0,10,0,22)
	SlashLabel.Position = UDim2.new(0,138,0,4)
	SlashLabel.Text = "/"
	SlashLabel.TextXAlignment = "Center"
	SlashLabel.Parent = self
	
	local BranchTextBox = NexusWrappedInstance.GetInstance("TextBox")
	BranchTextBox.Size = UDim2.new(0,84,0,22)
	BranchTextBox.Position = UDim2.new(0,150,0,4)
	BranchTextBox.TextXAlignment = "Left"
	BranchTextBox.Text = DefaultBranch
	BranchTextBox.Parent = self
	
	local ErrorMessageLabel = NexusWrappedInstance.GetInstance("TextLabel")
	ErrorMessageLabel.Size = UDim2.new(1,-180,0,22)
	ErrorMessageLabel.Position = UDim2.new(0,4,1,-26)
	ErrorMessageLabel.Text = ""
	ErrorMessageLabel.Parent = self
	
	--Create the list frame.
	local CommitsScrollingFrame = NexusScrollingFrame.new(NexusEnums.NexusScrollTheme.Qt5)
	CommitsScrollingFrame.BorderSizePixel = 1
	CommitsScrollingFrame.Position = UDim2.new(0,0,0,31)
	CommitsScrollingFrame.Size = UDim2.new(1,0,1,-62)
	CommitsScrollingFrame.Parent = self
	
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
	
	local PushButton = NexusWrappedInstance.GetInstance("TextButton")
	PushButton.Size = UDim2.new(0,84,0,22)
	PushButton.Position = UDim2.new(1,-176,1,-26)
	PushButton.BackgroundColor3 = "DialogMainButton"
	PushButton.TextColor3 = "DialogMainButtonText"
	PushButton.Text = "Push"
	PushButton.Parent = self
	self:__SetChangedOverride("PushButton",function() end)
	self.PushButton = PushButton
	
	--[[
	Sets a single line message to display on the list.
	--]]
	local function SetListSingleLine(Text)
		--Clear the existing frame.
		CommitsScrollingFrame:ClearAllChildren()
		
		--Create the label and set the canvas size.
		local MessageLabel = NexusWrappedInstance.GetInstance("TextLabel")
		MessageLabel.Size = UDim2.new(0,10,0,20)
		MessageLabel.Position = UDim2.new(0,2,0,0)
		MessageLabel.Text = Text
		MessageLabel.TextXAlignment = "Left"
		MessageLabel.Parent = CommitsScrollingFrame
		CommitsScrollingFrame.CanvasSize = UDim2.new(0,0,0,20)
	end
	
	--[[
	Updates the displayed commits.
	--]]
	local LastDisplayedCommits
	local function UpdateDisplayedCommits(OverrideRemote,OverrideBranch)
		local Remote,Branch = OverrideRemote or RemoteTextBox.Text,OverrideBranch or BranchTextBox.Text
		local CombinedName = Remote.."/"..Branch
		if CombinedName == LastDisplayedCommits then return end
		LastDisplayedCommits = CombinedName
		
		--Clear the list.
		SetListSingleLine("Loading commits...")
		
		--Get the commits.
		local Worked,Commits = pcall(function()
			return ListCommitsRequest.new(Host,Remote,Branch):GetCommits()
		end)
		
		--Retrn if it should not be loaded.
		if CombinedName ~= LastDisplayedCommits then
			return
		end
		
		--Display an error message if it failed.
		if not Worked then
			SetListSingleLine("Get commits failed because "..tostring(Commits))
			return
		end
		
		--Create the commits list.
		CommitsScrollingFrame:ClearAllChildren()
		local CurrentSpot = 0
		for _,Commit in pairs(Commits) do
			local Id,Message = Commit.id,Commit.message
			
			local CommitIdLabel = NexusWrappedInstance.GetInstance("TextLabel")
			CommitIdLabel.Text = "Commit "..Id
			CommitIdLabel.Position = UDim2.new(0,2,0,20 * CurrentSpot)
			CommitIdLabel.Size = UDim2.new(0,100,0,20)
			CommitIdLabel.TextXAlignment = "Left"
			CommitIdLabel.Parent = CommitsScrollingFrame
			CurrentSpot = CurrentSpot + 1
			
			for _,Line in pairs(string.split(Message,"\n")) do
				local CommitMessageLabel = NexusWrappedInstance.GetInstance("TextLabel")
				CommitMessageLabel.Text = Line
				CommitMessageLabel.Size = UDim2.new(0,100,0,20)
				CommitMessageLabel.Position = UDim2.new(0,22,0,20 * CurrentSpot)
				CommitMessageLabel.TextXAlignment = "Left"
				CommitMessageLabel.Parent = CommitsScrollingFrame
				CurrentSpot = CurrentSpot + 1
			end
		end
		CommitsScrollingFrame.CanvasSize = UDim2.new(0,0,0,20 * CurrentSpot)
	end
	
	--[[
	Updates the error message.
	--]]
	local function UpdateErrorMessage()
		if not RemotesToBranches[RemoteTextBox.Text] then
			--Display a message if the remote is unknown.
			ErrorMessageLabel.Text = "Remote is unknown"
			PushButton.Visible = false
			
			--Clear the commits.
			LastDisplayedCommits = nil
			SetListSingleLine("Remote is unknown")
		else
			PushButton.Visible = true
			
			--Find the branch.
			local BranchFound = false
			for _,Branch in pairs(RemotesToBranches[RemoteTextBox.Text]) do
				if Branch == BranchTextBox.Text then
					BranchFound = true
					break
				end
			end
			
			--Display a message if the branch was not found.
			if not BranchFound then
				ErrorMessageLabel.Text = "New remote branch will be created"
				UpdateDisplayedCommits(DefaultRemote,DefaultBranch)
			else
				ErrorMessageLabel.Text = ""
				UpdateDisplayedCommits()
			end
		end
	end
	
	--Set up the text box events.
	RemoteTextBox:GetPropertyChangedSignal("Text"):Connect(UpdateErrorMessage)
	BranchTextBox:GetPropertyChangedSignal("Text"):Connect(UpdateErrorMessage)
	UpdateErrorMessage()
	
	--Set up the clicked events.
	local DB = true
	PushButton.MouseButton1Down:Connect(function()
		if DB then
			DB = false
			self.object:PushCommits(RemoteTextBox.Text,BranchTextBox.Text)
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
Sends a git request to push.
--]]
function GitPushView:SendPushRequest(RemoteRepo,RemoteBranch)
	RemotePushRequest.new(self.Host,RemoteRepo,RemoteBranch):SendRequest()
end

--[[
Sends the push request to the server.
--]]
function GitPushView:PushCommits(RemoteRepo,RemoteBranch)
	--Send the network requests to push.
	local Worked,Return = pcall(function()
		self:SendPushRequest(RemoteRepo,RemoteBranch)
	end)
	
	--Show either a success or failure.
	if Worked then
		self:UpdatePushStatus(true,"Push successful.")
	else
		self:UpdatePushStatus(false,"Push failed.")
		warn("Push request failed because "..tostring(Return))
	end
end

--[[
Updates the status of the push.
--]]
function GitPushView:UpdatePushStatus(Percent,StatusText)
	if self ~= self.object then
		self.object:UpdatePushStatus(Percent,StatusText)
	end
end

--[[
Closes the view.
--]]
function GitPushView:Close()
	self:Destroy()
end



return GitPushView