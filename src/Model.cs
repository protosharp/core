using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace OOPArt
{
    public class Model: IDisposable
    {
        protected const string CONNECTION_STRING = "";
        private IDbConnection _connection;

        public Model()
        {
            this._connection = new NpgsqlConnection (CONNECTION_STRING);

            this._connection.Open();
        }

        public virtual string TableName => "unknow";


        public IEnumerable<object> All()
        {
            return this._connection.Query<object>($"SELECT * FROM {this.TableName};");
        }

        public bool Create()
        {
            return this._connection.Execute($"INSERT INTO {this.TableName} (name, age) VALUES ('{Guid.NewGuid().ToString()}', {(new Random()).Next(0, 100)});") > 0;
        }
        
        public object Find()
        {
            return this._connection.Query<object>($"SELECT * FROM {this.TableName} LIMIT 1");
        }


        public void Dispose()
        {
            if(this._connection.State == ConnectionState.Open)
            {
                this._connection.Close();
            }
        }

    }
}