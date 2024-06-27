using Microsoft.Data.SqlClient;
using System.Text;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;

using System.Web;

namespace WebAppSimplifica2.Helpers
{
    public class Util
    {
        public static String ToJson(SqlDataReader rdr)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            using (JsonWriter jsonWriter = new JsonTextWriter(sw))
            {
                jsonWriter.WriteStartArray();

                while (rdr.Read())
                {
                    jsonWriter.WriteStartObject();
                    int fields = rdr.FieldCount;

                    for(int i = 0; i < fields; i++) 
                    {
                        jsonWriter.WritePropertyName(rdr.GetName(i));
                        jsonWriter.WriteValue(rdr[i]);
                    }
                    jsonWriter.WriteEndObject();
                }
                jsonWriter.WriteEndArray();
                return sw.ToString();

            }

        }

        public static string returnJson(SqlDataReader reader)
        {
            string result = "[";
            int columnCount = reader.FieldCount;

            while (reader.Read())
            {
                result += "{";

                for(int x = 0; x < columnCount; x++)
                {
                    result += "\"" + reader.GetName(x) + "\":\"";
                    string stringValue = "";
                    if (!reader.IsDBNull(x))
                    {
                        stringValue = (string)reader.GetValue(x);
                    }
                    else
                    {
                        stringValue = "NULL";
                    }

                    result += stringValue + "\"";

                    if (x < columnCount - 1)
                    {
                        result += ",";
                    }
                    
                }
                result += "},";
            }

            result = result.TrimEnd(result[result.Length - 1]) + "]";

            return result;
        }

    }
}
