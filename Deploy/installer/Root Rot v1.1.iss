; LANDIS-II Extension infomation
#define CoreRelease "LANDIS-II-V7"
#define ExtensionName "Root Rot"
#define AppVersion "1.1"
#define AppPublisher "LANDIS-II Foundation"
#define AppURL "http://www.landis-ii.org/"

; Build directory
;define BuildDir "..\..\src\bin\Debug"
#define BuildDir "..\libs"

; LANDIS-II installation directories
#define ExtDir "C:\Program Files\LANDIS-II-v7\extensions"
#define AppDir "C:\Program Files\LANDIS-II-v7"
#define LandisPlugInDir "C:\Program Files\LANDIS-II-v7\plug-ins-installer-files"
#define ExtensionsCmd AppDir + "\commands\landis-ii-extensions.cmd"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{D5C7F295-81A3-440E-9942-422ACF547973}
AppName={#CoreRelease} {#ExtensionName}
AppVersion={#AppVersion}
; Name in "Programs and Features"
AppVerName={#CoreRelease} {#ExtensionName} v{#AppVersion}
AppPublisher={#AppPublisher}
AppPublisherURL={#AppURL}
AppSupportURL={#AppURL}
AppUpdatesURL={#AppURL}
DefaultDirName={commonpf}\{#ExtensionName}
DisableDirPage=yes
DefaultGroupName={#ExtensionName}
DisableProgramGroupPage=yes
LicenseFile=LANDIS-II_Binary_license.rtf
OutputDir={#SourcePath}
OutputBaseFilename={#CoreRelease} {#ExtensionName} {#AppVersion}-setup
Compression=lzma
SolidCompression=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"


[Files]
; This .dll IS the extension (ie, the extension's assembly)
; NB: Do not put an additional version number in the file name of this .dll
; (The name of this .dll is defined in the extension's \src\*.csproj file)
Source: {#BuildDir}\Landis.Extension.RootRot-v1.1.dll; DestDir: {#ExtDir}; Flags: ignoreversion

; Complete example for testing the extension
;Source: ..\examples\biomass-Pnet-succession-example\*.txt; DestDir: {#AppDir}\examples\{#ExtensionName}; Flags: replacesameversion
;Source: ..\examples\biomass-Pnet-succession-example\*.gis; DestDir: {#AppDir}\examples\{#ExtensionName}; Flags: replacesameversion skipifsourcedoesntexist
;Source: ..\examples\biomass-Pnet-succession-example\*.img; DestDir: {#AppDir}\examples\{#ExtensionName}; Flags: replacesameversion skipifsourcedoesntexist
;Source: ..\examples\biomass-Pnet-succession-example\*.bat; DestDir: {#AppDir}\examples\{#ExtensionName}; Flags: replacesameversion skipifsourcedoesntexist


; LANDIS-II identifies the extension with the info in this .txt file
; NB. New releases must modify the name of this file and the info in it
#define InfoTxt "Root Rot 1.1.txt"
Source: {#InfoTxt}; DestDir: {#LandisPlugInDir}
; NOTE: Don't use "Flags: ignoreversion" on any shared system files


[Run]
Filename: {#ExtensionsCmd}; Parameters: "remove ""Root Rot v1.1"" "; WorkingDir: {#LandisPlugInDir}
Filename: {#ExtensionsCmd}; Parameters: "add ""{#InfoTxt}"" "; WorkingDir: {#LandisPlugInDir} 


[UninstallRun]
; Remove "Root Rot" from "extensions.xml" file.
Filename: {#ExtensionsCmd}; Parameters: "remove ""Root Rot v1.1"" "; WorkingDir: {#LandisPlugInDir}


