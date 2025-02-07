public class FileTests
{
    private static string programFilePath = @"../../../../HomeEnergyApi/Program.cs";
    private string programContent = File.ReadAllText(programFilePath);

    [Fact]
    public void DoesProgramFileAddScopedServiceHomeRepository()
    {
        bool containsHomeRepositoryScoped = programContent.Contains("builder.Services.AddScoped<HomeRepository>();");
        Assert.True(containsHomeRepositoryScoped,
            "HomeEnergyApi/Program.cs does not add a Scoped Service of type `HomeRepository`");
    }

    [Fact]
    public void DoesProgramFileAddScopedServiceIReadRepositoryWithRequiredServiceProviderHomeRepository()
    {
        bool containsIReadScoped = programContent.Contains("builder.Services.AddScoped<IReadRepository<int, Home>>(provider => provider.GetRequiredService<HomeRepository>());");
        Assert.True(containsIReadScoped,
            "HomeEnergyApi/Program.cs does not add a Scoped Service of type `IReadRepository` with the required Service Provider of type `HomeRepository`");
    }

    [Fact]
    public void DoesProgramFileAddScopedServiceIWriteRepositoryWithRequiredServiceHomeProviderRepository()
    {
        bool containsIWriteScoped = programContent.Contains("builder.Services.AddScoped<IWriteRepository<int, Home>>(provider => provider.GetRequiredService<HomeRepository>());");
        Assert.True(containsIWriteScoped,
            "HomeEnergyApi/Program.cs does not add a Scoped Service of type `IWriteRepository` with the required Service Provider of type `HomeRepository`");
    }

    [Fact]
    public void DoesProgramFileAddTransientForZipLocationService()
    {
        bool containsTransient = programContent.Contains("builder.Services.AddTransient<ZipCodeLocationService>();");
        Assert.True(containsTransient,
            "HomeEnergyApi/Program.cs does not add a Transient Service of type `ZipLocationService`");
    }

    [Fact]
    public void DoesProgramFileAddHttpClientForZipLocationService()
    {
        bool containsHttpClient = programContent.Contains("builder.Services.AddHttpClient<ZipCodeLocationService>();");
        Assert.True(containsHttpClient,
            "HomeEnergyApi/Program.cs does not add a HttpClient Service of type `ZipLocationService`");
    }

    [Fact]
    public void DoesProgramFileAddDBContextService()
    {
        bool containsHttpClient = programContent.Contains("builder.Services.AddDbContext<HomeDbContext>");
        Assert.True(containsHttpClient,
            "HomeEnergyApi/Program.cs does not add a DbContext Service with type HomeDBContext");
    }

    [Fact]
    public void DoesProgramFileDBContextServiceHaveUseSQLITEOption()
    {
        bool containsHttpClient = programContent.Contains("options.UseSqlite");
        Assert.True(containsHttpClient,
            "HomeEnergyApi/Program.cs does not add a DbContext Service that uses the option UseSqlite");
    }

    [Fact]
    public void DoesProgramFileGetRequiredServiceOfTypeHomeDbContext()
    {
        bool containsHttpClient = programContent.Contains(".ServiceProvider.GetRequiredService<HomeDbContext>();");
        Assert.True(containsHttpClient,
            "HomeEnergyApi/Program.cs does not get a required service of type HomeDbContext");
    }

    [Fact]
    public void DoesProgramFileCallDatabaseMigrate()
    {
        bool containsHttpClient = programContent.Contains(".Database.Migrate();");
        Assert.True(containsHttpClient,
            "HomeEnergyApi/Program.cs does not call Database.Migrate()");
    }
}
