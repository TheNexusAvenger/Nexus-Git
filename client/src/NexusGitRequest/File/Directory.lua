--[[
TheNexusAvenger

Contains a set of files.
--]]

local NexusGit = require(script.Parent.Parent.Parent):GetContext(script)
local File = NexusGit:GetResource("NexusGitRequest.File.File")
local NexusEnums = NexusGit:GetResource("NexusPluginFramework.Data.Enum.NexusEnumCollection").GetBuiltInEnums()

local Directory = File:Extend()
Directory:SetClassName("Directory")



--[[
Converts a list of files to file and directories.
--]]
function Directory.FromStrings(Strings)
	--Create a "root" directory.
	local RootDirectory = Directory.new("")
	
	--Add the files.
	for _,String in pairs(Strings) do
		local CurrentDirectory = RootDirectory
		local SplitStrings = string.split(String,"/")
		
		--Add the directories.
		for i = 1,#SplitStrings - 1 do
			local DirectoryName = SplitStrings[i]
			local NextDirectory = CurrentDirectory:GetFile(DirectoryName)
			if not NextDirectory or not NextDirectory:IsA("Directory") then
				NextDirectory = Directory.new(DirectoryName)
				CurrentDirectory:AddFile(NextDirectory)
			end
			
			CurrentDirectory = NextDirectory
		end
		
		--Add the file.
		local FileName = SplitStrings[#SplitStrings]
		local ExistingFile = CurrentDirectory:GetFile(FileName)
		if not ExistingFile or ExistingFile:IsA("Directory") then
			CurrentDirectory:AddFile(File.new(FileName))
		end
	end
	
	--Return the files.
	return RootDirectory:GetFiles()
end

--[[
Filters a list of directories for a given set
of file statuses. Note that if a directory is
empty, it will be removed.
--]]
function Directory.FilterFiles(Files,Statuses)
	local FilteredFiles = {}
	
	--Turn the statuses to a map.
	local StatusesMap = {}
	for _,Status in pairs(Statuses) do
		StatusesMap[tostring(Status)] = true
	end
	
	--Add the filtered files.
	for _,File in pairs(Files) do
		if File:IsA("Directory") then
			local SubFilteredFiles = Directory.FilterFiles(File:GetFiles(),Statuses)
			if #SubFilteredFiles > 0 then
				local NewDirectory = Directory.new(File:GetFileName())
				for _,SubFile in pairs(SubFilteredFiles) do
					NewDirectory:AddFile(SubFile)
				end
				table.insert(FilteredFiles,NewDirectory)
			end
		elseif StatusesMap[tostring(File:GetStatus())] then
			table.insert(FilteredFiles,File)
		end
	end
	
	--Return the files.
	return FilteredFiles
end

--[[
Creates a directory object.
--]]
function Directory:__new(DirectoryName)
	self:InitializeSuper(DirectoryName)
	self.Files = {}
end

--[[
Returns the status of the file.
--]]
function Directory:GetStatus()
	local Untracked,Created,Deleted,Modified,Renamed = 0,0,0,0,0
	
	--Update the counts.
	for _,File in pairs(self:GetFiles()) do
		if NexusEnums.FileStatus.Untracked:Equals(File:GetStatus()) then
			Untracked = Untracked + 1
		elseif NexusEnums.FileStatus.Created:Equals(File:GetStatus()) then
			Created = Created + 1
		elseif NexusEnums.FileStatus.Deleted:Equals(File:GetStatus()) then
			Deleted = Deleted + 1
		elseif NexusEnums.FileStatus.Modified:Equals(File:GetStatus()) then
			Modified = Modified + 1
		elseif NexusEnums.FileStatus.Renamed:Equals(File:GetStatus()) then
			Renamed = Renamed + 1
		end
	end
	
	--Return the status.
	if Untracked > 0 and Created == 0 and Deleted == 0 and Modified == 0 and Renamed == 0 then
		return NexusEnums.FileStatus.Untracked
	elseif Untracked == 0 and Created > 0 and Deleted == 0 and Modified == 0 and Renamed == 0 then
		return NexusEnums.FileStatus.Created
	elseif Untracked == 0 and Created == 0 and Deleted > 0 and Modified == 0 and Renamed == 0 then
		return NexusEnums.FileStatus.Deleted
	elseif Untracked == 0 and Created == 0 and Deleted == 0 and Modified == 0 and Renamed > 0 then
		return NexusEnums.FileStatus.Renamed
	end
	
	--Return unmodified (mixed state)
	return NexusEnums.FileStatus.Unmodified
end

--[[
Returns the files in the directory.
--]]
function Directory:GetFiles()
	--Clone the list.
	local FilesList = {}
	for _,File in pairs(self.Files) do
		table.insert(FilesList,File)
	end
	
	--Return the cloned list.
	return FilesList
end

--[[
Returns the file for a name.
--]]
function Directory:GetFile(FileName)
	for _,File in pairs(self.Files) do
		if File:GetFileName() == FileName then
			return File
		end
	end
end

--[[
Adds a file to the directory.
--]]
function Directory:AddFile(File)
	table.insert(self.Files,File)
end




return Directory