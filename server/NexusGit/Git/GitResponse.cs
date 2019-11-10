/*
 * TheNexusAvenger
 * 
 * Stores responses for Git actions.
 */

using System.Collections.Generic;
using Newtonsoft.Json;

namespace NexusGit.Git
{
    /*
     * Class representing a Git response.
     */
    public class GitResponse : List<string>
    {
        /*
         * Creates a single line response.
         */
        public static GitResponse CreateSingleLineResponse(string line)
        {
            var response = new GitResponse();
            response.AddResponse(line);
            return response;
        }

        /*
         * Adds a response line.
         */
        public void AddResponse(string line)
        {
            this.Add(line);
        }

        /*
         * Serializes the response to JSON.
         */
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this,Formatting.None);
        }
    }
}