using System.IO;
using System.Runtime.CompilerServices;
using VerifyTests;

namespace Spectre.Console.Tests
{
    public static class VerifyConfiguration
    {
        [ModuleInitializer]
        public static void Init()
        {
            VerifierSettings.DisableRequireUniquePrefix();
            VerifierSettings.UniqueForRuntime();
            VerifierSettings.UniqueForRuntimeAndVersion();

            Verifier.DerivePathInfo((sourceFile, projectDirectory, type, method) =>
            {
                var directory = Path.Combine(projectDirectory, "Snapshots");
                var className = type.Name;
                var methodName = method.Name;
                var fileName = $"{className}.{methodName}";
                var settings = new VerifySettings();
                settings.UseDirectory(directory);
                settings.UseFileName(fileName);
                return new(directory, fileName);
            });
        }
    }
}
