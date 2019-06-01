--[[
TheNexusAvenger

Stores responses from the server.
--]]

local HttpService = game:GetService("HttpService")

local Root = script.Parent.Parent
local HttpResponse = require(script.Parent:WaitForChild("HttpResponse"))

local MultiHttpResponse = HttpResponse:Extend()
MultiHttpResponse:SetClassName("MultiHttpResponse")



--[[
Creates an Http response.
--]]
function MultiHttpResponse:__new(CompleteSize)
	self.PartialResponses = {}
	self.CompleteSize = CompleteSize
end

--[[
Returns if the response is complete.
--]]
function MultiHttpResponse:IsComplete()
	--Return false if a response is missing.
	for i = 1,self.CompleteSize do
		if self.PartialResponses[i] == nil then
			return false
		end
	end
	
	--Return true (no nil responses.
	return true
end

--[[
Returns the response. Throws an error if the response
is incomplete.
--]]
function MultiHttpResponse:GetResponse()
	--Throw an error if the response is incomplete.
	if not self:IsComplete() then
		error("Response is incomplete.")
	end

	--Create and return the response.
	local Response = ""
	for _,PartialResponse in pairs(self.PartialResponses) do
		Response = Response..PartialResponse.Packet
	end
	
	return Response
end

--[[
Adds a partial response.
--]]
function MultiHttpResponse:AddPartialResponse(Response)
	self.PartialResponses[Response.CurrentPacket + 1] = Response
end



return MultiHttpResponse