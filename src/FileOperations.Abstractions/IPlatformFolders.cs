namespace FileOperations.Abstractions;

public interface IPlatformFolders
{
    /// <summary>
    /// <para>
    /// Windows: %APPDATA%\
    /// </para>
    /// <para>
    /// macOS: $HOME/Library/Application Support/
    /// </para>
    /// <para>
    /// Linux: $HOME/.config/
    /// </para>
    /// </summary>
    string GetAppDataPath();


    string? GetDesktopFolderPath();

    string? GetDownloadFolderPath();

    string? GetDocumentsFolderPath();

    string? GetMoviesFolderPath();

    string? GetMusicFolderPath();

    string? GetPicturesFolderPath();
}