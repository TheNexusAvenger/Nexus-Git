/*
 * TheNexusAvenger
 * 
 * Stores the request handlers for the server.
 */

using NexusGit.Http.Request;
using System.Collections.Generic;

namespace NexusGit.Http
{
    public class RequestHandler
    {
        private Dictionary<string,Dictionary<string,IClientRequestHandler>> Handlers;

        /*
         * Creates a request handler.
         */
        public RequestHandler()
        {
            this.Handlers = new Dictionary<string,Dictionary<string,IClientRequestHandler>>();
        }

        /*
         * Returns the request handler for the given request.
         */
        public IClientRequestHandler GetRequestHandler(HttpRequest request)
        {
            // Get the type and base url.
            string requestType = request.GetRequestType().ToLower();
            string baseURL = request.GetURL().GetBaseURL().ToLower();

            // Return if no handler for the type exists.
            if (!this.Handlers.ContainsKey(requestType))
            {
                return null;
            }

            // Return if no handler for the request exists.
            Dictionary<string,IClientRequestHandler> typeRequests = this.Handlers[requestType];
            if (!typeRequests.ContainsKey(baseURL))
            {
                return null;
            }

            // Return the request handler.
            return typeRequests[baseURL];
        }

        /*
         * Returns the response for a request.
         */
        public Response GetResponse(HttpRequest request)
        {
            // Get the handler.
            IClientRequestHandler handler = this.GetRequestHandler(request);

            // If the handler exists, return the response.
            if (handler != null)
            {
                return handler.GetResponseData(request);
            }

            // Return an invalid response error.
            return Response.CreateBadRequestResponse("Invalid request");
        }

        /*
         * Registers a request handler.
         */
        public void RegisterHandler(string requestType,string urlBase,IClientRequestHandler clientRequestHandler)
        {
            requestType = requestType.ToLower();
            urlBase = urlBase.ToLower();

            // Add the request type handler if it doesn't exist.
            if (!this.Handlers.ContainsKey(requestType))
            {
                this.Handlers.Add(requestType,new Dictionary<string,IClientRequestHandler>());
            }

            //Add the handler.
            this.Handlers[requestType].Add(urlBase,clientRequestHandler);
        }
    }
}