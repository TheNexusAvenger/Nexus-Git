--[[
TheNexusAvenger

Class that contains a name of a file and a
check box if the file is selected.
--]]

local GIT_STATUS_COLORS = {
	["NexusEnum.FileStatus.Unmodified"] = "MainText",
	["NexusEnum.FileStatus.Untracked"] = Color3.new(208/255,84/255,62/255),
	["NexusEnum.FileStatus.Created"] = Color3.new(80/255,150/255,79/255),
	["NexusEnum.FileStatus.Deleted"] = "SubText",
	["NexusEnum.FileStatus.Modified"] = Color3.new(87/255,150/255,165/255),
	["NexusEnum.FileStatus.Renamed"] = Color3.new(87/255,150/255,165/255),
}



local NexusGit = require(script.Parent.Parent.Parent.Parent):GetContext(script)
local NexusWrappedInstance = NexusGit:GetResource("NexusPluginFramework.Base.NexusWrappedInstance")
local NexusCollapsableListFrame = NexusGit:GetResource("NexusPluginFramework.UI.CollapsableList.NexusCollapsableListFrame")
local NexusCheckBox = NexusGit:GetResource("NexusPluginFramework.UI.Input.NexusCheckBox")
local Directory = NexusGit:GetResource("NexusGitRequest.File.Directory")
local NexusEnums = NexusGit:GetResource("NexusPluginFramework.Data.Enum.NexusEnumCollection").GetBuiltInEnums()

local FileListFrame = NexusCollapsableListFrame:Extend()
FileListFrame:SetClassName("FileListFrame")

local TextService = game:GetService("TextService")



--[[
Creates a list frame and children frames from
a directory or file.
--]]
function FileListFrame.FromFile(File)
	--Create the list frame.
	local ListFrame = FileListFrame.new(File)
	ListFrame.Expanded = true
	
	--Add the directory files.
	if File:IsA("Directory") then
		for _,SubFile in pairs(File:GetFiles()) do
			local SubListFrame = FileListFrame.FromFile(SubFile)
			SubListFrame.Parent = ListFrame:GetCollapsableContainer()
		end
	end	
	
	--Return the list frame.
	return ListFrame
end

--[[
Creates a file list frame object.
--]]
function FileListFrame:__new(File)
	self:InitializeSuper()
	self.__BoundingSizeConstraint:Destroy()
	self.Selectable = false
	
	--Get and set the file name.
	local FileName = File:GetFileName()
	local FileStatus = File:GetStatus()
	self.Name = File:GetFileName()
	
	--Set up the size.
	local TextBoundsX = TextService:GetTextSize(FileName,14,Enum.Font.SourceSans,Vector2.new(10000,10000)).X
	self:__SetChangedOverride("TextBoundsX",function() end)
	self.TextBoundsX = TextBoundsX
	self.Size = UDim2.new(0,22 + TextBoundsX,0,20)
	
	--Store the file.
	self:__SetChangedOverride("File",function() end)
	self.File = File
	
	--Add a check box.
	local CheckBox = NexusCheckBox.new()
	CheckBox.Hidden = true
	CheckBox.Size = UDim2.new(0,14,0,14)
	CheckBox.Position = UDim2.new(0,3,0,3)
	CheckBox.Parent = self:GetMainContainer()
	self:__SetChangedOverride("CheckBox",function() end)
	self.CheckBox = CheckBox
	
	--Add the file name.
	local TextLabel = NexusWrappedInstance.GetInstance("TextLabel")
	TextLabel.Hidden = true
	TextLabel.Size = UDim2.new(1,-20,0,16)
	TextLabel.Position = UDim2.new(0,22,0,2)
	TextLabel.Text = FileName
	TextLabel.TextColor3 = GIT_STATUS_COLORS[tostring(FileStatus)]
	TextLabel.Parent = self:GetMainContainer()
	self:__SetChangedOverride("TextLabel",function() end)
	self.TextLabel = TextLabel

	--Make deleted files more clear.
	if tostring(FileStatus) == "NexusEnum.FileStatus.Deleted" then
		TextLabel.Font = "SourceSansItalic"
		TextLabel.Text = FileName.." (Deleted)"
	end
	
	--Add a UI constaint to make the children list.
	local UIListLayout = NexusWrappedInstance.GetInstance("UIListLayout")
	UIListLayout.Hidden = true
	UIListLayout.Parent = self:GetCollapsableContainer()
	
	--Set up changes to the checked state.
	self:__SetChangedOverride("IgnoreChanges",function() end)
	self:__SetChangedOverride("CheckedState",function()
		self.CheckBox.BoxState = self.CheckedState
		self:UpdateChildrenSelection()
	end)
	self.CheckBox:GetPropertyChangedSignal("BoxState"):Connect(function()
		self.CheckedState = self.CheckBox.BoxState
	end)
	
	--Set up children being added and removed.
	local ChangedSignals = {}
	self.IgnoreChanges = {}
	self:GetCollapsableContainer().ChildAdded:Connect(function(Child)
		if not ChangedSignals[Child] and Child:IsA(self.ClassName) then
			ChangedSignals[Child] = Child:GetPropertyChangedSignal("CheckedState"):Connect(function()
				if not self.IgnoreChanges[Child] then
					self:UpdateCheckedState()
				else
					self.IgnoreChanges[Child] = nil
				end
			end)
			self:UpdateCheckedState()
		end
	end)
	self:GetCollapsableContainer().ChildRemoved:Connect(function(Child)
		local Connection = ChangedSignals[Child]
		if Connection then
			Connection:Disconnect()
			ChangedSignals[Child] = nil
			self:UpdateCheckedState()
		end
	end)
	
	--Set the checked state.
	self:InitializeConstraint()
	self:UpdateCheckedState()
	self.CheckedState = NexusEnums.CheckBoxState.Unchecked
end

--[[
Sets up the height constraint.
--]]
function FileListFrame:InitializeConstraint()
	local ListFrameEvents = {}
	
	--[[
	Updates the size of the frame.
	--]]
	local function UpdateSize()
		--Get the bounding size y.
		local SizeY = 0
		if self.Expanded then
			for Frame,_ in pairs(ListFrameEvents) do
				SizeY = SizeY + Frame:GetWrappedInstance().AbsoluteSize.Y
			end
		end
		
		--Set the size.
		self:GetCollapsableContainer().Size = UDim2.new(0,self.TextBoundsX,0,SizeY)
		self:GetWrappedInstance().Size = UDim2.new(UDim.new(0,22 + self.TextBoundsX),UDim.new(self.Size.Y.Scale,self.Size.Y.Offset + SizeY))
	end
	
	--[[
	Handles a list frame being added.
	--]]
	local function ListFrameAdded(ListFrame)
		if ListFrame:IsA(self.ClassName) then
			local Events = {}
			
			--Connect the events.
			table.insert(Events,ListFrame:GetWrappedInstance():GetPropertyChangedSignal("AbsoluteSize"):Connect(UpdateSize))
			
			--Store the events.
			ListFrameEvents[ListFrame] = Events
		end
	end
	
	--[[
	Handles a list frame being removed.
	--]]
	local function ListFrameRemoved(ListFrame)
		local Events = ListFrameEvents[ListFrame]
		if Events then
			for _,Event in pairs(Events) do
				Event:Disconnect()
			end
		end
		
		ListFrameEvents[ListFrame] = nil
	end
	
	--Connect the events.
	self:GetPropertyChangedSignal("Expanded"):Connect(UpdateSize)
	self:GetCollapsableContainer().ChildAdded:Connect(ListFrameAdded)
	self:GetCollapsableContainer().ChildRemoved:Connect(ListFrameRemoved)
	UpdateSize()
end

--[[
Updates the checked state based on the children.
--]]
function FileListFrame:UpdateCheckedState()
	local SelectedChildren,MixedChildren,UnselectedChildren = 0,0,0
	
	--Add the children.
	for _,ChildFrame in pairs(self:GetCollapsableContainer():GetChildren()) do
		if ChildFrame:IsA(self.ClassName) then
			if NexusEnums.CheckBoxState.Unchecked:Equals(ChildFrame.CheckedState) then
				UnselectedChildren = UnselectedChildren + 1
			elseif NexusEnums.CheckBoxState.Checked:Equals(ChildFrame.CheckedState) then
				SelectedChildren = SelectedChildren + 1
			else
				MixedChildren = MixedChildren + 1
			end
		end
	end
	
	--Update the checked state.
	if SelectedChildren == 0 and MixedChildren == 0 and UnselectedChildren > 0 then
		self.CheckedState = NexusEnums.CheckBoxState.Unchecked
	elseif SelectedChildren > 0 and MixedChildren == 0 and UnselectedChildren == 0 then
		self.CheckedState = NexusEnums.CheckBoxState.Checked
	elseif (SelectedChildren > 0 and UnselectedChildren > 0) or MixedChildren > 0 then
		self.CheckedState = NexusEnums.CheckBoxState.Mixed
	end
end

--[[
Updates the children if the list frame
is checked or unchecked.
--]]
function FileListFrame:UpdateChildrenSelection()
	if not NexusEnums.CheckBoxState.Mixed:Equals(self.CheckedState) then
		for _,ChildFrame in pairs(self:GetCollapsableContainer():GetChildren()) do
			if ChildFrame:IsA(self.ClassName) and ChildFrame.CheckedState ~= self.CheckedState then
				self.IgnoreChanges[ChildFrame] = true
				ChildFrame.CheckedState = self.CheckedState
			end
		end
	end
end

--[[
Returns the bounding size of the frame.
in the x axis.
--]]
function FileListFrame:GetBoundingSizeX()
	--Get the base value.
	local AbsoluteSizeY = self:GetMainContainer().AbsoluteSize.Y
	local BoundingSizeX = 22 + self.TextBoundsX
	
	--Get the bounding size of the sub-frames.
	for _,ChildFrame in pairs(self:GetCollapsableContainer():GetChildren()) do
		if ChildFrame:IsA(self.ClassName) then
			local ChildBoundingSize = AbsoluteSizeY + ChildFrame:GetBoundingSizeX()
			if ChildBoundingSize > BoundingSizeX then
				BoundingSizeX = ChildBoundingSize
			end
		end
	end
	
	--Reutrn the bounding size.
	return BoundingSizeX
end

--[[
Returns the selected files. If the frame
is unselected, nil is returned.
--]]
function FileListFrame:GetSelectedFiles()
	--Return nil if the frame is not selected.
	if NexusEnums.CheckBoxState.Unchecked:Equals(self.CheckedState) then
		return nil
	end
	
	--Return the file.
	if self.File:IsA("Directory") then
		--Clone the directory.
		local Directory = Directory.new(self.File:GetFileName())
		
		--Add the selected children.
		for _,ListFrame in pairs(self:GetCollapsableContainer():GetChildren()) do
			if ListFrame:IsA("FileListFrame") and not NexusEnums.CheckBoxState.Unchecked:Equals(ListFrame.CheckedState) then
				Directory:AddFile(ListFrame:GetSelectedFiles())
			end
		end
		
		--Return the directory.
		return Directory
	else
		return self.File
	end
end



return FileListFrame