namespace Smq.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStoreProcedure : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("GetRevenueStatistic",
               p => new
               {
                   fromDate = p.String(),
                   toDate = p.String()
               }
               ,
               @"  select ORD.CreatedDate as Date,
                           SUM(OD.Quantity*OD.Price) as Revenues,
                           SUM((OD.Quantity*OD.Price) - (OD.Quantity*PD.OriginalPrice)) as Benefit
                    from  OrderDetails OD inner join Orders ORD on ORD.ID = OD.OrderID
                                    inner join Products PD on PD.ID= OD.ProductID
                    where ORD.CreatedDate between @fromDate and @toDate
                    group by ORD.CreatedDate"
             );
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.GetRevenueStatistic");
        }
    }
}
