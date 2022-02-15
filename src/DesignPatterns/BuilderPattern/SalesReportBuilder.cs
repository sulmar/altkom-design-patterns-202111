using System;
using System.Collections.Generic;
using System.Linq;

namespace BuilderPattern
{
    // Concrete Builder
    public class SalesReportBuilder : ISalesReportBuilder
    {
        // Product
        private SalesReport salesReport;

        private IEnumerable<Order> orders;

        public SalesReportBuilder(IEnumerable<Order> orders)
        {
            this.orders = orders;

            salesReport = new SalesReport();            
        }

        public void AddFooter()
        {
            salesReport.Footer = $"Wydrukowano z programu ABC w dn. {DateTime.Now}";
        }

        public void AddHeader()
        {
            salesReport.Title = "Raport sprzedaży";
            salesReport.CreateDate = DateTime.Now;
            salesReport.TotalSalesAmount = orders.Sum(s => s.Amount);
        }

        public void AddSectionByGender()
        {
            salesReport.GenderDetails = orders
               .GroupBy(o => o.Customer.Gender)
               .Select(g => new GenderReportDetail(
                           g.Key,
                           g.Sum(x => x.Details.Sum(d => d.Quantity)),
                           g.Sum(x => x.Details.Sum(d => d.LineTotal))));
        }

        public void AddSectionByProduct()
        {
            salesReport.ProductDetails = orders
             .SelectMany(o => o.Details)
             .GroupBy(o => o.Product)
             .Select(g => new ProductReportDetail(g.Key, g.Sum(p => p.Quantity), g.Sum(p => p.LineTotal)));
        }


        // Product
        public SalesReport Build()
        {           

            return salesReport;
        }

    }


}