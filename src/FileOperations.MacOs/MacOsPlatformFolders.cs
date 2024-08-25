using System;
using System.IO;
using FileOperations.Abstractions;

namespace FileOperations.MacOs;

public class MacOsPlatformFolders : IPlatformFolders
{
    public string GetAppDataPath()
    {
        string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        string appDataPath = Path.Combine(home, "Library", "Application Support");

        return appDataPath;
    }

    public string? GetDesktopFolderPath() => Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

    public string? GetDownloadFolderPath()
    {
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "Downloads");
    }

    public string? GetDocumentsFolderPath() => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);


    public string? GetMoviesFolderPath()
    {
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "Movies");
    }

    public string? GetMusicFolderPath() => Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);

    public string? GetPicturesFolderPath() => Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
}