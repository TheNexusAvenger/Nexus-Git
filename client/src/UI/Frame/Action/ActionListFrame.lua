--[[
TheNexusAvenger

Class representing a list of actions.
--]]

local NexusGit = require(script.Parent.Parent.Parent.Parent):GetContext(script)
local NexusWrappedInstance = NexusGit:GetResource("NexusPluginFramework.Base.NexusWrappedInstance")
local NexusScrollingFrame = NexusGit:GetResource("NexusPluginFramework.UI.Scroll.NexusScrollingFrame")
local ActionFrame = NexusGit:GetResource("UI.Frame.Action.ActionFrame")
local NexusEnums = NexusGit:GetResource("NexusPluginFramework.Data.Enum.NexusEnumCollection").GetBuiltInEnums()

local ActionListFrame = NexusScrollingFrame:Extend()
ActionListFrame:SetClassName("ActionListFrame")



--[[
Creates a action list object.
--]]
function ActionListFrame:__new(Actions)
	self:InitializeSuper(NexusEnums.NexusScrollTheme.Qt5)
	
	--Add a UI List Constraint.
	local UIListLayout = NexusWrappedInstance.GetInstance("UIListLayout")
	UIListLayout.Hidden = true
	UIListLayout.Parent = self
	
	--Add the actions.
	for _,Action in pairs(Actions) do
		local Frame = ActionFrame.new(Action)
		Frame.Parent = self
	end
	
	--Set the canvas size.
	self.CanvasSize = UDim2.new(0,0,0,20 * #Actions)
end



return ActionListFrame