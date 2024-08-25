namespace FileOperations.Abstractions.Enums;


/// <summary>
/// Specify the action to do when deleting a directory and it is not empty.
/// </summary>
public enum DeleteDirectoryOption
{
    ThrowIfDirectoryNonEmpty = 4,
    
    DeleteAllContents = 5
}