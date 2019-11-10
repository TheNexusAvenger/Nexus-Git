--[[
TheNexusAvenger

Class representing a view for adding files.
--]]

local NexusGit = require(script.Parent.Parent.Parent.Parent):GetContext(script)
local GitAddRequest = NexusGit:GetResource("NexusGitRequest.PostRequest.GitAddRequest")
local Directory = NexusGit:GetResource("NexusGitRequest.File.Directory")
local NexusWrappedInstance = NexusGit:GetResource("NexusPluginFramework.Base.NexusWrappedInstance")
local FileSelectionFrame = NexusGit:GetResource("UI.Frame.FileSelection.FileSelectionFrame")
local NexusEnums = NexusGit:GetResource("NexusPluginFramework.Data.Enum.NexusEnumCollection").GetBuiltInEnums()

local GitAddView = NexusWrappedInstance:Extend()
GitAddView:SetClassName("GitAddView")
GitAddView:Implements(NexusGit:GetResource("UI.View.Frame.IViewFrame"))





--[[
Creates an git add view object.
--]]
function GitAddView:__new(Files,Host)
	self:InitializeSuper("Frame")
	
	--Store the host.
	self:__SetChangedOverride("Host",function() end)
	self.Host = Host
	
	--Create the list frame.
	local FilterdFiles = Directory.FilterFiles(Files,{NexusEnums.FileStatus.Untracked})
	local ListFrame = FileSelectionFrame.new(FilterdFiles)
	ListFrame.BorderSizePixel = 1
	ListFrame.Size = UDim2.new(1,0,1,-30)
	ListFrame.Parent = self
	self:__SetChangedOverride("ListFrame",function() end)
	self.ListFrame = ListFrame
	
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
	
	local AddButton = NexusWrappedInstance.GetInstance("TextButton")
	AddButton.Size = UDim2.new(0,84,0,22)
	AddButton.Position = UDim2.new(1,-176,1,-26)
	AddButton.BackgroundColor3 = "DialogMainButton"
	AddButton.TextColor3 = "DialogMainButtonText"
	AddButton.Text = "Add"
	AddButton.Parent = self
	self:__SetChangedOverride("AddButton",function() end)
	self.AddButton = AddButton
	
	--Set up the clicked events.
	local DB = true
	AddButton.MouseButton1Down:Connect(function()
		if DB then
			DB = false
			self.object:AddFiles(ListFrame:GetSelectedFiles())
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
function GitAddView:SendAddRequest(Files)
	GitAddRequest.new(self.Host,Files):SendRequest()
end

--[[
Sends add requests to the server.
--]]
function GitAddView:AddFiles(SelectedFiles)
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
			self:UpdateAddStatus((i - 1)/#FilesList,"Adding "..tostring(FileString).."...")
			self:SendAddRequest({FileString})
		end
	end)
	
	--Show either a success or failure.
	if Worked then
		if #FilesList == 1 then
			self:UpdateAddStatus(true,"1 file is now tracked.")
		else
			self:UpdateAddStatus(true,tostring(#FilesList).." files are now tracked.")
		end
	else
		self:UpdateAddStatus(false,"Add failed.")
		warn("Add request failed because "..tostring(Return))
	end
end

--[[
Updates the status of the add.
--]]
function GitAddView:UpdateAddStatus(Percent,StatusText)
	if self ~= self.object then
		self.object:UpdateAddStatus(Percent,StatusText)
	end
end

--[[
Closes the view.
--]]
function GitAddView:Close()
	self:Destroy()
end



return GitAddView