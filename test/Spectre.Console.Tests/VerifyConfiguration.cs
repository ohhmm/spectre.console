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
            Verifier.DerivePathInfo((sourceFile, projectDirectory, type, method) =>
            {
                var directory = Path.Combine(projectDirectory, "Snapshots");
                var className = type.Name;
                var methodName = method.Name;
                var fileName = $"{className}.{methodName}";
                return new(directory, fileName);
            });

            var settings = new VerifySettings();
            settings.UseDirectory("Snapshots");
            settings.UseFileName($"{type.Name}.{method.Name}");

            VerifierSettings.DisableRequireUniquePrefix();
            VerifierSettings.UniqueForRuntime();
            VerifierSettings.UniqueForRuntimeAndVersion();
        }
    }
}
