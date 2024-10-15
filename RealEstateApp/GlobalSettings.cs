namespace RealEstateApp;

public class GlobalSettings
{
    public static GlobalSettings Instance { get; } = new();

    public string ImageBaseUrl => "https://dbroadfootpluralsight.blob.core.windows.net/files/";
    public string NoImageUrl => ImageBaseUrl + "no_image.jpg";
}