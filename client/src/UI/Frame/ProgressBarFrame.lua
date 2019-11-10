--[[
TheNexusAvenger

Class representing a progress bar.
--]]

local NexusGit = require(script.Parent.Parent.Parent):GetContext(script)
local NexusWrappedInstance = NexusGit:GetResource("NexusPluginFramework.Base.NexusWrappedInstance")

local ProgressBarFrame = NexusWrappedInstance:Extend()
ProgressBarFrame:SetClassName("ProgressBarFrame")



--[[
Creates a progress bar frame object.
--]]
function ProgressBarFrame:__new()
	self:InitializeSuper("Frame")
	
	--Set the default values.
	self.BackgroundColor3 = "InputFieldBackground"
	self.BorderColor3 = "InputFieldBorder"
	self.BorderSizePixel = 1
		
	--Create a fill frame.
	local FillFrame = NexusWrappedInstance.GetInstance("Frame")
	FillFrame.Hidden = true
	FillFrame.BorderSizePixel = 0
	FillFrame.Parent = self
	
	--Set up the color changes.
	self:__SetChangedOverride("FillColor3",function()
		FillFrame.BackgroundColor3 = self.FillColor3
	end)
	
	--Set up the fill changes.
	self:__SetChangedOverride("FillAmount",function()
		FillFrame.Size = UDim2.new(self.FillAmount,0,1,0)
	end)
	
	--Set the default values.
	self.FillColor3 = "DialogMainButton"
	self.FillAmount = 0
end

--[[
Sets the progress bar as normal.
--]]
function ProgressBarFrame:SetAsNormal()
	self.FillColor3 = "DialogMainButton"
end

--[[
Sets the progress bar as failed.
--]]
function ProgressBarFrame:SetAsFailed()
	self.FillColor3 = "ScriptError"
end



return ProgressBarFrame