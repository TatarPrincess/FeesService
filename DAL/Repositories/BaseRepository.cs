using System.Data;
using System.Data.Common;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;


namespace FeesService.DAL.Repositories;

public class BaseRepository
{
    private readonly DbProviderFactory _provider; /*SqlClientFactory.Instance;*/
    protected IDbConnection ?cn;
    bool _disposed = false;
    public BaseRepository(DbProviderFactory provider)
    {
        _provider = provider; 
    }
    protected IDbConnection CreateConnection()
    {
        var connectionString = GetProviderDataFromConfiguration();
        IDbConnection? connection = _provider.CreateConnection();

        if (connection != null)
        {
            connection.ConnectionString = connectionString;
            return connection;
        }
        else throw new Exception();        
    }
    protected IDbCommand CreateCommand(string commandText, IDbConnection cn) 
    {
        IDbCommand? command = _provider.CreateCommand();
        if (command != null)
        {
            command.Connection = cn;
            command.CommandText = commandText;
            return command;
        }
        else throw new Exception();
    }

    protected IDataReader ExecuteCommand(IDbCommand command, bool isSingleRow = false)
    {
        if (command.Connection != null)
        {
            IDataReader dataReader;
            dataReader = (isSingleRow) ? command.ExecuteReader(CommandBehavior.SingleRow & CommandBehavior.CloseConnection)
                                       : command.ExecuteReader(CommandBehavior.CloseConnection);
            return dataReader;
        }
        else throw new Exception();
    }
    static string GetProviderDataFromConfiguration()
    {
        IConfiguration config = new ConfigurationBuilder()
        .SetBasePath(@"C:\Store\C#\FeesService\FeesService\DAL\Repositories")
        .AddJsonFile("appsettings.json", true, true)
        .Build();
        var providerName = config["ProviderName"];

        if (providerName == "SqlServer")
        {
            var cn = config[$"{providerName}:ConnectionString"];
            return cn ?? "";
        }
        else throw new Exception("Invalid data provider value supplied.");
        
    }
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }
        if (disposing)
        {
            cn?.Dispose();
        }
        _disposed = true;
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

}
