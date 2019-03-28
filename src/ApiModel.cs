using Dapper;
using Newtonsoft.Json;
using Npgsql;
using ProtoSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ProtoSharp
{
    public delegate void ProcessApi(object content);

    public abstract class ApiModel: Model
    {
        public virtual string Url => "";

        public ApiModel(IDbConnection connection) : base(connection) { }

        public override IEnumerable<object> All()
        {
            this.Sync();

            return base.All();
        }

        public virtual dynamic Process(dynamic content)
        {
            return content;
        }

        public virtual void ProccessLot(dynamic lot)
        {
            foreach(object item in lot)
            {
                ProcessItem(item);
            }
        }

        public virtual void ProcessItem(dynamic item)
        {
            this.Create(item.id.ToString(), item.name.ToString());
        }

        public async Task Sync()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(this.Url);

            if(response.IsSuccessStatusCode)
            {
                string body = await response.Content.ReadAsStringAsync();
                var json = JsonConvert.DeserializeObject<dynamic>(body);

                var content = this.Process(json);
                this.ProccessLot(content);
            }

            return;
        }
    }
}