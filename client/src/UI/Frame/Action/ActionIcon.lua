--[[
TheNexusAvenger

Class representing an image for an icon.
--]]

local NexusGit = require(script.Parent.Parent.Parent.Parent):GetContext(script)
local Configuration = NexusGit:GetResource("Configuration")
local NexusWrappedInstance = NexusGit:GetResource("NexusPluginFramework.Base.NexusWrappedInstance")
local NexusEnums = NexusGit:GetResource("NexusPluginFramework.Data.Enum.NexusEnumCollection").GetBuiltInEnums()

local ActionIcon = NexusWrappedInstance:Extend()
ActionIcon:SetClassName("ActionIcon")



local ICON_OFFSETS = {
	[NexusEnums.NexusGitAction.GitAdd] = Vector2.new(768,0),
	[NexusEnums.NexusGitAction.GitCommit] = Vector2.new(512,0),
	[NexusEnums.NexusGitAction.GitPull] = Vector2.new(0,256),
	[NexusEnums.NexusGitAction.GitPush] = Vector2.new(256,256),
	[NexusEnums.NexusGitAction.LocalPull] = Vector2.new(512,256),
	[NexusEnums.NexusGitAction.LocalPush] = Vector2.new(768,256),
	[NexusEnums.NexusGitAction.ShowBranches] = Vector2.new(0,512),
	[NexusEnums.NexusGitAction.ShowSettings] = Vector2.new(256,0),
	[NexusEnums.NexusGitAction.ShowVersion] = Vector2.new(0,0),
}



--[[
Creates a action itme object.
--]]
function ActionIcon:__new(ActionEnum)
	self:InitializeSuper("ImageLabel")
	
	--Get the offset.
	local ImageOffset
	for Action,Offset in pairs(ICON_OFFSETS) do
		if Action:Equals(ActionEnum) then
			ImageOffset = Offset
			break
		end
	end
	
	--Throw an error if the action is unknown.
	if ImageOffset == nil then
		error("Unknown action: "..tostring(ActionEnum))
	end
	
	--Set the default values.
	self.BackgroundTransparency = 1
	self.Image = Configuration.IconsSpritesheet
	self.ImageRectOffset = ImageOffset
	self.ImageRectSize = Vector2.new(256,256)
	self.ImageColor3 = "DialogMainButton"
end



return ActionIcon