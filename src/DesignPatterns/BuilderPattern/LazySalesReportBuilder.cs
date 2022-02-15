using System;
using System.Collections.Generic;
using System.Linq;

namespace BuilderPattern
{
    public class LazySalesReportBuilder : ISalesReportBuilder
    {        
        private IEnumerable<Order> orders;

        private bool hasHeader;
        private bool hasSectionByGender;
        private bool hasSectionByProduct;


        public LazySalesReportBuilder(IEnumerable<Order> orders)
        {
            this.orders = orders;
        }

        public void AddFooter()
        {
            throw new NotImplementedException();
        }

        public void AddHeader()
        {
            hasHeader = true;
        }

        public void AddSectionByGender()
        {
            hasSectionByGender = true;
        }

        public void AddSectionByProduct()
        {
            hasSectionByProduct = true;          
        }


        // Product
        public SalesReport Build()
        {
            SalesReport salesReport = new SalesReport();

            if (hasHeader)
            {
                salesReport.Title = "Raport sprzedaży";
                salesReport.CreateDate = DateTime.Now;
                salesReport.TotalSalesAmount = orders.Sum(s => s.Amount);
            }

            if (hasSectionByGender)
            {
                salesReport.GenderDetails = orders
                  .GroupBy(o => o.Customer.Gender)
                  .Select(g => new GenderReportDetail(
                              g.Key,
                              g.Sum(x => x.Details.Sum(d => d.Quantity)),
                              g.Sum(x => x.Details.Sum(d => d.LineTotal))));
            }

            if (hasSectionByProduct)
            {
                salesReport.ProductDetails = orders
                   .SelectMany(o => o.Details)
                   .GroupBy(o => o.Product)
                   .Select(g => new ProductReportDetail(g.Key, g.Sum(p => p.Quantity), g.Sum(p => p.LineTotal)));
            }

            return salesReport;
        }

    }

    #endregion


}