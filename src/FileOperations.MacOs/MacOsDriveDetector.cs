using System;
using FileOperations.Abstractions;
using FileOperations.Abstractions.Enums;

namespace FileOperations.MacOs;

public class MacOsDriveDetector : IDriveDetector
{
    public event Action<DriveDetectType, string>? DriveChanged;
}