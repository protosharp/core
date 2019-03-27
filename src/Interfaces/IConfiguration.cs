using System.Data;

namespace ProtoSharp.Interfaces
{
    public interface IConfiguration
    {
         IDbConnection Database();
    }
}