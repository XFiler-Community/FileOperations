using System;
using AppKit;
using FileOperations.Abstractions;
using FileOperations.Abstractions.Enums;
using Foundation;

namespace FileOperations.MacOs;

/// <summary>Reports volumes mounted and unmounted, from the NSWorkspace notifications.</summary>
public class MacOsDriveDetector : IDriveDetector, IDisposable
{
    // NSWorkspaceVolumeURLKey; the binding does not expose the constant.
    private static readonly NSString VolumeUrlKey = new("NSWorkspaceVolumeURLKey");

    private readonly NSObject _mountObserver;
    private readonly NSObject _unmountObserver;

    public event Action<DriveDetectType, string>? DriveChanged;

    public MacOsDriveDetector()
    {
        var center = NSWorkspace.SharedWorkspace.NotificationCenter;

        _mountObserver = center.AddObserver(NSWorkspace.DidMountNotification,
            notification => Raise(DriveDetectType.Added, notification));
        _unmountObserver = center.AddObserver(NSWorkspace.DidUnmountNotification,
            notification => Raise(DriveDetectType.Removed, notification));
    }

    public void Dispose()
    {
        var center = NSWorkspace.SharedWorkspace.NotificationCenter;

        center.RemoveObserver(_mountObserver);
        center.RemoveObserver(_unmountObserver);
    }

    private void Raise(DriveDetectType type, NSNotification notification)
    {
        if (notification.UserInfo?[VolumeUrlKey] is NSUrl { Path: { } path })
            DriveChanged?.Invoke(type, path);
    }
}
