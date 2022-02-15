namespace BuilderPattern
{
    // Abstract Builder
    public interface ISalesReportBuilder
    {
        void AddHeader();
        void AddSectionByGender();
        void AddSectionByProduct();

        void AddFooter();

        SalesReport Build();
    }

    #endregion


}