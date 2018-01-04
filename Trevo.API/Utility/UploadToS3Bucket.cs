using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Amazon;
using Amazon.IdentityManagement.Model;
using Amazon.Runtime;
using Amazon.S3.Transfer;
using Microsoft.Ajax.Utilities;

namespace StatMedClinic.API.Utility
{
    /// <summary>
    /// Manage Amazon S3 related services. 
    /// </summary>
    public class UploadToS3Bucket
    {
        /// <summary>
        /// Default
        /// </summary>
        public UploadToS3Bucket()
        {
            CreateClient();
        }

        /// <summary>
        /// AmazonWebService AccessKey
        /// </summary>
        public static string _awsAccessKey { get; } = ConfigurationManager.AppSettings["AWSAccessKey"];

        /// <summary>
        /// AmazonWebService Secret AccessKey 
        /// </summary>
        private static string _awsSecretKey { get; } = ConfigurationManager.AppSettings["AWSSecretKey"];

        /// <summary>
        /// AmazonS3 fixed Bucket Name
        /// </summary>
        private static string _bucketName { get; } = ConfigurationManager.AppSettings["Bucketname"];

        /// <summary>
        /// Communication client for Amazon S3
        /// </summary>
        public IAmazonS3 AmazonS3Client { get; set; }


        public static bool SaveImageToS3(string fileName, string fileType, string base64String)
        {
            bool result = false;
            string filename = "";
            try
            {
                IAmazonS3 client;
                byte[] bytes = Convert.FromBase64String(base64String);

                using (client = Amazon.AWSClientFactory.CreateAmazonS3Client(_awsAccessKey, _awsSecretKey, Amazon.RegionEndpoint.USEast1))
                {
                    var request = new PutObjectRequest
                    {
                        BucketName = _bucketName,
                        CannedACL = S3CannedACL.PublicRead,
                        Key = string.Format("{0}", fileName + fileType)
                    };
                    using (var ms = new MemoryStream(bytes))
                    {
                        request.InputStream = ms;
                        client.PutObject(request);
                    }
                }
                result = true;
                return result;
            }
            catch (Exception ex)
            {
                return result;

            }
        }

        /// <summary>
        /// This method is used to upload a file using input-stream(request)
        /// </summary>
        /// <param name="directoryName"></param>
        /// <param name="keyFileName"></param>
        /// <param name="contentType"></param>
        /// <param name="fileStream"></param>
        /// <returns>bool</returns>
        public static bool SaveImageToS3(
            string directoryName, 
            string keyFileName, 
            string contentType, 
            Stream fileStream)
        {
            try
            {
                IAmazonS3 client;
                using (
                    client =
                        Amazon.AWSClientFactory.CreateAmazonS3Client(_awsAccessKey, _awsSecretKey,
                            Amazon.RegionEndpoint.USEast1))
                {
                    TransferUtility utility = new TransferUtility(client);
                    TransferUtilityUploadRequest request = new TransferUtilityUploadRequest
                    {
                        BucketName = String.IsNullOrEmpty(directoryName)
                            ? _bucketName
                            : _bucketName + @"/" + directoryName,
                        Key = keyFileName,
                        InputStream = fileStream,
                        ContentType = contentType
                    };
                    var op =utility.UploadAsync(request);
                    return true;
                }
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// This method signify the files extention and return the contentType of 
        /// the file.
        /// </summary>
        /// <param name="fileType"></param>
        /// <returns>bool</returns>
        public static string GetImageContentType(string fileType)
        {
            switch (fileType.ToLower(CultureInfo.InvariantCulture))
            {
                case @"jpeg":
                case @"jpg":
                    return @"image/jpeg";
                case @"png":
                    return @"image/png";
                case @"gif":
                    return @"image/gif";
                default:
                    return @"image/generic";
            }
        }

        #region Protected Methods

        /// <summary>
        /// Create a new client to connect with Amazon
        /// </summary>
        /// <returns>bool</returns>
        protected bool CreateClient()
        {
            try
            {
                AWSCredentials awsCredentials = new BasicAWSCredentials(_awsAccessKey, _awsSecretKey);
                AmazonS3Client = new AmazonS3Client(awsCredentials, RegionEndpoint.USWest2);
                return true;
            }
            catch (Exception)
            {
                // write exception in log
                return false;
            }
        }
        #endregion

        #region Public Methods

        #region Bucket 

        /// <summary>
        /// Get informations of all bucket
        /// </summary>
        /// <returns>bool</returns>
        public IEnumerable<S3Bucket> GetAllBucket()
        {
            if (AmazonS3Client == null)
            {
                if (!CreateClient()) return new List<S3Bucket>();

                if (AmazonS3Client != null) return AmazonS3Client.ListBuckets().Buckets;
            }
            else
            {
                return AmazonS3Client.ListBuckets().Buckets;
            }
            return null;
        }

        /// <summary>
        /// Check bucket existance
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns>bool</returns>
        public bool BucketExists(string bucketName)
        {
            if (AmazonS3Client == null)
            {
                CreateClient();
            }
            if (!CreateClient()) return false;

            if (!GetAllBucket().Any()) return false;

            var result = GetAllBucket()
                .Any(bkt => String.Equals(bkt.BucketName, bucketName, StringComparison.CurrentCultureIgnoreCase));
            return result;
        }

        /// <summary>
        /// Create a new Bucket in Amazon S3 Storage
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns>bool</returns>
        public bool CreateBucket(string bucketName)
        {
            if (!CreateClient()) return false;
            if (BucketExists(bucketName)) return false;

            PutBucketRequest putBucketRequest = new PutBucketRequest
            {
                BucketName = bucketName,
                UseClientRegion = false,
                BucketRegion = S3Region.USW2,
                BucketRegionName = RegionEndpoint.USWest2.SystemName
            };

            Task<PutBucketResponse> bucketResponse = AmazonS3Client.PutBucketAsync(putBucketRequest);
            return BucketExists(bucketName);
        }

        /// <summary>
        /// Delete a particular bucket by Bucket Name
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        public bool DeleteBucket(string bucketName)
        {
            if (!CreateClient()) return false;
            if (!BucketExists(bucketName)) return false;

            DeleteBucketRequest deleteBucketRequest = new DeleteBucketRequest
            {
                BucketName = bucketName,
                UseClientRegion = false,
                BucketRegion = S3Region.USW2
            };

            Task<DeleteBucketResponse> deleteBucketResponse = AmazonS3Client.DeleteBucketAsync(deleteBucketRequest);
            return !BucketExists(bucketName);
        }
        #endregion

        #region Files and Folders

        /// <summary>
        /// Create a New folder or Directory in a particular bucket
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns>bool</returns>
        public bool CreateFolderInBucket(string folderName)
        {
            if (!CreateClient()) return false;
            PutObjectRequest putObjectRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = folderName,
                ContentBody = String.Empty
            };
            try
            {

                Task<PutObjectResponse> bucketResponse = AmazonS3Client.PutObjectAsync(putObjectRequest);
                return true;
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Get files List of a Particular Bucket
        /// </summary>
        /// <returns>List of S3Object</returns>
        public IList<S3Object> GetFilesinBucket()
        {
            if (!CreateClient()) return new List<S3Object>();
            if (!BucketExists(_bucketName)) return new List<S3Object>();

            ListObjectsRequest request = new ListObjectsRequest {BucketName = _bucketName };
            try
            {
                ListObjectsResponse response = AmazonS3Client.ListObjects(request);
                return response.S3Objects;
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                return new List<S3Object>();
            }
        }

        /// <summary>
        /// Upload any types of file in a particualar bucket by Bucket name
        /// </summary>
        /// <param name="directoryName"></param>
        /// <param name="fileStream"></param>
        /// <param name="keyFileName"></param>
        /// <param name="contentType"></param>
        /// <returns>bool</returns>
        public bool UploadSingleFileInBucket(string directoryName, Stream fileStream,
            string keyFileName, string contentType)
        {
            try
            {
                if (!BucketExists(_bucketName)) return false;

                TransferUtility utility = new TransferUtility(AmazonS3Client);
                TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();

                if (string.IsNullOrEmpty(directoryName))
                {
                    request.BucketName = _bucketName;
                }
                else
                {
                    request.BucketName = _bucketName + @"/" + directoryName;
                }
                request.Key = keyFileName;
                request.InputStream = fileStream;
                request.ContentType = contentType;
                utility.Upload(request);
                return true;
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public GetObjectResponse PrepareFileRespose(string keyName)
        {
            if (!CreateClient()) return null;
            if (!BucketExists(_bucketName)) return null;
            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = keyName
            };

            try
            {
                GetObjectResponse response = AmazonS3Client.GetObject(request);
                return response;
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Delete a particular file from a particular bucket
        /// </summary>
        /// <param name="keyFileName"></param>
        /// <returns>bool</returns>
        public bool DeleteSingleFileInBucket(string keyFileName)
        {
            if (!CreateClient()) return false;
            if (!BucketExists(_bucketName)) return false;
            DeleteObjectRequest deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = keyFileName
            };
            try
            {
                Task<DeleteObjectResponse> deleteObjectResponseTask =
                    AmazonS3Client.DeleteObjectAsync(deleteObjectRequest);
                return PrepareFileRespose(keyFileName) == null;
            }
            catch (AmazonS3Exception s3Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Delete all files from a particular bucket
        /// </summary>
        /// <returns>int</returns>
        public int DeleteAllFilesInBucket()
        {
            int filesDeleted = 0;
            IList<S3Object> getAllFilesInBucket = GetFilesinBucket();
            foreach (var obj in getAllFilesInBucket)
            {
                DeleteObjectRequest deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = _bucketName,
                    Key = obj.Key
                };
                try
                {
                    Task<DeleteObjectResponse> deleteObjectResponseTask =
                        AmazonS3Client.DeleteObjectAsync(deleteObjectRequest);
                    filesDeleted++;
                }
                catch (AmazonS3Exception s3Exception)
                {
                    break;
                }
            }
            return filesDeleted;
        }

        #endregion

        #endregion
    }
}