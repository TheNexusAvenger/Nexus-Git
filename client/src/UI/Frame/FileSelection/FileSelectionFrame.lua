--[[
TheNexusAvenger

Class representing a list of directories.
--]]

local NexusGit = require(script.Parent.Parent.Parent.Parent):GetContext(script)
local NexusWrappedInstance = NexusGit:GetResource("NexusPluginFramework.Base.NexusWrappedInstance")
local NexusScrollingFrame = NexusGit:GetResource("NexusPluginFramework.UI.Scroll.NexusScrollingFrame")
local NexusEnums = NexusGit:GetResource("NexusPluginFramework.Data.Enum.NexusEnumCollection").GetBuiltInEnums()
local FileListFrame = NexusGit:GetResource("UI.Frame.FileSelection.FileListFrame")

local FileSelectionFrame = NexusScrollingFrame:Extend()
FileSelectionFrame:SetClassName("FileSelectionFrame")



--[[
Creates a progress bar frame object.
--]]
function FileSelectionFrame:__new(Files)
	self:InitializeSuper(NexusEnums.NexusScrollTheme.Qt5)
	local ListFrames = {}
	self:__SetChangedOverride("ListFrames",function() end)
	self.ListFrames = ListFrames
	
	--Add a UI List Constraint.
	local UIListLayout = NexusWrappedInstance.GetInstance("UIListLayout")
	UIListLayout.Hidden = true
	UIListLayout.Parent = self
	
	--[[
	Updates the canvas size.
	--]]
	local function UpdateCanvasSize()
		--Get the bounding size X.
		local BoundingSizeX = 0
		for _,ListFrame in pairs(ListFrames) do
			local NewBoundingSizeX = ListFrame:GetBoundingSizeX()
			if NewBoundingSizeX > BoundingSizeX then
				BoundingSizeX = NewBoundingSizeX
			end
		end
		
		--Set the canvas size.
		self.CanvasSize = UDim2.new(0,BoundingSizeX,0,UIListLayout.AbsoluteContentSize.Y)
	end
	
	--Add the file selection frames.
	for _,File in pairs(Files) do
		local ListFrame = FileListFrame.FromFile(File)
		table.insert(ListFrames,ListFrame)
		ListFrame.Parent = self
	end
	
	--Set up updating the canvas size.
	self:GetPropertyChangedSignal("Parent"):Connect(UpdateCanvasSize)
	UIListLayout:GetPropertyChangedSignal("AbsoluteContentSize"):Connect(UpdateCanvasSize)
	UpdateCanvasSize()
end

--[[
Returns the selected files.
--]]
function FileSelectionFrame:GetSelectedFiles()
	local SelectedFiles = {}
	
	--Add the selected files.
	for _,ListFrame in pairs(self.ListFrames) do
		local ListFrameSelectedFiles = ListFrame:GetSelectedFiles()
		if ListFrameSelectedFiles ~= nil then
			table.insert(SelectedFiles,ListFrameSelectedFiles)
		end
	end
	
	--Return the selected files.
	return SelectedFiles
end



return FileSelectionFrame