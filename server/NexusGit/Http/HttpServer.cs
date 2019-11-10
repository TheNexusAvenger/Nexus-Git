/*
 * TheNexusAvenger
 * 
 * Class that handles accepts client connections.
 */
 
using System.Net;

namespace NexusGit.Http
{
    /*
     * Class for the server.
     */
    public class HttpServer
    {
        private bool Running;
        private HttpListener Listener;
        private RequestHandler Handlers;

        /*
         * Creates a server object.
         */
        public HttpServer(int port,RequestHandler requestHandler)
        {
            this.Running = false;
            this.Handlers = requestHandler;

            // Set up the HTTP handler.
            this.Listener = new HttpListener();
            this.Listener.Prefixes.Add("http://localhost:" + port + "/");
        }

        /*
         * Handles a new request.
         */
        public void HandleRequest(HttpListenerContext httpRequestContext)
        {
            // Create and start the handler.
            var handler = new ContextHandler(httpRequestContext,this.Handlers);
            handler.StartHandling();
        }

        /*
         * Starts the server.
         */
        public void Start()
        {
            // Start the listener.
            this.Running = true;
            this.Listener.Start();

            // Run a loop to accept client connections.
            while (Running)
            {
                var httpRequestContext = this.Listener.GetContext();
                this.HandleRequest(httpRequestContext);
            }
        }

        /*
         * Stops the server.
         */
        public void Stop()
        {
            this.Running = false;
        }
    }
}