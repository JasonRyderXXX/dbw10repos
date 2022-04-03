using System.Data;

namespace Rydersoft.Components.SimpleDB.Interfaces
{
    public interface ISimpleDB
    {
        List<IDictionary<string, object>> ExecuteSQLWithResult(string sql, IDictionary<string, object> parameters);
        void ExecuteSQL(string sql);
        IDictionary<string, IList<IDictionary<string, object>>> ExecuteSQLWithResult(string sql, IList<IDictionary<string, object>> parameters);

        IDataReader ExecuteSQLDataReader(string sql, IDictionary<string, object> parameters);
    }
}