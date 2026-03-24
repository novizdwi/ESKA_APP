using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using LumenWorks.Framework.IO.Csv;

namespace Models._Utils 
{
   public class Csv
    {
        //true,
        //                            System.Convert.ToChar(","),
        //                            System.Convert.ToChar("\"")
        public static DataTable CsvToDataTable(string FileName, bool hasHeaders)
        {
            DataTable csvTable;

            using (CsvReader csv = new CsvReader(
                                      new StreamReader(FileName),
                                          hasHeaders
                                  ))
            {
                int fieldCount = csv.FieldCount;
                string[] headers = csv.GetFieldHeaders();
                csvTable = new DataTable();
                csvTable.Load(csv);
            }

            return csvTable;
        }

        public static DataTable CsvToDataTable(string FileName, bool hasHeaders, char delimiter)
        {
            DataTable csvTable;

            using (CsvReader csv = new CsvReader(
                                      new StreamReader(FileName),
                                          hasHeaders,
                                          delimiter
                                  ))
            {
                int fieldCount = csv.FieldCount;
                string[] headers = csv.GetFieldHeaders();
                csvTable = new DataTable();
                csvTable.Load(csv);
            }

            return csvTable;
        }

        public static DataTable CsvToDataTable(string FileName, bool hasHeaders, char delimiter, char quote)
        {
            DataTable csvTable;

            using (CsvReader csv = new CsvReader(
                                      new StreamReader(FileName),
                                          hasHeaders,
                                          delimiter,
                                            quote
                                  ))
            {
                int fieldCount = csv.FieldCount;
                string[] headers = csv.GetFieldHeaders();
                csvTable = new DataTable();
                csvTable.Load(csv);
            }

            return csvTable;
        }



        public static DataTable CsvToDataTable(string FileName, bool hasHeaders, char delimiter, char quote, char escape, char comment, ValueTrimmingOptions trimmingOptions)
        {
            DataTable csvTable;

            using (CsvReader csv = new CsvReader(
                                      new StreamReader(FileName),
                                          hasHeaders,
                                          delimiter,
                                            quote,
                                            escape,
                                         comment,
                                         trimmingOptions
                                  ))
            {
                int fieldCount = csv.FieldCount;
                string[] headers = csv.GetFieldHeaders();
                csvTable = new DataTable();
                csvTable.Load(csv);
            }

            return csvTable;
        }


    }
}
