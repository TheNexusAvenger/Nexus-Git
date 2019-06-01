/*
 * TheNexusAvenger
 * 
 * Splits up responses.
 */

using NexusGit.Http.Request;
using System.Collections.Generic;

namespace NexusGit.SplitRequestHttp
{
    /*
     * Class for spliting up responses.
     */
    public class PartialResponse : Response
    {
        public const int DEFAULT_MAX_RESPONSE_LENGTH = 64000;

        private List<Response> Responses;
        private bool[] ResponsesRead;
        private int ResponsesToRead;

        /*
         * Creates a partial response.
         */
        public PartialResponse(List<Response> responses,string fullMessage) : base(responses[0].GetStatus(), responses[0].GetMimeType(), fullMessage)
        {
            this.Responses = responses;
            this.ResponsesToRead = responses.Count;

            // Set up tbe bools.
            this.ResponsesRead = new bool[responses.Count];
            for (int i = 0; i < responses.Count; i++)
            {
                this.ResponsesRead[i] = false;
            }
        }

        /*
         * Creates a PartialResponse from a request and a max response size.
         */
        public static PartialResponse SplitResponse(Response response,int maxLength)
        {
            // Get the base response data.
            int status = response.GetStatus();
            string mimeType = response.GetMimeType();
            string completeResponseData = response.GetResponseData();

            // Split the responses.
            List<Response> splitResponses = new List<Response>();
            string remainingResponseData = completeResponseData;
            while (remainingResponseData.Length != 0)
            {
                if (remainingResponseData.Length <= maxLength)
                {
                    splitResponses.Add(new Response(status,mimeType,remainingResponseData));
                    remainingResponseData = "";
                } else
                {
                    splitResponses.Add(new Response(status,mimeType,remainingResponseData.Substring(0,maxLength)));
                    remainingResponseData = remainingResponseData.Substring(maxLength);
                }
            }

            // Return the partial response.
            return new PartialResponse(splitResponses, completeResponseData);
        }

        /*
         * Creates a PartialResponse from a request and a max response size.
         */
        public static PartialResponse SplitResponse(Response response)
        {
            return SplitResponse(response,DEFAULT_MAX_RESPONSE_LENGTH);
        }

        /*
         * Returns the amount of responses.
         */
        public int GetNumberOfResponses()
        {
            return this.Responses.Count;
        }

        /*
         * Returns the response for the id.
         */
        public Response GetResponseFromId(int index)
        {
            // Mark the response as read.
            if (this.ResponsesRead[index] == false)
            {
                this.ResponsesRead[index] = true;
                this.ResponsesToRead += -1;
            }

            // Return the response.
            return this.Responses[index];
        }

        /*
         * Returns if all of the responses have been sent.
         */
        public bool AllResponsesSent()
        {
            return (this.ResponsesToRead == 0);
        }
    }
}