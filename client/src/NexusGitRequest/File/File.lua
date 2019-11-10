--[[
TheNexusAvenger

Contains information about a file.
--]]

local NexusGit = require(script.Parent.Parent.Parent):GetContext(script)
local NexusInstance = NexusGit:GetResource("NexusInstance.NexusInstance")
local NexusEnums = NexusGit:GetResource("NexusPluginFramework.Data.Enum.NexusEnumCollection").GetBuiltInEnums()

local File = NexusInstance:Extend()
File:SetClassName("File")



--[[
Creates a file object.
--]]
function File:__new(FileName)
	self:InitializeSuper()
	self.FileName = FileName
	self.Status = NexusEnums.FileStatus.Untracked
end

--[[
Returns the file name.
--]]
function File:GetFileName()
	return self.FileName
end

--[[
Sets the status of the file.
--]]
function File:SetStatus(Status)
	self.Status = Status
end

--[[
Returns the status of the file.
--]]
function File:GetStatus()
	return self.Status
end



return File