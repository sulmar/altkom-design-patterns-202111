using System;
using System.Collections.Generic;
using System.Linq;

namespace BuilderPattern
{
    // Abstract Builder
    public interface IFluentSalesReportBuilder
    {
        IFluentSalesReportBuilder AddHeader(string title);
        IFluentSalesReportBuilder AddSectionByGender();
        IFluentSalesReportBuilder AddSectionByProduct();
        IFluentSalesReportBuilder AddFooter();
        SalesReport Build();
    }

    
    // Leniwy budowniczy w wersji generycznej
    // TSelf    - typ budowniczego
    // TSubject - typ produktu
    public abstract class FunctionalBuilder<TSubject, TSelf>
        where TSelf : FunctionalBuilder<TSubject, TSelf>
        where TSubject : new()
    {
        // W tej liście przechowujemy akcje do wykonania
        private readonly IList<Func<TSubject, TSubject>> actions = new List<Func<TSubject, TSubject>>();

        public TSelf Do(Action<TSubject> action) => AddAction(action);

        private TSelf AddAction(Action<TSubject> action)
        {
            actions.Add(p => { action(p); return p; });

            return (TSelf)this;
        }

        // Dopiero w tym momencie wykonujemy wszystkie akcje 
        // Zamiast pętli foreach zastosowano funkcję Aggregate
        public TSubject Build() => actions.Aggregate(new TSubject(), (p, f) => f(p));

    }


    // Dzięki budowniczemu w wersji generycznej utworzenie leniwego budownicznego jest łatwiejsze:
    public class LazySalesReportBuilder : FunctionalBuilder<SalesReport, LazySalesReportBuilder>, IFluentSalesReportBuilder
    {
        private readonly IEnumerable<Order> orders;

        public LazySalesReportBuilder(IEnumerable<Order> orders)
        {
            this.orders = orders;
        }

        public IFluentSalesReportBuilder AddHeader(string title) => Do(salesReport =>
        {
            salesReport.Title = title;
            salesReport.CreateDate = DateTime.Now;
            salesReport.TotalSalesAmount = orders.Sum(s => s.Amount);
        });

        public IFluentSalesReportBuilder AddSectionByGender() => Do(salesReport =>
        {
            salesReport.GenderDetails = orders
              .GroupBy(o => o.Customer.Gender)
              .Select(g => new GenderReportDetail(
                          g.Key,
                          g.Sum(x => x.Details.Sum(d => d.Quantity)),
                          g.Sum(x => x.Details.Sum(d => d.LineTotal))));
        });

        public IFluentSalesReportBuilder AddSectionByProduct() => Do(salesReport =>
        {
            salesReport.ProductDetails = orders
             .SelectMany(o => o.Details)
             .GroupBy(o => o.Product)
             .Select(g => new ProductReportDetail(g.Key, g.Sum(p => p.Quantity), g.Sum(p => p.LineTotal)));
        });

        public IFluentSalesReportBuilder AddFooter() => Do(salesReport => salesReport.Footer = $"Wydrukowano z programu ABC w dn. {DateTime.Now}");

    }



}