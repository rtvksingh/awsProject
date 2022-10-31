using Amazon.Runtime;
using Amazon.S3;
using Microsoft.AspNetCore.Mvc;

namespace AWSfile.Controllers
{
    [Route("api/buckets")]
    [ApiController]
    public class BucketsController : ControllerBase
    {

        private readonly IAmazonS3 _s3Client;
        
        public BucketsController()
        {
            var credentials = new BasicAWSCredentials("AKIA6F5VDVGSQ2DRYNUI", "nJqdj3PtlMx/2/8vJeSfVQ6Dj948D6+l5PgUDFkw");
            _s3Client=new AmazonS3Client(credentials, Amazon.RegionEndpoint.USWest2);
        }

        [HttpPost("create")]    
        public async Task<IActionResult> CreateBucketAsync(string bucketName)
        {
            var bucketExists = await _s3Client.DoesS3BucketExistAsync(bucketName);
            if (bucketExists) return BadRequest($"Bucket {bucketName} already exists.");
            await _s3Client.PutBucketAsync(bucketName);
            return Ok($"Bucket {bucketName} created.");
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllBucketAsync()
        {
            var data = await _s3Client.ListBucketsAsync();
            var buckets = data.Buckets.Select(b => { return b.BucketName; });
            return Ok(buckets);
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteBucketAsync(string bucketName)
        {
            await _s3Client.DeleteBucketAsync(bucketName);
            return NoContent();
        }
    }
}
