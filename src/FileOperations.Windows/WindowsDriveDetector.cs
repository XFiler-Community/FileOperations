using System.Management;
using FileOperations.Abstractions;
using FileOperations.Abstractions.Enums;

namespace FileOperations.Windows;

public class WindowsDriveDetector : IDriveDetector
{
    #region Private Fields

    private const string QUERY = "SELECT * FROM Win32_VolumeChangeEvent WHERE EventType = 2 or EventType = 3";

    #endregion


    #region Events

    public event Action<DriveDetectType, string>? DriveChanged;

    #endregion


    #region Constructor

    public WindowsDriveDetector()
    {
        ManagementEventWatcher watcher = new()
        {
            Query = new WqlEventQuery(QUERY)
        };

        watcher.EventArrived += WatcherOnEventArrived;

        watcher.Start();
    }

    #endregion


    #region Private Methods

    private void WatcherOnEventArrived(object sender, EventArrivedEventArgs e)
    {
        string? driveName = e.NewEvent.Properties["DriveName"].Value.ToString();

        bool res = int.TryParse(e.NewEvent.Properties["EventType"].Value.ToString(), out int eventTypeValue);

        if (res && driveName != null)
            RaiseDriveChanged(driveName, eventTypeValue);
    }


    private void RaiseDriveChanged(string driveName, int eventType)
    {
        DriveDetectType type = eventType switch
        {
            2 => DriveDetectType.Added,
            3 => DriveDetectType.Removed,

            _ => DriveDetectType.Unknown
        };

        DriveChanged?.Invoke(type, driveName);
    }

    #endregion
}