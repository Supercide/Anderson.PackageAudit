///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////
var target                  = Argument<string>("target", "Default");
var buildConfig             = Argument<string>("buildConfig", "Release");
var buildVersion            = Argument<string>("buildVersion", "0.1.0");

//////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
//////////////////////////////////////////////////////////////////////
var artifacts               = "./artifacts";
var testResults             = string.Concat(artifacts, "/test-results/");
var solution                = GetFiles("./**/*.sln").FirstOrDefault();

///////////////////////////////////////////////////////////////////////////////
// SETUP
///////////////////////////////////////////////////////////////////////////////

Task("Check")
    .WithCriteria(() => IsSolutionMissing(solution))
    .Does(() => {
        throw new Exception("oops.. i was unable to find your solution");
    });

Task("Clean")
    .IsDependentOn("Check")
    .Does(() => {
        CleanDirectories("./**/bin");
        CleanDirectories("./**/obj");
        CleanDirectories(string.Concat("./**/", artifacts));
    });

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() => 
    {
        NuGetRestore(solution.FullPath);
    });

Task("Build")
    .IsDependentOn("Restore")
    .Does(() =>
    {
        var buildSettings = new DotNetCoreBuildSettings
        {
            Configuration = buildConfig,
            ArgumentCustomization = args => args.Append("/p:buildVersion=" + buildVersion)
        };

        DotNetCoreBuild(solution.FullPath, buildSettings);
    });

public bool IsSolutionMissing(FilePath filePath) =>
                  filePath == null ||
                  string.IsNullOrWhiteSpace(filePath.FullPath);

///////////////////////////////////////////////////////////////////////////////
// Run Tests
///////////////////////////////////////////////////////////////////////////////

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
    {
        var settings = new DotNetCoreTestSettings
        {
           NoBuild = true,
           Configuration = buildConfig,
           ArgumentCustomization = args => {
                args.Append("--logger:trx");
                args.Append("--results-directory:"+MakeAbsolute(File(testResults)));
                return args;
           }
        };
        var testProjects = GetFiles("./tests/**/*.csproj");
        foreach(var projectFile in testProjects)
        {
            DotNetCoreTest(projectFile.FullPath, settings);
        }
    });
///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Test");

RunTarget(target);