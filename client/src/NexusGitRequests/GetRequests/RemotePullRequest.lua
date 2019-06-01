--[[
TheNexusAvenger

Requests remote pulling on the server.
--]]

local Root = script.Parent.Parent.Parent
local NexusInstance = require(Root:WaitForChild("NexusInstance"):WaitForChild("NexusInstance"))
local SplitHttp = Root:WaitForChild("SplitHttp")
local MultiHttpRequest = require(SplitHttp:WaitForChild("MultiHttpRequest"))
local HttpService = game:GetService("HttpService")

local RemotePullRequest = NexusInstance:Extend()
RemotePullRequest:SetClassName("RemotePullRequest")
RemotePullRequest:Implements(require(SplitHttp:WaitForChild("HttpRequest")))



--[[
Creates a remote pull request.
--]]
function RemotePullRequest:__new(BaseURL,Remote,Branch)
	self.Request = MultiHttpRequest.new("GET",BaseURL.."/RemotePull?remote="..Remote.."&branch="..Branch)
end

--[[
Sends the request and returns a response.
--]]
function RemotePullRequest:SendRequest()
	return self.Request:SendRequest()
end

--[[
Preforms the remote pull and parses the response
--]]
function RemotePullRequest:RemotePull()
	local CurrentReadMode
	local WasUpdateToDate = false
	local ChangedFiles = {
		Modified = {},
		Created = {},
		Deleted = {},
	}
	
	--Read and parse the response.
	local Response = self:SendRequest():GetResponse()
	local ParsedResponse = HttpService:JSONDecode(Response)
	
	--Return the response if it is only 1 line.
	if #ParsedResponse == 1 then
		return ParsedResponse[1]
	end
	
	--Parse the lines.
	for _,Line in pairs(ParsedResponse) do
		--Set the new read mode or read the line.
		if Line == "Already up to date." then
			WasUpdateToDate = true
		elseif Line == "Modified:" then
			CurrentReadMode = "Modified"
		elseif Line == "Created:" then
			CurrentReadMode = "Created"
		elseif Line == "Deleted:" then
			CurrentReadMode = "Deleted"
		else
			--Get the table to add to.
			local TableToAddTo = ChangedFiles.Modified
			if CurrentReadMode == "Created" then
				TableToAddTo = ChangedFiles.Created
			elseif CurrentReadMode == "Deleted" then
				TableToAddTo = ChangedFiles.Deleted
			elseif CurrentReadMode == "Modified" then
				TableToAddTo = ChangedFiles.Modified
			end
			
			--Add the line.
			table.insert(TableToAddTo,Line)
		end
		
	end
	
	--Return the result.
	return {
		WasUpdateToDate = WasUpdateToDate,
		ChangedFiles = ChangedFiles,
	}
end



return RemotePullRequest