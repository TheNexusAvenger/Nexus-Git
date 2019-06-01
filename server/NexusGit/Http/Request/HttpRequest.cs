/*
 * TheNexusAvenger
 * 
 * Stores information about requests.
 */

namespace NexusGit.Http.Request
{
    /*
     * Data class representing a request.
     */
    public class HttpRequest
    {
        private string Type;
        private URL Target;
        private string Host;
        private string Body;

        /*
         * Creates a request object.
         */
        public HttpRequest(string type,URL target,string host,string body)
        {
            this.Type = type;
            this.Target = target;
            this.Host = host;
            this.Body = body;
        }

        /*
         * Creates a request object.
         */
        public HttpRequest(string type,string target,string host,string body) : this(type,URL.FromString(target),host,body)
        {
            
        }

        /*
         * Returns the request type.
         */
        public string GetRequestType()
        {
            return this.Type;
        }

        /*
         * Returns the request URL.
         */
        public URL GetURL()
        {
            return this.Target;
        }

        /*
         * Returns the request host.
         */
        public string GetHost()
        {
            return this.Host;
        }

        /*
         * Returns the request body.
         */
        public string GetBody()
        {
            return this.Body;
        }
    }
}