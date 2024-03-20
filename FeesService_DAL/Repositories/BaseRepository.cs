using System.Data;
using System.Data.Common;


namespace FeesService_DAL.Repositories;

public class BaseRepository
{
    private readonly DbProviderFactory _provider; 
    protected string _cn;
    protected IDbConnection? Connection;
    private bool _disposed = false;
    public BaseRepository(DbProviderFactory provider, string cn)
    {
        if (provider is null) throw new NullReferenceException("No provider is passed");
        if (cn is null) throw new NullReferenceException("No connection string is passed");
        _provider = provider; 
        _cn = cn;
    }
    protected void CreateConnection()
    {
        Connection = _provider.CreateConnection();

        if (Connection != null)
        {
            Connection.ConnectionString = _cn;
            Connection.Open();
        }
        else throw new Exception();        
    }
    
    private void CleanUp()
    {
        if (_disposed) return; 
        if (Connection?.State != 0) Connection?.Close();
        _disposed = true;
    }
    public void Dispose()
    {
        CleanUp();        
    }

}
