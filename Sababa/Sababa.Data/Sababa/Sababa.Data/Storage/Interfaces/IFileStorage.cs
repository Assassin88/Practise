using System;
using System.IO;
using System.Threading.Tasks;

namespace Sababa.Data.Storage.Interfaces
{
    public interface IFileStorage
    {
        /// <summary>
        /// Writes the passed-in stream to the file asynchronously
        /// </summary>
        /// <param name="stream">Transmitted stream</param>
        /// <param name="fileName">File name</param>
        /// <param name="lifeTime">Life time of file</param>
        /// <returns>Returns a task generalized by type bool</returns>
        Task<bool> SaveAsync(Stream stream, string fileName, DateTime lifeTime);

        /// <summary>
        /// Writes the passed-in stream to the file
        /// </summary>
        /// <param name="stream">Transmitted stream</param>
        /// <param name="fileName">File name</param>
        /// <param name="lifeTime">Life time of file</param>
        /// <returns>Returns true if file saved else false</returns>
        bool Save(Stream stream, string fileName, DateTime lifeTime);

        /// <summary>
        /// Writes the byte array to the file asynchronously
        /// </summary>
        /// <param name="bytes">Array byte</param>
        /// <param name="fileName">File name</param>
        /// <param name="lifeTime">Life time of file</param>
        /// <returns>Returns a task generalized by type bool</returns>
        Task<bool> SaveAsync(byte[] bytes, string fileName, DateTime lifeTime);

        /// <summary>
        /// Writes the byte array to the file
        /// </summary>
        /// <param name="bytes">Array byte</param>
        /// <param name="fileName">File name</param>
        /// <param name="lifeTime">Life time of file</param>
        /// <returns>Returns true if file saved else false</returns>
        bool Save(byte[] bytes, string fileName, DateTime lifeTime);

        /// <summary>
        /// Reads all bytes from a file asynchronously
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <returns>Returns a task generalized by type array byte</returns>
        Task<byte[]> ReadAllBytesAsync(string fileName);

        /// <summary>
        /// Reads all bytes from a file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>Returns array byte</returns>
        byte[] ReadAllBytes(string fileName);

        /// <summary>
        /// Returns stream from file
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <returns>Returns stream</returns>
        Stream GetStream(string fileName);

        /// <summary>
        /// Deletes file by name
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <returns>Returns true if file deleted else false</returns>
        bool Remove(string fileName);

        /// <summary>
        /// Clear file storage
        /// </summary>
        void Clear();

        /// <summary>
        /// Returns counts elements in file storage
        /// </summary>
        /// <returns>Returns counts elements</returns>
        int Count();
    }
}