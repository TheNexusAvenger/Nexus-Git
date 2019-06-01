/*
 * TheNexusAvenger
 * 
 * Specific handlers for client requests.
 */

namespace NexusGit.Http.Request
{
    /*
     * Interface for a client request handler.
     */
    public interface IClientRequestHandler
    {
        /*
         * Returns a response for a given request.
         */
        Response GetResponseData(HttpRequest request);
    }
}