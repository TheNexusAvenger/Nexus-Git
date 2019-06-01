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
            HttpListenerRequest request = this.HttpRequestContext.Request;
            string requestType = request.HttpMethod;
            string url = request.RawUrl;
            string host = request.UserHostName;

            // Get the request body.
            StreamReader bodyReader = new StreamReader(request.InputStream,request.ContentEncoding);
            string body = bodyReader.ReadToEnd();
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
            HttpRequest request = this.GetClientRequest();
            
            // Get and send the response.
            Response response = this.Handlers.GetResponse(request);
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