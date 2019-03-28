using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace ProtoSharp
{
    public class Model: IDisposable
    {
        protected IDbConnection _connection;

        public Model(IDbConnection connection)
        {
            this._connection = connection;

            this._connection.Open();
        }

        public virtual string TableName => "unknow";


        public virtual IEnumerable<object> All()
        {
            return this._connection.Query<object>($"SELECT * FROM {this.TableName};");
        }

        public virtual bool Create(string id, string name)
        {
            return this._connection.Execute($"INSERT INTO {this.TableName} (id, name) VALUES ('{id}', '{name}');") > 0;
        }

        public virtual bool Create()
        {
            return this._connection.Execute($"INSERT INTO {this.TableName} (name, age) VALUES ('a name', {(new Random()).Next(0, 100)});") > 0;
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