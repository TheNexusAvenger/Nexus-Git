--[[
TheNexusAvenger

Class that invokes actions.
--]]

local NexusGit = require(script.Parent.Parent.Parent.Parent):GetContext(script)
local NexusWrappedInstance = NexusGit:GetResource("NexusPluginFramework.Base.NexusWrappedInstance")
local NexusCollapsableListFrame = NexusGit:GetResource("NexusPluginFramework.UI.CollapsableList.NexusCollapsableListFrame")
local ActionIcon = NexusGit:GetResource("UI.Frame.Action.ActionIcon")

local ActionFrame = NexusCollapsableListFrame:Extend()
ActionFrame:SetClassName("ActionFrame")

local TextService = game:GetService("TextService")


--[[
Creates a file list frame object.
--]]
function ActionFrame:__new(Action)
	self:InitializeSuper()
	self.__BoundingSizeConstraint:Destroy()
	self.ArrowVisible = false
	self.Size = UDim2.new(1,0,0,20)
	
	--Add the icon.
	local Icon = ActionIcon.new(Action:GetActionEnum())
	Icon.Hidden = true
	Icon.Size = UDim2.new(0,14,0,14)
	Icon.Position = UDim2.new(0,3,0,3)
	Icon.Parent = self:GetMainContainer()
	
	--Add the action name.
	local TextLabel = NexusWrappedInstance.GetInstance("TextLabel")
	TextLabel.Hidden = true
	TextLabel.Size = UDim2.new(1,-20,0,16)
	TextLabel.Position = UDim2.new(0,22,0,2)
	TextLabel.Text = Action:GetDisplayName()
	TextLabel.Parent = self:GetMainContainer()
	
	--Set up disabling selecting.
	self:GetPropertyChangedSignal("Selected"):Connect(function()
		if self.Selected == true then
			self.Selected = false
			Action:PerformAction()
		end
	end)
end



return ActionFrame