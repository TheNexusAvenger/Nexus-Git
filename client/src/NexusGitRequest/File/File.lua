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
	self.Status = NexusEnums.FileStatus.Untracked
	
	--Set the file name.
	local ChangedNameIndex = string.find(FileName,"->")
	if ChangedNameIndex then
		self.FileName = string.sub(FileName,ChangedNameIndex + 3)
		self.PreviousFileName = string.sub(FileName,1,ChangedNameIndex - 2)
	else
		self.FileName = FileName
		self.PreviousFileName = nil
	end
end

--[[
Returns the file name.
--]]
function File:GetFileName()
	return self.FileName
end

--[[
Returns the previous file name.
--]]
function File:GetPreviousFileName()
	return self.PreviousFileName
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