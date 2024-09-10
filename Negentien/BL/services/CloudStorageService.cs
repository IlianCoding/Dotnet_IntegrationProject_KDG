using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;

namespace NT.BL.services;

public class CloudStorageService
{
    private readonly string _bucketName;
    private readonly StorageClient _client;


    public CloudStorageService()
    {
        GoogleCredential credential = GoogleCredential.GetApplicationDefault();
        _client = StorageClient.Create(credential);
        
        _bucketName = Environment.GetEnvironmentVariable("BUCKET_NAME");
    }


    public string UploadFileToBucket(MemoryStream memoryStream, string fileName, string contentType)
    {
        string folder = contentType.Split("/")[0];
        var objectName = $"{folder}/{fileName}";

        _client.UploadObject(_bucketName, objectName,
            contentType, memoryStream);

        return objectName;
    }

    public string GetMedia(string objectName)
    {
        if (String.IsNullOrWhiteSpace(objectName)  || !CheckObjectExists(objectName))
        {
            return null;
        }

        UrlSigner urlSigner = _client.CreateUrlSigner();
        string url = urlSigner.Sign(_bucketName, objectName, new TimeSpan(0, 3, 0), HttpMethod.Get);
        return url;
    }

    private bool CheckObjectExists( string objectName)
    {
        try
        {
            var obj = _client.GetObject(_bucketName, objectName);

            return true;
        }
        catch (Google.GoogleApiException ex) when (ex.Error.Code == 404)
        {
            return false;
        }
    }
    
    
    public void RemoveMedia(string objectName)
    {
        if (String.IsNullOrWhiteSpace(objectName) || !CheckObjectExists(objectName))
        {
            return ;
        }

        _client.DeleteObject(_bucketName,objectName);
    }
}