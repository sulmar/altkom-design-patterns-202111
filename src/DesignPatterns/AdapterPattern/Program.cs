using CrystalDecisions.CrystalReports;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace AdapterPattern
{
    // Abstract Adapter
    public interface IReportAdapter
    {
        void GenerateReport(string template, string output);
    }

    // Concrete Adapter
    public class CrystalReportsReportAdapter : CrystalDecisions.CrystalReports.ReportDocument, IReportAdapter
    {
        // Adaptee
        // private readonly CrystalDecisions.CrystalReports.ReportDocument rpt;

        public CrystalReportsReportAdapter()
            : base()
        {
            
        }
        public void GenerateReport(string template, string output)
        {
            this.Load(template);

            this.SetDatabaseLogon("user", "password");

            this.ExportToDisk(ReportDocument.ExportFormatType.PortableDocFormat, output);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Adapter Pattern!");

            MotorolaRadioTest();

            HyteriaRadioTest();

            // CrystalReportTest();
        }

        private static void CrystalReportTest()
        {
            IReportAdapter reportAdapter = new CrystalReportsReportAdapter();
            reportAdapter.GenerateReport("report1.rpt", "report1.pdf");
        }

        private static void MotorolaRadioTest()
        {
            IRadioAdapter radio = new MotorolaRadioAdapter("1234");
            radio.Send(10, "Hello World!");
        }

        private static void HyteriaRadioTest()
        {
            IRadioAdapter radio = new PanasonicRadioAdapter();
            radio.Send(10, "Hello World!");
        }
    }

    


}
