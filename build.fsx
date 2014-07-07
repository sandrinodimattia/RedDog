// include Fake lib
#r @"tools/fake/FakeLib.dll"
open Fake
open Fake.AssemblyInfoFile

// Properties
let version = environVarOrDefault "version" "0.1.1.0"
let buildDir = "./build/"
let packagingDir = "./packaging/"

let testDir = "./tests/output"
let testReferences = !! "./tests/**/*.csproj"

Target "Clean" (fun _ ->
    CleanDir buildDir
    CleanDir packagingDir
)

Target "BuildTests" (fun _ ->
    MSBuildDebug testDir "Build" testReferences
        |> Log "TestBuild-Output: "
)

Target "RunTests" (fun _ ->  
    !! (testDir + "/*.Tests.dll")
        |> xUnit (fun p -> 
            {p with 
                ShadowCopy = false;
                HtmlOutput = true;
                XmlOutput = true;
                OutputDir = testDir })
)

Target "BuildApp" (fun _ ->
    // Generate assembly info.
    CreateCSharpAssemblyInfo "./src/RedDog.Storage/Properties/AssemblyInfo.cs"
        [Attribute.Title "RedDog.Storage"
         Attribute.Description "Helpers for Microsoft Azure Storage"
         Attribute.Product "Red Dog for Microsoft Azure"
         Attribute.Version version
         Attribute.FileVersion version]
    CreateCSharpAssemblyInfo "./src/RedDog.ServiceBus/Properties/AssemblyInfo.cs"
        [Attribute.Title "RedDog.ServiceBus"
         Attribute.Description "Helpers for Microsoft Azure Service Bus"
         Attribute.Product "Red Dog for Microsoft Azure"
         Attribute.Version version
         Attribute.FileVersion version]

    // Build all projects.
    !! "./src/**/*.csproj"
      |> MSBuildRelease buildDir "Build"
      |> Log "AppBuild-Output: "
)

Target "CreatePackages" (fun _ ->
    let author = ["Sandrino Di Mattia"]
    
    // Prepare RedDog.Storage.
    let workingDir = packagingDir @@ "RedDog.Storage"
    let net40Dir = workingDir @@ "lib/net40/"
    CleanDirs [workingDir; net40Dir]
    CopyFile net40Dir (buildDir @@ "RedDog.Storage.dll")
    
    // Package RedDog.Storage
    NuGet (fun p ->
        {p with
            Authors = author
            Project = "RedDog.Storage"
            Description = "Tools to help you build solutions on the Microsoft Azure platform."
            OutputPath = packagingDir
            Summary = "Tools to help you build solutions on the Microsoft Azure platform."
            WorkingDir = workingDir
            Version = version }) "./nuget/RedDog.Storage.nuspec"
    
    // Prepare RedDog.ServiceBus.
    let workingDir = packagingDir @@ "RedDog.ServiceBus"
    let net40Dir = workingDir @@ "lib/net40-full/"
    CleanDirs [workingDir; net40Dir]
    CopyFile net40Dir (buildDir @@ "RedDog.ServiceBus.dll")
    
    // Package RedDog.ServiceBus
    NuGet (fun p ->
        {p with
            Authors = author
            Project = "RedDog.ServiceBus"
            Description = "Tools to help you build solutions on the Microsoft Azure platform."
            OutputPath = packagingDir
            Summary = "Tools to help you build solutions on the Microsoft Azure platform."
            WorkingDir = workingDir
            Version = version }) "./nuget/RedDog.ServiceBus.nuspec"
			
    // Prepare RedDog.Messenger.
    let workingDir = packagingDir @@ "RedDog.Messenger"
    let net40Dir = workingDir @@ "lib/net40-full/"
    CleanDirs [workingDir; net40Dir]
    CopyFile net40Dir (buildDir @@ "RedDog.Messenger.dll")
    CopyFile net40Dir (buildDir @@ "RedDog.Messenger.Contracts.dll")
    
    // Package RedDog.Messenger
    NuGet (fun p ->
        {p with
            Authors = author
            Project = "RedDog.Messenger"
            Description = "Tools to help you build solutions on the Microsoft Azure platform."
            OutputPath = packagingDir
            Summary = "Tools to help you build solutions on the Microsoft Azure platform."
            WorkingDir = workingDir
            Version = version }) "./nuget/RedDog.Messenger.nuspec"
)
    
// Default target
Target "Default" (fun _ ->
    let msg = "Building RedDog version: " + version
    trace msg
)

// Dependencies
"Clean"
   ==> "BuildApp"
   ==> "BuildTests"
   ==> "RunTests"
   ==> "CreatePackages"
   ==> "Default"
  
// Start Build
RunTargetOrDefault "Default"