using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace ProtoSharp
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


        public virtual IEnumerable<object> All()
        {
            return this._connection.Query<object>($"SELECT * FROM {this.TableName};");
        }

        public virtual bool Create()
        {
            return this._connection.Execute($"INSERT INTO {this.TableName} (name, age) VALUES ('{Guid.NewGuid().ToString()}', {(new Random()).Next(0, 100)});") > 0;
        }

        public virtual bool Delete(int id)
        {
            return this._connection.Execute($"DELETE FROM {this.TableName} WHERE id = {id};") > 0;
        }
        
        public virtual object Find()
        {
            return this._connection.Query<object>($"SELECT * FROM {this.TableName} LIMIT 1");
        }

        public virtual object Update(int id, object data)
        {
            return this._connection.Query<object>($"UPDATE {this.TableName} SET WHERE id = {id};");
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