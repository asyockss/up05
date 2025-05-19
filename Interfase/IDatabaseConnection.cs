using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.Interfase
{
    public interface IDatabaseConnection
    {
        object OpenConnection();
        object Query(string sql, object connection);
        void CloseConnection(object connection);
    }
}
