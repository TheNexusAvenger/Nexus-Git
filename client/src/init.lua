--[[
TheNexusAvenger

Project for managing Nexus Git.
--]]

local NexusProject = require(script:WaitForChild("NexusPluginFramework"):WaitForChild("NexusProject"))
local NexusGitProject = NexusProject.new(script)



--Add the enums.
local NexusEnums = NexusGitProject:GetResource("NexusPluginFramework.Data.Enum.NexusEnumCollection").GetBuiltInEnums()
NexusEnums:CreateEnum("FileStatus","Unmodified")
NexusEnums:CreateEnum("FileStatus","Untracked")
NexusEnums:CreateEnum("FileStatus","Created")
NexusEnums:CreateEnum("FileStatus","Deleted")
NexusEnums:CreateEnum("FileStatus","Modified")
NexusEnums:CreateEnum("FileStatus","Renamed")
NexusEnums:CreateEnum("NexusGitAction","GitAdd")
NexusEnums:CreateEnum("NexusGitAction","GitCommit")
NexusEnums:CreateEnum("NexusGitAction","GitPull")
NexusEnums:CreateEnum("NexusGitAction","GitPush")
NexusEnums:CreateEnum("NexusGitAction","LocalPull")
NexusEnums:CreateEnum("NexusGitAction","LocalPush")
NexusEnums:CreateEnum("NexusGitAction","ShowBranches")
NexusEnums:CreateEnum("NexusGitAction","ShowSettings")
NexusEnums:CreateEnum("NexusGitAction","ShowVersion")



return NexusGitProject