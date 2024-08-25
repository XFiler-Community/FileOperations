using FileOperations.Abstractions.Enums;


namespace FileOperations.Abstractions;


public interface IFileOperations
{
    void DeleteDirectory(string directory, DeleteDirectoryOption onDirectoryNotEmpty, RecycleOption recycle = RecycleOption.DeletePermanently);
    
    
    void DeleteFile(string file, RecycleOption recycle);

    
    string[] GetLogicalDrives();
}