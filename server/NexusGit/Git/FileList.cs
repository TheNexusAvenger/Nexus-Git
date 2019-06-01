/*
 * TheNexusAvenger
 *
 * Stores lists of files.
 */

using System.Collections.Generic;
using System.Text;

namespace NexusGit.Git
{
    /*
     * Class representing a list of files.
     */
    public class FileList : List<string>
    {
        /*
         * Returns the list of files as a string with proper escaping.
         */
        public string GetFilesAsString()
        {
            // Create the string builder.
            StringBuilder fileList = new StringBuilder();
            
            // Add the files.
            foreach (string file in this)
            {
                fileList.Append("\"" + file.Replace("\"","\\\"") + "\" ");
            }
            
            // Return the list.
            return fileList.ToString();
        }
    }
}