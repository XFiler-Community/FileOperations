using FileOperations.Abstractions;
using Microsoft.Win32;

namespace FileOperations.Windows;

public class WindowsPlatformFolders : IPlatformFolders
{
    public string GetAppDataPath() => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

    public string? GetDesktopFolderPath() => Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

    public string? GetDownloadFolderPath()
    {
        return Registry
            .GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders",
                "{374DE290-123F-4565-9164-39C4925E467B}", null)
            ?.ToString() ?? null;
    }

    public string? GetDocumentsFolderPath() => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

    public string? GetMoviesFolderPath() => Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);

    public string? GetMusicFolderPath() => Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);

    public string? GetPicturesFolderPath() => Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
}