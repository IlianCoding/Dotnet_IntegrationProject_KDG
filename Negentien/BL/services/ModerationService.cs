using Google.Cloud.Language.V1;
using Google.Cloud.Vision.V1;

namespace NT.BL.services;

public class ModerationService
{
    private readonly LanguageServiceClient _languageClient;
    private readonly ImageAnnotatorClient _imageAnnotatorClient;
    
    public ModerationService()
    {
        string visionCredentialsPath = Environment.GetEnvironmentVariable("MODERATION_GOOGLE_APPLICATION_CREDENTIALS");
        string languageCredentialsPath = Environment.GetEnvironmentVariable("MODERATION_GOOGLE_APPLICATION_CREDENTIALS");

        _imageAnnotatorClient = new ImageAnnotatorClientBuilder
        {
            CredentialsPath = visionCredentialsPath
        }.Build();

        _languageClient = new LanguageServiceClientBuilder
        {
            CredentialsPath = languageCredentialsPath
        }.Build();
    }

    public bool IsGivenUserInputMalicious(string userInput)
    {
        var document = Document.FromPlainText(userInput);
        AnalyzeSentimentResponse response = _languageClient.AnalyzeSentiment(document);
        var sentiment = response.DocumentSentiment;

        if (sentiment.Magnitude > 0.6 && sentiment.Score < -0.2)
        {
            return true;
        } 
        if (sentiment.Score < -0.4)
        {
            return true;
        }

        if (sentiment.Magnitude > 0.95)
        {
            return true;
        }

        return false;
    }

    public async Task<bool> IsGivenUserImageMalicious(byte[] imageUploaded)
    {
        var image = Image.FromBytes(imageUploaded);
        var response = await _imageAnnotatorClient.DetectSafeSearchAsync(image);

        return response.Adult == Likelihood.Possible ||
               response.Violence == Likelihood.Possible ||
               response.Racy == Likelihood.Possible ||
               response.Spoof == Likelihood.Possible ||
               response.Medical == Likelihood.Possible;
    }
}