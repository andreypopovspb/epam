using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using Epam.FundManager.Common.Extensions;

namespace Epam.FundManager.Common
{
    public static class Utility
    {
        #region Hash

        public static byte[] Hash256(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException("assembly");

            var hashEvidence = new Hash(assembly);
            byte[] hashCode = hashEvidence.SHA256;
            return hashCode;
        }

        public static byte[] Hash1(string data, Encoding encoding = null)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            encoding = encoding ?? Encoding.UTF8;
            byte[] rawData = encoding.GetBytes(data);
            return Hash1(rawData);
        }

        public static byte[] Hash256(string data, Encoding encoding = null)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            encoding = encoding ?? Encoding.UTF8;
            byte[] rawData = encoding.GetBytes(data);
            return Hash256(rawData);
        }

        public static byte[] Hash1(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            using (SHA1 sha = new SHA1Managed())
            {
                return sha.ComputeHash(data);
            }
        }

        public static byte[] Hash256(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            using (SHA256 sha = new SHA256Managed())
            {
                return sha.ComputeHash(data);
            }
        }

        #endregion

        #region Copy

        public static byte[] Copy(byte[] from)
        {
            if (from == null)
            {
                return null;
            }
            var newAr = new byte[from.Length];
            from.CopyTo(newAr, 0);
            return newAr;
        }

        public static string[] Copy(string[] from)
        {
            if (from == null)
            {
                return null;
            }
            var newAr = new string[from.Length];
            from.CopyTo(newAr, 0);
            return newAr;
        }

        #endregion

        #region Checksum

        public static string GetChecksum(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException("assembly");

            byte[] hash = Hash256(assembly);
            return ToHexString(hash);
        }

        public static string GetChecksum(string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            byte[] hash = Hash256(value);
            return ToHexString(hash);
        }

        public static string GetChecksum(byte[] value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            byte[] hash = Hash256(value);
            return ToHexString(hash);
        }

        #endregion

        #region Compress

        public static byte[] CompressBytes(byte[] data)
        {
            data = data ?? new byte[0];
            using (var compressedStream = new MemoryStream())
            {
                using (var deflateStream = new DeflateStream(compressedStream, CompressionMode.Compress))
                {
                    deflateStream.Write(data, 0, data.Length);
                }
                return compressedStream.ToArray();
            }
        }

        public static byte[] DecompressBytes(byte[] data)
        {
            data = data ?? new byte[0];
            var compressedStream = new MemoryStream(data);
            using (var zipStream = new DeflateStream(compressedStream, CompressionMode.Decompress))
            {
                var resultStream = new MemoryStream();
                var buffer = new byte[4096];
                int read;
                while ((read = zipStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    resultStream.Write(buffer, 0, read);
                }
                return resultStream.ToArray();
            }
        }

        #endregion

        public static byte[] ToByteArray(string data)
        {
            if (string.IsNullOrEmpty(data))
                throw new ArgumentNullException("data");

            if (data.StartsWith("0x"))
            {
                data = data.Substring(2);
            }

            int numberChars = data.Length;
            var bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(data.Substring(i, 2), 16);
            }
            return bytes;
        }

        public static byte[] ToByteArray(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            if (stream is MemoryStream)
            {
                return ((MemoryStream)stream).ToArray();
            }

            using (var sr = new StreamReader(stream))
            {
                var result = new List<byte>();
                while (!sr.EndOfStream)
                {
                    int byteValue = sr.Read();
                    if (byteValue != -1)
                    {
                        result.Add((byte)byteValue);
                    }
                }
                return result.ToArray();
            }
        }

        public static string ExtractPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
                throw new ArgumentNullException("propertyExpression");

            var memberExpression = propertyExpression.Body as MemberExpression;
            if (memberExpression == null)
                throw new ArgumentOutOfRangeException("propertyExpression", "The expression is not a member access expression.");

            var property = memberExpression.Member as PropertyInfo;
            if (property == null)
                throw new ArgumentOutOfRangeException("propertyExpression", "The member access expression does not access a property.");

            var getMethod = property.GetGetMethod(true);
            if (getMethod == null)
                throw new ArgumentOutOfRangeException("propertyExpression", "The referenced property does not have a get method.");

            if (getMethod.IsStatic)
                throw new ArgumentOutOfRangeException("propertyExpression", "The referenced property is a static property.");

            return memberExpression.Member.Name;
        }

        public static string ToTraceString(Exception ex, bool addNewLine = true)
        {
            var cr = (addNewLine) ? Environment.NewLine : string.Empty;
            if (ex == null)
            {
                return string.Empty;
            }
            var message = string.Format("Message: '{0}' Type: '{1}'{2}", ex.Message, ex.GetType(), cr);
            message += string.Format(" StackTrace: '{0}'{1}", ex.StackTrace, cr);
            if (ex.InnerException != null)
            {
                message += string.Format(" InnerExceptionMessage:{0}{1}", cr, ex.InnerException.ToTraceString());
            }
            message = message.Trim() + cr;
            return message;
        }

        public static string ToHexString(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            int length = data.Length;
            var hex = new StringBuilder(length * 2);
            for (int i = 0; i < length; i++)
            {
                hex.AppendFormat("{0:X2}", data[i]);
            }
            return hex.ToString();
        }
    }
}