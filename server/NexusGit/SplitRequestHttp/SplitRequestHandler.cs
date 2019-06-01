/*
 * TheNexusAvenger
 * 
 * Extends the RequestHandler class for SplitClientRequestHandler since
 * any non-GET request handler also needs to register the GET request
 * handler for getting complete parts of packets.
 */

using NexusGit.Http;
using NexusGit.Http.Request;

namespace NexusGit.SplitRequestHttp
{
    /*
     * Class for handling split client request handlers.
     */
    class SplitRequestHandler : RequestHandler
    {
        /*
         * Registers a request handler.
         */
        public new void RegisterHandler(string requestType,string urlBase,IClientRequestHandler clientRequestHandler)
        {
            // Register the base request.
            base.RegisterHandler(requestType,urlBase,clientRequestHandler);

            // Register the GET request.
            if (requestType != "GET")
            {
                base.RegisterHandler("GET",urlBase,clientRequestHandler);
            }
        }
    }
}