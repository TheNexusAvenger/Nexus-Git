--[[
TheNexusAvenger

Multiple HTTP requests to the server.
This is for the case where over 1MB needs to be sent.
--]]

local MAX_BODY_LENGTH = 1000000
local PARALLIZE_SEQUENTIAL_REQUESTS = true



local NexusGit = require(script.Parent.Parent):GetContext(script)
local NexusInstance = NexusGit:GetResource("NexusInstance.NexusInstance")
local HttpRequest = NexusGit:GetResource("SplitHttp.HttpRequest")
local PartialHttpRequest = NexusGit:GetResource("SplitHttp.PartialHttpRequest")
local PartialHttpResponseRequest = NexusGit:GetResource("SplitHttp.PartialHttpResponseRequest")
local MultiHttpResponse = NexusGit:GetResource("SplitHttp.MultiHttpResponse")

local HttpService = game:GetService("HttpService")

local MultiHttpRequest = NexusInstance:Extend()
MultiHttpRequest:Implements(HttpRequest)
MultiHttpRequest:SetClassName("MultiHttpRequest")



--[[
Creates a partial Http request
--]]
function MultiHttpRequest:__new(Method,URL,Body)
	self.Method = Method
	self.URL = URL
	self.SplitBody = self:__SplitString(Body or "",MAX_BODY_LENGTH)
end

--[[
Splits a string based on the given maximum length.
--]]
function MultiHttpRequest:__SplitString(String,MaxLength)
	--Split the string.
	local SplitStrings = {}
	while String ~= "" do
		table.insert(SplitStrings,string.sub(String,1,MaxLength))
		String = string.sub(String,MaxLength + 1)
	end
	
	--Add an empty string if the split strings is empty.
	if #SplitStrings == 0 then
		table.insert(SplitStrings,"")
	end
	
	--Return the split strings.
	return SplitStrings
end

--[[
Sends a request and returns the response.
--]]
function MultiHttpRequest:__SendRequest(Id,RequestId)
	--Create and send the request.
	local Request = PartialHttpRequest.new(self.Method,self.URL,self.SplitBody[Id],RequestId,Id - 1,#self.SplitBody)
	return Request:SendRequest()
end

--[[
Sends a response request and returns the response.
--]]
function MultiHttpRequest:__GetResponse(Id,ResponseId)
	--Create and send the request.
	local Request = PartialHttpResponseRequest.new(self.URL,ResponseId,Id - 1)
	return Request:SendRequest()
end

--[[
Sends all of the requests and returns the first "success" request.
--]]
function MultiHttpRequest:__GetFirstResponse()
	--Send the first request.
	local FirstResponse = self:__SendRequest(1)
	local LastResponse
	
	--Send the remaining requests if needed.
	if FirstResponse.Status == "Incomplete" then
		local RequestId = FirstResponse.Id
		for i = 2,#self.SplitBody do
			--Send the request.
			local NewResponse
			if PARALLIZE_SEQUENTIAL_REQUESTS then
				spawn(function()
					NewResponse = self:__SendRequest(i,RequestId)
				end)
			else
				NewResponse = self:__SendRequest(i,RequestId)
			end
			
			spawn(function()
				--Wait for the new response.
				while not NewResponse do wait() end
					
				--Set the last request if it wasn't incomplete.
				if NewResponse.Status == "Success" then
					LastResponse = NewResponse
				end
			end)
		end
	elseif FirstResponse.Status == "Success" then
		LastResponse = FirstResponse
	end
	
	--Wait for a final response.
	while not LastResponse do wait() end
	
	--Return the response.
	return LastResponse
end

--[[
Builds the response and returns the 
--]]
function MultiHttpRequest:__GetCompleteResponse(FirstResponse)
	--Create the complete response.
	local ResponseId = FirstResponse.Id
	local TotalResponses = FirstResponse.MaxPackets 
	local FinalResponse = MultiHttpResponse.new(TotalResponses)
	FinalResponse:AddPartialResponse(FirstResponse)
	
	--Get the remaining responses.
	if TotalResponses > 0 then
		local ResponsesLeft = TotalResponses - 1
	
		--Send the requests.
		for i = 2,TotalResponses do
			if PARALLIZE_SEQUENTIAL_REQUESTS then
				spawn(function()
					FinalResponse:AddPartialResponse(self:__GetResponse(i,ResponseId))
					ResponsesLeft = ResponsesLeft - 1
				end)
			else
				FinalResponse:AddPartialResponse(self:__GetResponse(i,ResponseId))
				ResponsesLeft = ResponsesLeft - 1
			end
		end
		
		--Wait for the responses to be completed.
		while ResponsesLeft > 0 do wait() end
	end
	
	--Return the final response.
	return FinalResponse
end

--[[
Sends the request and returns a response.
--]]
function MultiHttpRequest:SendRequest()
	--Get the first response.
	local FirstResponse = self:__GetFirstResponse()
	
	--Return the final response.
	return self:__GetCompleteResponse(FirstResponse)
end



return MultiHttpRequest