/*
 * TheNexusAvenger
 * 
 * Stores objects until they are no longer needed with an id.
 */

using System.Collections.Generic;

namespace NexusGit.SplitRequestHttp
{
    /*
     * Class for storing temporary objects.
     */
    public class TemporaryStorage<T>
    {
        private List<T> StoredObjects;

        /*
         * Creates a temporary storage object.
         */
        public TemporaryStorage()
        {
            this.StoredObjects = new List<T>();
        }

        /*
         * Returns the next available index.
         */
        private int GetNextIndex()
        {
            // Iterate through the list and find a null object.
            for (int i = 0; i < this.StoredObjects.Count; i++)
            {
                if (this.StoredObjects[i] == null)
                {
                    return i;
                }
            }

            // Return the length (no holes).
            return this.StoredObjects.Count;
        }

        /*
         * Stores an object and returns the id to index it.
         */
        public int Store(T objectToStore)
        {
            // Get the next id and store the object.
            int nextIndex = this.GetNextIndex();
            if (nextIndex > this.StoredObjects.Count - 1)
            {
                this.StoredObjects.Add(objectToStore);
            } else
            {
                this.StoredObjects[nextIndex] = objectToStore;
            }

            // Return the object.
            return nextIndex;
        }

        /*
         * Returns the object for a given id.
         */
        public T Get(int index)
        {
            return this.StoredObjects[index];
        }

        /*
         * Removes the object at the given id.
         */
        public void Remove(int index)
        {
            this.StoredObjects[index] = default(T);
        }
    }
}