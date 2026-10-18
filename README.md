# FileOperations

[![CI](https://github.com/XFiler-Community/FileOperations/actions/workflows/ci.yml/badge.svg)](https://github.com/XFiler-Community/FileOperations/actions/workflows/ci.yml)
[![License: MIT](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/XFiler-Community/FileOperations/blob/main/LICENSE)

Platform-native file operations for .NET 10 desktop apps: move to Recycle Bin / Trash, well-known user
folders, logical drives and drive mount notifications — behind one set of interfaces for Windows and macOS.
Used by the [X-Filer](https://x-filer.com) file manager.

| Package | Target | Version | Downloads |
|---|---|---|---|
| [XFiler.FileOperations.Abstractions](https://www.nuget.org/packages/XFiler.FileOperations.Abstractions) | `net10.0` | [![NuGet](https://img.shields.io/nuget/v/XFiler.FileOperations.Abstractions.svg)](https://www.nuget.org/packages/XFiler.FileOperations.Abstractions) | [![Downloads](https://img.shields.io/nuget/dt/XFiler.FileOperations.Abstractions.svg)](https://www.nuget.org/packages/XFiler.FileOperations.Abstractions) |
| [XFiler.FileOperations.Windows](https://www.nuget.org/packages/XFiler.FileOperations.Windows) | `net10.0-windows` | [![NuGet](https://img.shields.io/nuget/v/XFiler.FileOperations.Windows.svg)](https://www.nuget.org/packages/XFiler.FileOperations.Windows) | [![Downloads](https://img.shields.io/nuget/dt/XFiler.FileOperations.Windows.svg)](https://www.nuget.org/packages/XFiler.FileOperations.Windows) |
| [XFiler.FileOperations.Mac](https://www.nuget.org/packages/XFiler.FileOperations.Mac) | `net10.0-macos` | [![NuGet](https://img.shields.io/nuget/v/XFiler.FileOperations.Mac.svg)](https://www.nuget.org/packages/XFiler.FileOperations.Mac) | [![Downloads](https://img.shields.io/nuget/dt/XFiler.FileOperations.Mac.svg)](https://www.nuget.org/packages/XFiler.FileOperations.Mac) |
| [Egorozh.Windows.ImageOperations](https://www.nuget.org/packages/Egorozh.Windows.ImageOperations) | `net10.0-windows` | [![NuGet](https://img.shields.io/nuget/v/Egorozh.Windows.ImageOperations.svg)](https://www.nuget.org/packages/Egorozh.Windows.ImageOperations) | [![Downloads](https://img.shields.io/nuget/dt/Egorozh.Windows.ImageOperations.svg)](https://www.nuget.org/packages/Egorozh.Windows.ImageOperations) |

## What is inside

| Interface (Abstractions) | Windows | macOS |
|---|---|---|
| `IFileOperations` — delete a file or folder permanently or to the Recycle Bin / Trash, list logical drives | `WindowsFileOperations` (Shell `SHFileOperation`) | `MacOsFileOperations` (`NSFileManager.TrashItem`, mounted volumes) |
| `IPlatformFolders` — app data, Desktop, Downloads, Documents, Movies, Music, Pictures | `WindowsPlatformFolders` | `MacOsPlatformFolders` |
| `IDriveDetector` — `DriveChanged` event when a drive is added or removed | `WindowsDriveDetector` (WMI `Win32_VolumeChangeEvent`) | `MacOsDriveDetector` (`NSWorkspace` mount/unmount notifications; `IDisposable`) |

`WindowsFileOperations` also has static helpers that go through the Windows Shell with its own progress
and confirmation UI: `CopyFile`/`CopyFiles`/`CopyDirectory`, `MoveFile`/`MoveFiles`/`MoveDirectory`,
`DeleteFile`/`DeleteFiles`/`DeleteDirectory` and `Rename*`.

`Egorozh.Windows.ImageOperations` extracts the Shell icon of a file (`ImageSystem.GetIcon`,
`ImageSystem.GetIconStream`).

## Install

Reference the abstractions from shared code and the platform package from the app head:

```xml
<ItemGroup>
  <PackageReference Include="XFiler.FileOperations.Abstractions" Version="10.0.0" />
</ItemGroup>

<ItemGroup Condition="'$(TargetFramework)' == 'net10.0-windows'">
  <PackageReference Include="XFiler.FileOperations.Windows" Version="10.0.0" />
</ItemGroup>

<ItemGroup Condition="'$(TargetFramework)' == 'net10.0-macos'">
  <PackageReference Include="XFiler.FileOperations.Mac" Version="10.0.0" />
</ItemGroup>
```

The Windows package targets `net10.0-windows`, so a plain `net10.0` project cannot reference it — the
Windows head of the app has to target `net10.0-windows` too. The Mac package needs the `macos` workload
(`dotnet workload install macos`).

## Usage

```csharp
using FileOperations.Abstractions;
using FileOperations.Abstractions.Enums;
#if MACOS
using FileOperations.MacOs;
#else
using FileOperations.Windows;
#endif

#if MACOS
IFileOperations fileOperations = new MacOsFileOperations();
IPlatformFolders folders = new MacOsPlatformFolders();
#else
IFileOperations fileOperations = new WindowsFileOperations();
IPlatformFolders folders = new WindowsPlatformFolders();
#endif

// Recycle Bin on Windows, Trash on macOS; throws if the item cannot be moved there.
fileOperations.DeleteFile(path, RecycleOption.SendToRecycleBin);
fileOperations.DeleteDirectory(folder, DeleteDirectoryOption.DeleteAllContents, RecycleOption.SendToRecycleBin);

string settingsFolder = Path.Combine(folders.GetAppDataPath(), "MyApp");
```

Watch for drives being plugged in or ejected:

```csharp
#if MACOS
IDriveDetector detector = new MacOsDriveDetector();
#else
IDriveDetector detector = new WindowsDriveDetector();
#endif

detector.DriveChanged += (type, drive) => Console.WriteLine($"{type}: {drive}");
```

All packages are marked AOT-compatible.

## Build

```bash
cd src
dotnet build FileOperations.sln
```

`EnableWindowsTargeting` is on, so the Windows projects also build on macOS and Linux (they only run on
Windows). Building the Mac project needs macOS with Xcode and the `macos` workload.

## Release

CI (`.github/workflows/ci.yml`) builds and packs the Windows packages on a Windows runner and the Mac
package on a macOS runner, on every push and pull request to `main`. Pushing a `v*` tag publishes all
packages to NuGet.org with the version taken from the tag (`v10.0.1` → `10.0.1`); this needs the
`NUGET_API_KEY` repository secret.

```bash
git tag v10.0.1
git push origin v10.0.1
```

## Changelog

**10.0.0**
- Targets .NET 10: `net10.0` (Abstractions), `net10.0-windows` (Windows, ImageOperations), `net10.0-macos` (Mac).
- macOS: moving to Trash throws an `IOException` when it fails instead of silently doing nothing.
- macOS: `MacOsDriveDetector` reports mounted and unmounted volumes (it was an empty stub).

**8.0.0** — first release, .NET 8.

## License

[MIT](https://github.com/XFiler-Community/FileOperations/blob/main/LICENSE)
