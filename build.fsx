// include Fake lib
#r @"tools/fake/FakeLib.dll"
open Fake
open Fake.AssemblyInfoFile

// Product information.
let productDescription = "A set helpers and tools for the Microsoft Azure platform."
let productName = "Red Dog"

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
    CreateCSharpAssemblyInfo "./src/RedDog.Messenger/Properties/AssemblyInfo.cs"
        [Attribute.Title "RedDog.Messenger"
         Attribute.Description productDescription
         Attribute.Product productName
         Attribute.Version version
         Attribute.FileVersion version]
    CreateCSharpAssemblyInfo "./src/RedDog.Messenger.Containers.Autofac/Properties/AssemblyInfo.cs"
        [Attribute.Title "RedDog.Messenger.Containers.Autofac"
         Attribute.Description productDescription
         Attribute.Product productName
         Attribute.Version version
         Attribute.FileVersion version]
    CreateCSharpAssemblyInfo "./src/RedDog.Messenger.Contracts/Properties/AssemblyInfo.cs"
        [Attribute.Title "RedDog.Messenger.Contracts"
         Attribute.Description productDescription
         Attribute.Product productName
         Attribute.Version version
         Attribute.FileVersion version]
    CreateCSharpAssemblyInfo "./src/RedDog.Messenger.Serialization.Avro/Properties/AssemblyInfo.cs"
        [Attribute.Title "RedDog.Messenger.Serialization.Avro"
         Attribute.Description productDescription
         Attribute.Product productName
         Attribute.Version version
         Attribute.FileVersion version]
    CreateCSharpAssemblyInfo "./src/RedDog.Messenger.Serialization.Json/Properties/AssemblyInfo.cs"
        [Attribute.Title "RedDog.Messenger.Serialization.Json"
         Attribute.Description productDescription
         Attribute.Product productName
         Attribute.Version version
         Attribute.FileVersion version]
    CreateCSharpAssemblyInfo "./src/RedDog.Storage/Properties/AssemblyInfo.cs"
        [Attribute.Title "RedDog.Storage"
         Attribute.Description productDescription
         Attribute.Product productName
         Attribute.Version version
         Attribute.FileVersion version]
    CreateCSharpAssemblyInfo "./src/RedDog.ServiceBus/Properties/AssemblyInfo.cs"
        [Attribute.Title "RedDog.ServiceBus"
         Attribute.Description productDescription
         Attribute.Product productName
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
            Description = productDescription
            OutputPath = packagingDir
            Summary = productDescription
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
            Description = productDescription
            OutputPath = packagingDir
            Summary = productDescription
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
            Description = productDescription
            OutputPath = packagingDir
            Summary = productDescription
            WorkingDir = workingDir
            Version = version }) "./nuget/RedDog.Messenger.nuspec"

    // Prepare RedDog.Messenger.Containers.Autofac.
    let workingDir = packagingDir @@ "RedDog.Messenger.Containers.Autofac"
    let net40Dir = workingDir @@ "lib/net40-full/"
    CleanDirs [workingDir; net40Dir]
    CopyFile net40Dir (buildDir @@ "RedDog.Messenger.Containers.Autofac.dll")
    
    // Package RedDog.Messenger.Containers.Autofac
    NuGet (fun p ->
        {p with
            Authors = author
            Project = "RedDog.Messenger.Containers.Autofac"
            Description = productDescription
            OutputPath = packagingDir
            Summary = productDescription
            WorkingDir = workingDir
            Version = version }) "./nuget/RedDog.Messenger.Containers.Autofac.nuspec"
            
    // Prepare RedDog.Messenger.Serialization.Avro.
    let workingDir = packagingDir @@ "RedDog.Messenger.Serialization.Avro"
    let net40Dir = workingDir @@ "lib/net40-full/"
    CleanDirs [workingDir; net40Dir]
    CopyFile net40Dir (buildDir @@ "RedDog.Messenger.Serialization.Avro.dll")
    
    // Package RedDog.Messenger.Serialization.Avro
    NuGet (fun p ->
        {p with
            Authors = author
            Project = "RedDog.Messenger.Serialization.Avro"
            Description = productDescription
            OutputPath = packagingDir
            Summary = productDescription
            WorkingDir = workingDir
            Version = version }) "./nuget/RedDog.Messenger.Serialization.Avro.nuspec"
            
    // Prepare RedDog.Messenger.Serialization.Json.
    let workingDir = packagingDir @@ "RedDog.Messenger.Serialization.Json"
    let net40Dir = workingDir @@ "lib/net40-full/"
    CleanDirs [workingDir; net40Dir]
    CopyFile net40Dir (buildDir @@ "RedDog.Messenger.Serialization.Json.dll")
    
    // Package RedDog.Messenger.Serialization.Json
    NuGet (fun p ->
        {p with
            Authors = author
            Project = "RedDog.Messenger.Serialization.Json"
            Description = productDescription
            OutputPath = packagingDir
            Summary = productDescription
            WorkingDir = workingDir
            Version = version }) "./nuget/RedDog.Messenger.Serialization.Json.nuspec"
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