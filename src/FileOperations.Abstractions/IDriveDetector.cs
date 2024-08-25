using FileOperations.Abstractions.Enums;

namespace FileOperations.Abstractions;

public interface IDriveDetector
{
    event Action<DriveDetectType, string> DriveChanged;
}