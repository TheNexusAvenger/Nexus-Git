/*
 * TheNexusAvenger
 * 
 * Store partial requests to be processed later.
 */

using NexusGit.Http.Request;
using System.Text;

namespace NexusGit.SplitRequestHttp
{
    /*
     * Class representing a partial request.
     */
    public class PartialHttpRequest : HttpRequest
    {
        private string[] PartialRequests;
        private int PacketsLeft;
        private bool RequestComplete;

        /*
         * Creates a partial request object.
         */
        public PartialHttpRequest(string type,URL target,string host,int bufferSize) : base(type,target,host,"")
        {
            this.PartialRequests = new string[bufferSize];
            this.PacketsLeft = bufferSize;
            this.RequestComplete = false;
        }

        /*
         * Returns if the request is complete.
         */
        public bool IsComplete()
        {
            return this.RequestComplete;
        }

        /*
         * Returns the request body. Returns null if the request is incomplete.
         */
        public new string GetBody()
        {
            // Return null if the packet is incomplete.
            if (!RequestComplete)
            {
                return null;
            }

            // Concat the body.
            var requestBody = new StringBuilder();
            foreach (var packet in this.PartialRequests)
            {
                requestBody.Append(packet);
            }

            // Return the body.
            return requestBody.ToString();
        }

        /*
         * Adds a partial packet.
         */
        public void AddPartialPacket(int id,string packet)
        {
            // Decrease the count if the packet is currently null.
            if (this.PartialRequests[id] == null)
            {
                this.PacketsLeft += -1;
                this.RequestComplete = (this.PacketsLeft == 0);
            }

            // Set the packet.
            this.PartialRequests[id] = packet;
        }

        /*
         * Creates a request object from the partial http request object.
         */
        public HttpRequest ToSingleRequest()
        {
            return new HttpRequest(this.GetRequestType(),this.GetURL(),this.GetHost(),this.GetBody());
        }
    }
}