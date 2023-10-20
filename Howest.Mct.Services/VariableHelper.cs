namespace Howest.Mct.Services;

public static class VariableHelper
{
    private static string? _databaseUrl;
    private static string? _storageAccount;
    private static string? _connectionString;

    public static string DatabaseUrl => (_databaseUrl ??= Environment.GetEnvironmentVariable("DatabaseUrl"))
                                        ?? throw new ArgumentNullException();
    
    public static string StorageAccount => (_storageAccount ??= Environment.GetEnvironmentVariable("StorageAccount"))
                                           ?? throw new ArgumentNullException();

    public static string ConnectionString => (_connectionString ??= Environment.GetEnvironmentVariable("ConnectionString"))
                                             ?? throw new ArgumentNullException();

    public static string DefaultDatabase { get; } = "https://database.windows.net/.default";
}