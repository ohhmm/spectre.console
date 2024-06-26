namespace Spectre.Console.Tests;

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
            var fileName = $"{className}.{methodName}.verified.txt";
            AnsiConsole.MarkupLine($"[DEBUG] Custom DerivePathInfo: {directory}/{fileName}"); // Debugging: Log the derived path info
            return new(directory, fileName);
        });
    }
}
