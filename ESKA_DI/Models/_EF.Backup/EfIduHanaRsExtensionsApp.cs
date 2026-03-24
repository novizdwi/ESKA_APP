using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Dynamic;

using Sap.Data.Hana;
using System.Data;
using MJL_DI.Models._EF;
using Models._Ef;


namespace Models._Ef
{
    public static class EfIduHanaRsExtensionsApp
    {
        public static HanaDataReader IduGetHanaDataReader(HANA_APP CONTEXT, string StrSql, params object[] parameters)
        {

            HanaConnection HanaConn = (HanaConnection)CONTEXT.Database.Connection;
            if (HanaConn.State == ConnectionState.Broken)
            {
                HanaConn.Close();
            }
            if (HanaConn.State == ConnectionState.Closed)
            {
                HanaConn.Open();
            }

            HanaCommand command = new HanaCommand();
            command.Connection = HanaConn;
            command.CommandType = CommandType.Text;
            command.CommandText = StrSql;

            int i = 0;
            foreach (var param in parameters)
            {
                command.Parameters.Add(":p" + i.ToString(), param);

                //command.Parameters[":p" + i.ToString()].Value = param;
                i++;
            }

            return command.ExecuteReader(CommandBehavior.SchemaOnly);
        }

        public static DataView IduGetDataView(HANA_APP CONTEXT, string sql, params object[] parameters)
        {

            DataSet Ds;
            Ds = IduGetDataSet(CONTEXT, sql, parameters);
            DataView Dv = new DataView(Ds.Tables[0]);
            return Dv;
        }

        public static DataTable IduGetDataTable(string sql, params object[] parameters)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return IduGetDataTable(CONTEXT, sql, parameters);
            }
        }

        public static DataTable IduGetDataTable(HANA_APP CONTEXT, string sql, params object[] parameters)
        {
            DataSet Ds;
            Ds = IduGetDataSet(CONTEXT, sql, parameters);
            return Ds.Tables[0];
        }


        public static DataSet IduGetDataSet(HANA_APP CONTEXT, string sql, params object[] parameters)
        {

            HanaConnection HanaConn = (HanaConnection)CONTEXT.Database.Connection;
            if (HanaConn.State == ConnectionState.Broken)
            {
                HanaConn.Close();
            }
            if (HanaConn.State == ConnectionState.Closed)
            {
                HanaConn.Open();
            }

            var ds = new DataSet();

            HanaCommand command = new HanaCommand();
            command.Connection = HanaConn;
            command.CommandType = CommandType.Text;
            command.CommandText = sql;

            int i = 0;
            foreach (var param in parameters)
            {
                command.Parameters.Add(":p" + i.ToString(), param);
                //command.Parameters[":p" + i.ToString()].Value = param;
                i++;
            }

            //var adapter = new HanaDataAdapter(sql, HanaConn);
            var adapter = new HanaDataAdapter(command);


            adapter.Fill(ds);

            return ds;
        }


        public static IEnumerable<dynamic> GetData(HANA_APP CONTEXT, string StrSql, params object[] parameters)
        {
            HanaConnection HanaConn = (HanaConnection)CONTEXT.Database.Connection;
            //HanaCommand command = new HanaCommand();
            //command.Connection = HanaConn;
            //command.CommandType = CommandType.Text;
            //command.CommandText = StrSql;


            using (var command = new HanaCommand(StrSql, HanaConn))
            {
                if (HanaConn.State == ConnectionState.Broken)
                {
                    HanaConn.Close();
                }
                if (HanaConn.State == ConnectionState.Closed)
                {
                    HanaConn.Open();
                }

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return GetDynamicData(reader);
                    }
                }
            }
        }

        private static dynamic GetDynamicData(HanaDataReader reader)
        {
            var expandoObject = new ExpandoObject() as IDictionary<string, object>;
            for (int i = 0; i < reader.FieldCount; i++)
            {
                expandoObject.Add(reader.GetName(i), reader[i]);
            }
            return expandoObject;
        }


    }
}