using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBasesSample
{
    class Model
    {
        protected static string dbName = "testDB";
        protected static string dbConnStr = DataBasesSample.Properties.Settings.Default.ClientConnectionString;

        //Добавляет USE DBname для корректой работы с БД
        protected static string prepareSqlCommand(string sqlQuery)
        {
            string correctSql = "USE " + dbName + "; " + sqlQuery;
            return correctSql;
        }
    }
}
