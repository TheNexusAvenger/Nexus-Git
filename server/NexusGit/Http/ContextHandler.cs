/*
 * TheNexusAvenger
 * 
 * Handles http requests in separate threads.
 */

using NexusGit.Http.Request;
using System.IO;
using System.Net;
using System.Threading;

namespace NexusGit.Http
{
    /*
     * Class representing an http context handler.
     */
    public class ContextHandler
    {
        private HttpListenerContext HttpRequestContext;
        private RequestHandler Handlers;
        private Thread HandlerThread;

        /*
         * Creates the client handler.
         */
        public ContextHandler(HttpListenerContext httpRequestContext,RequestHandler requestHandler) {
            this.HttpRequestContext = httpRequestContext;
            this.Handlers = requestHandler;

            // Create the thread.
            this.HandlerThread = new Thread(new ThreadStart(this.Handle));
        }

        /*
         * Returns the client request.
         */
        private HttpRequest GetClientRequest()
        {
            // Get the base request information.
            var request = this.HttpRequestContext.Request;
            var requestType = request.HttpMethod;
            var url = request.RawUrl;
            var host = request.UserHostName;

            // Get the request body.
            var bodyReader = new StreamReader(request.InputStream,request.ContentEncoding);
            var body = bodyReader.ReadToEnd();
            bodyReader.Close();

            // Return the request.
            return new HttpRequest(requestType,url,host,body);
        }

        /*
         * Handles the client.
         */
        private void Handle()
        {
            // Get the request.
            var request = this.GetClientRequest();
            
            // Get and send the response.
            var response = this.Handlers.GetResponse(request);
            response.SendResponse(this.HttpRequestContext);
        }

        /*
         * Starts the thread for handling the client.
         */
        public void StartHandling()
        {
            this.HandlerThread.Start();
        }
    }
}