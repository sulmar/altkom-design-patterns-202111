using System;
using System.Collections.Generic;

namespace CrystalDecisions.CrystalReports
{
    public class ReportDocument
    {
        public Database Database { get; set; }

        public void Load(string filename)
        {
            Console.WriteLine($"Load {filename}");
        }

        public void SetDatabaseLogon(string user, string password)
        {

        }

        public void ExportToDisk(ExportFormatType format, string filename)
        {

        }

        public enum ExportFormatType
        {
            PortableDocFormat,
            Word,
            Excel
        }
    }

    public struct ConnectionInfo
    {
        public string ServerName;
        public string DatabaseName;
        public string UserID;
        public string Password;
    }

    public class Table
    {
        public LogOnInfo LogOnInfo { get; set; }

        public void ApplyLogOnInfo(LogOnInfo logOnInfo)
        {

        }
    }

    public class Database
    {
        public IEnumerable<Table> Tables { get; set; }
    }

    public class LogOnInfo
    {
        public ConnectionInfo ConnectionInfo { get; set; }
    }


}
