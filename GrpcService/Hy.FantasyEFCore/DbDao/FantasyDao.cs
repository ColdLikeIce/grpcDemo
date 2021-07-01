using DapperAid;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hy.FantasyEFCore.DbDao
{
    public static class FantasyDao
    {
        private readonly static MySqlConnection _conn = new MySqlConnection(DaoEnv.GetConnStr());

        public static async Task<List<T>> Save<T>(List<T> obj)
        {
            try
            {
                int affected = await _conn.InsertOrUpdateRowsAsync<T>(obj);
                return obj;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public static async Task<List<T>> Select<T>(Expression<Func<T, bool>> where)
        {
            try
            {
                var list = await _conn.SelectAsync<T>(where);
                return list.ToList();
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}