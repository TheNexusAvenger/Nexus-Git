--[[
TheNexusAvenger

Stores responses from the server.
--]]

local HTTP_RESPONSE_STATUSES = {
	success = "Success",
	incomplete = "Incomplete",
}



local HttpService = game:GetService("HttpService")

local Root = script.Parent.Parent
local NexusInstance = require(Root:WaitForChild("NexusInstance"):WaitForChild("NexusInstance"))

local HttpResponse = NexusInstance:Extend()
HttpResponse:SetClassName("HttpResponse")



--[[
Creates an Http response.
--]]
function HttpResponse:__new(Response)
	self.Response = Response
end

--[[
Returns the response.
--]]
function HttpResponse:GetResponse()
	return self.Response
end

--[[
Creates an Http response from a JSON string.
--]]
function HttpResponse.FromJSON(JSONResponse)
	--Create a new response.
	local Response = HttpResponse.new(JSONResponse)
	
	--Parse the response.
	local ParsedResponse = HttpService:JSONDecode(JSONResponse)
	Response.Status = HTTP_RESPONSE_STATUSES[ParsedResponse.status]
	Response.Id = ParsedResponse.id
	Response.CurrentPacket = ParsedResponse.currentPacket
	Response.MaxPackets = ParsedResponse.maxPackets
	Response.Packet = ParsedResponse.packet
	
	--Return the response.
	return Response
end



return HttpResponse