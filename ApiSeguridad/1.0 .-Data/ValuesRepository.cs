using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace ApiSeguridad._0._0_._Data
{
    public class ValuesRepository
    {
        private readonly string _stringConnection;
        private MySqlConnection sql;
        private MySqlCommand cmd;
        private readonly IConfiguration _configuration;

        public ValuesRepository(IConfiguration configuration) {
            this._configuration = configuration;
            this._stringConnection = _configuration["StringConnection"];
        }

        public void OpenStore(string nameStore) {
            this.sql = new MySqlConnection(this._stringConnection);
            this.cmd = new MySqlCommand(nameStore, sql);
            this.cmd.CommandType = System.Data.CommandType.StoredProcedure;
            this.sql.Open();
        }

        public void AddParametherIn(string name,object value) {
            this.cmd.Parameters.AddWithValue(name,value);
        }

        public void AddParametherOut(string name, MySqlDbType type)
        {
            this.cmd.Parameters.Add(name, type).Direction = System.Data.ParameterDirection.Output;
        }

        public MySqlDataReader GetValuesStore() {
            return this.cmd.ExecuteReader();
        }

        public object GetParam(string name)
        {
            return this.cmd.Parameters[name].Value;
        }

        public void CloseStore() {
            this.sql.Close();
        }

    }
}
