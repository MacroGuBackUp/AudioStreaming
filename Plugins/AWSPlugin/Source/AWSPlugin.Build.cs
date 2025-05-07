// Copyright 2025 (c) Tangha Technologies, LLC. All Rights Reserved.

using UnrealBuildTool;
using System;
using System.IO;
using System.Collections.Generic;

public class AWSPlugin : ModuleRules
{
    private static AWSPluginPlatform AWSPluginPlatformInstance;

    public AWSPlugin(ReadOnlyTargetRules Target) : base(Target)
	{
		PCHUsage = ModuleRules.PCHUsageMode.UseExplicitOrSharedPCHs;
        CppStandard = CppStandardVersion.Cpp20;
        UndefinedIdentifierWarningLevel = WarningLevel.Off;

        AWSPluginPlatformInstance = GetAWSPluginPlatformInstance(Target);
		
        // SDK. Source: https://github.com/aws/aws-sdk-cpp/blob/main/Docs/SDK_usage_guide.md#build-defines
        PublicDefinitions.Add("USE_IMPORT_EXPORT");
        PublicDefinitions.Add("AWS_CRT_CPP_USE_IMPORT_EXPORT");		

        if (Target.Configuration != UnrealTargetConfiguration.Shipping)
		{
			OptimizeCode = CodeOptimization.Never;
		}

		PublicIncludePaths.AddRange(
			new string[] {
				// ... add public include paths required here ...
				ModuleDirectory,
			}
			);
				
		PublicDependencyModuleNames.AddRange(
			new string[]
			{
				"Core",
				// ... add other public dependencies that you statically link with here ...
			}
			);


		PrivateDependencyModuleNames.AddRange(
			new string[]
			{
				"CoreUObject",
				"Engine",
                "UMG",
                "Slate",
				"SlateCore",
				"AppFramework"
				// ... add private dependencies that you statically link with here ...	
			}
			);

        //ThirdParty include
        PrivateIncludePaths.AddRange(
            new string[] {
                Path.Combine(ThirdPartyRoot(), "AWSSDK/Include")
            }
        );

        //ThirdParty Libraries
        ConfigurePlatform(Target.Platform.ToString(), Target.Configuration);

    }
    private AWSPluginPlatform GetAWSPluginPlatformInstance(ReadOnlyTargetRules Target)
    {
        var AWSPluginPlatformType = System.Type.GetType("AWSPluginPlatform_" + Target.Platform.ToString());
        if (AWSPluginPlatformType == null)
        {
            throw new BuildException("AWSPlugin does not support platform " + Target.Platform.ToString());
        }

        var PlatformInstance = Activator.CreateInstance(AWSPluginPlatformType) as AWSPluginPlatform;
        if (PlatformInstance == null)
        {
            throw new BuildException("AWSPlugin could not instantiate platform " + Target.Platform.ToString());
        }

        return PlatformInstance;
    }

    protected string ThirdPartyRoot()
    {
        return Path.GetFullPath(Path.Combine(ModuleDirectory, "./ThirdParty/"));
    }

    private bool ConfigurePlatform(string Platform, UnrealTargetConfiguration Configuration)
    {
        //AWSPlugin thirdparty libraries root path
        string root = ThirdPartyRoot();

        //AWSPlugin
        foreach (var lib in AWSSDKLibs)
        {
            foreach (var arch in AWSPluginPlatformInstance.Architectures())
            {
                string fullPath = root + "AWSSDK/" + "Binaries/" +
                    AWSPluginPlatformInstance.LibrariesPath + arch +
                    AWSPluginPlatformInstance.ConfigurationDir(Configuration) +
                    lib + AWSPluginPlatformInstance.LibraryPostfixName;
                PublicAdditionalLibraries.Add(fullPath);
            }
        }

        return false;
    }

    private List<string> AWSSDKLibs = new List<string>
    {
        "aws-c-auth",
        "aws-c-cal",
        "aws-c-common",
        "aws-c-compression",
        "aws-c-event-stream",
        "aws-checksums",
        "aws-c-http",
        "aws-c-io",
        "aws-c-mqtt",
        "aws-cpp-sdk-access-management",
        "aws-cpp-sdk-cognito-identity",
        "aws-cpp-sdk-core",
        "aws-cpp-sdk-iam",
        "aws-cpp-sdk-kinesis",
        "aws-crt-cpp",
        "aws-c-s3",
    };

}
public abstract class AWSPluginPlatform
{
    public virtual string ConfigurationDir(UnrealTargetConfiguration Configuration)
    {
        return "Release/";
    }
    public abstract string LibrariesPath { get; }
    public abstract List<string> Architectures();
    public abstract string LibraryPrefixName { get; }
    public abstract string LibraryPostfixName { get; }
}

public class AWSPluginPlatform_Win64 : AWSPluginPlatform
{
    public override string LibrariesPath { get { return "win64/"; } }
    public override List<string> Architectures() { return new List<string> { "" }; }
    public override string LibraryPrefixName { get { return ""; } }
    public override string LibraryPostfixName { get { return ".lib"; } }
}
