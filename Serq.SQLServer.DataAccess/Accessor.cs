using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serq.SQLServer.DataAccess
{
    public class Accessor
    {
        public enum QueryType
        {
            Text,
            StoredProc
        }
        string connString = string.Empty;
        public Accessor(string connString)
        {
            this.connString = connString;
        }
      
        public List<Dictionary<string,object>> Read(string query, QueryType type=QueryType.Text, Dictionary<string,string> inputParams=null)
        {
            List<Dictionary<string, object>> retVal = new List<Dictionary<string, object>>();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = conn;
                    comm.CommandText = query;
                    if (type == QueryType.Text)
                    {
                        comm.CommandType = CommandType.Text;
                    }
                    else
                    {
                        comm.CommandType = CommandType.StoredProcedure;
                    }

                    if (inputParams != null)
                    {
                        comm.Parameters.AddRange(inputParams.Select(dbParam => new SqlParameter()
                        {
                            ParameterName = dbParam.Key,
                            Value = dbParam.Value,
                        }).ToArray());
                    }
                    conn.Open();
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Dictionary<string,object> record = new Dictionary<string, object>();
                            for (var i = 0; i < reader.FieldCount; i++)
                            {
                                record.Add(reader.GetName(i), reader[i]);
                            }
                            retVal.Add(record);
                        }
                        reader.Close();
                    }
                    conn.Close();
                }
            }
            
            return retVal;
        }

        public int Put(string query, QueryType type=QueryType.Text, Dictionary<string, string> inputParams = null)
        {
            int retVal = 0;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = conn;
                    comm.CommandText = query;
                    if (type == QueryType.Text)
                    {
                        comm.CommandType = CommandType.Text;
                    }
                    else
                    {
                        comm.CommandType = CommandType.StoredProcedure;
                    }

                    if (inputParams != null)
                    {
                        comm.Parameters.AddRange(inputParams.Select(dbParam => new SqlParameter()
                        {
                            ParameterName = dbParam.Key,
                            Value = dbParam.Value,
                        }).ToArray());
                    }
                    conn.Open();
                    retVal = comm.ExecuteNonQuery();
                    conn.Close();
                }
            }

            return retVal;
        } 
    }
}
