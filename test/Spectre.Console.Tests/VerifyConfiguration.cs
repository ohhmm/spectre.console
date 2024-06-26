namespace Spectre.Console.Tests
{
    public static class VerifyConfiguration
    {
        [ModuleInitializer]
        public static void Init()
        {
            Verifier.DerivePathInfo((sourceFile, projectDirectory, type, method) =>
            {
                var directory = Path.GetDirectoryName(sourceFile);
                var className = type.Name;
                var methodName = method.Name;
                var fileName = $"{className}.{methodName}";
                return new(directory, fileName);
            });

            VerifierSettings.DisableRequireUniquePrefix();
            VerifierSettings.UseDirectory("Snapshots");
            VerifierSettings.UseFileName("CustomFileName");
            VerifierSettings.UniqueForRuntime();
            VerifierSettings.UniqueForRuntimeAndVersion();
        }
    }
}
