--[[
TheNexusAvenger

Class representing a view for pulling from remote repositories.
--]]

local NexusGit = require(script.Parent.Parent.Parent.Parent):GetContext(script)
local RemotePullRequest = NexusGit:GetResource("NexusGitRequest.GetRequest.RemotePullRequest")
local Directory = NexusGit:GetResource("NexusGitRequest.File.Directory")
local File = NexusGit:GetResource("NexusGitRequest.File.File")
local FileSelectionFrame = NexusGit:GetResource("UI.Frame.FileSelection.FileSelectionFrame")
local NexusWrappedInstance = NexusGit:GetResource("NexusPluginFramework.Base.NexusWrappedInstance")
local NexusScrollingFrame = NexusGit:GetResource("NexusPluginFramework.UI.Scroll.NexusScrollingFrame")
local NexusCollapsableListFrame = NexusGit:GetResource("NexusPluginFramework.UI.CollapsableList.NexusCollapsableListFrame")
local NexusEnums = NexusGit:GetResource("NexusPluginFramework.Data.Enum.NexusEnumCollection").GetBuiltInEnums()

local GitPullView = NexusWrappedInstance:Extend()
GitPullView:SetClassName("GitPullView")
GitPullView:Implements(NexusGit:GetResource("UI.View.Frame.IViewFrame"))





--[[
Creates an git pull view object.
--]]
function GitPullView:__new(Host,RemotesToBranches,DefaultRemote,DefaultBranch)
	self:InitializeSuper("Frame")
	
	--Store the host.
	self:__SetChangedOverride("Host",function() end)
	self.Host = Host
	
	--Convert the remotes and branches to files.
	local BranchesFiles = {}
	for Remote,Branches in pairs(RemotesToBranches) do
		local RemoteDirectory = Directory.new(Remote)
		table.insert(BranchesFiles,RemoteDirectory)
		
		--Add the branches.
		for _,Branch in pairs(Branches) do
			local BranchFile = File.new(Branch)
			BranchFile:SetStatus(NexusEnums.FileStatus.Unmodified)
			RemoteDirectory:AddFile(BranchFile)
		end
	end
	
	--Create the list.
	local RemotesListFrame = FileSelectionFrame.new(BranchesFiles)
	RemotesListFrame.BorderSizePixel = 1
	RemotesListFrame.Size = UDim2.new(1,0,1,-30)
	RemotesListFrame.Parent = self
	
	--Check the default branch.
	local DefaultRemoteListElement = RemotesListFrame:FindFirstChild(DefaultRemote)
	if DefaultRemoteListElement then
		local DefaultBranchListElement = DefaultRemoteListElement:GetCollapsableContainer():FindFirstChild(DefaultBranch)
		if DefaultBranchListElement then
			DefaultBranchListElement.CheckedState = NexusEnums.CheckBoxState.Checked
		end
	end
	
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
	
	local PullButton = NexusWrappedInstance.GetInstance("TextButton")
	PullButton.Size = UDim2.new(0,84,0,22)
	PullButton.Position = UDim2.new(1,-176,1,-26)
	PullButton.BackgroundColor3 = "DialogMainButton"
	PullButton.TextColor3 = "DialogMainButtonText"
	PullButton.Text = "Pull"
	PullButton.Parent = self
	self:__SetChangedOverride("PullButton",function() end)
	self.PullButton = PullButton
	
	--Set up the clicked events.
	local DB = true
	PullButton.MouseButton1Down:Connect(function()
		if DB then
			DB = false
			
			--Get the selected branches.
			local BranchesToPull = {}
			for _,RemoteDirectory in pairs(RemotesListFrame:GetSelectedFiles()) do
				for _,BranchFile in pairs(RemoteDirectory:GetFiles()) do
					table.insert(BranchesToPull,{RemoteDirectory:GetFileName(),BranchFile:GetFileName()})
				end
			end
			
			--Pull the selected branches.
			self.object:PullCommits(BranchesToPull)
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
Sends a git request to pull.
--]]
function GitPullView:SendPullRequest(RemoteRepo,RemoteBranch)
	RemotePullRequest.new(self.Host,RemoteRepo,RemoteBranch):SendRequest()
end

--[[
Sends the pull request to the server.
--]]
function GitPullView:PullCommits(RemoteBranches)
	--Pull the branches.
	for i,RemoteBranch in pairs(RemoteBranches) do
		local RemoteName,BranchName = RemoteBranch[1],RemoteBranch[2]
		self:UpdatePullStatus((i - 1)/#RemoteBranches,"Pulling from "..RemoteName.."/"..BranchName..".")
		
		local Worked,Return =  pcall(function()
			self:SendPullRequest(RemoteName,BranchName)
		end)
		
		--Show an error if one happened.
		if not Worked then
			self:UpdatePullStatus(false,"Pull failed.")
			warn("Pull request failed because "..tostring(Return))
			return
		end
	end
	
	--Show a success message.
	self:UpdatePullStatus(true,"Pull successful.")
end

--[[
Updates the status of the push.
--]]
function GitPullView:UpdatePullStatus(Percent,StatusText)
	if self ~= self.object then
		self.object:UpdatePullStatus(Percent,StatusText)
	end
end

--[[
Closes the view.
--]]
function GitPullView:Close()
	self:Destroy()
end



return GitPullView