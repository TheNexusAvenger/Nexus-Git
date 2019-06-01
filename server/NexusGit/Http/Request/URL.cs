/*
 * TheNexusAvenger
 * 
 * Parses URLs into base URLs and parameters.
 */

using System;
using System.Collections.Generic;

namespace NexusGit.Http.Request
{
    /*
     * Data class for URLs with parameters.
     */
    public class URL
    {
        private string BaseURL;
        private Dictionary<string, string> Parameters;

        /*
         * Creates a URL object.
         */
        private URL(string baseURL,Dictionary<string,string> parameters)
        {
            this.BaseURL = baseURL;
            this.Parameters = parameters;
        }

        /*
         * Creates a URL object.
         */
        private URL(string baseURL) : this(baseURL,new Dictionary<string, string>())
        {

        }

        /*
         * Returns the base URL.
         */
        public string GetBaseURL()
        {
            return this.BaseURL;
        }

        /*
         * Returns if a parameter exists.
         */
        public bool ParameterExists(string parameter)
        {
            return this.Parameters.ContainsKey(parameter.ToLower());
        }

        /*
         * Returns the value for a parameter.
         */
        public string GetParameter(string parameter)
        {
            parameter = parameter.ToLower();

            // Return the value if it exists.
            if (this.ParameterExists(parameter))
            {
                return this.Parameters[parameter];
            }

            // Return null (doesn't exist).
            return null;
        }

        /*
         * Parses the parameters from a string.
         */
        public static Dictionary<string,string> ParseParameters(string parametersString)
        {
            // Create the parameters dictionary.
            Dictionary<string,string> parameters = new Dictionary<string,string>();

            // Parse the parameters.
            foreach (string parameter in parametersString.Split('&'))
            {
                string[] parameterData = parameter.Split('=');

                // Add the parameter.
                if (parameterData.Length >= 2)
                {
                    parameters.Add(parameterData[0].ToLower(),Uri.UnescapeDataString(parameterData[1]));
                } else if (parameterData.Length == 1)
                {
                    parameters.Add(parameterData[0].ToLower(),null);
                }
            }

            // Return the parameters.
            return parameters;
        }

        /*
         * Removes the slash from the beginning and end if they exist.
         */
        public static string RemoveSlashes(string baseString)
        {
            // Remove the beginning slash if it exists.
            if (baseString.Length > 0 && (baseString[0] == '/' || baseString[0] == '\\'))
            {
                baseString = baseString.Substring(1);
            }

            // Remove the ending slash if it exists.
            if (baseString.Length > 0 && (baseString[baseString.Length - 1] == '/' || baseString[baseString.Length - 1] == '\\'))
            {
                baseString = baseString.Remove(baseString.Length - 1);
            }

            // Return the base string.
            return baseString;
        }

        /*
         * Parses a URL from a string.
         */
        public static URL FromString(string url)
        {
            // Parse the URL.
            string[] splitURL = url.Split('?');

            // Return the URL object.
            if (splitURL.Length < 2)
            {
                return new URL(RemoveSlashes(url));
            }
            else
            {
                return new URL(RemoveSlashes(splitURL[0]), ParseParameters(splitURL[1]));
            }
        }
    }
}