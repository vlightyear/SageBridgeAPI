namespace SageBridgeAPI.Services
{
    using System;
    using Pastel.Evolution;
    using SageBridgeAPI.Controllers;

    public class SageService
    {
        public SageService()
        {
            DatabaseContext.CreateCommonDBConnection("41.63.0.222", "EDEN_SDK Module", "SageSDK", "SageSDK", false);
            DatabaseContext.SetLicense("DE12111100", "7825105");
            DatabaseContext.CreateConnection("41.63.0.222", "Eden_Sage_Test", "SageSDK", "SageSDK", false);
        }

        public Customer CreateCustomer(string customerId, string customerName)
        {
            Customer C = new Customer();
            C.Code = customerId;
            C.Description = customerName;
            C.Save();

            return C;
        }

        /*public CustomerTransaction PostCustomerTransaction(string customerCode, string reference, string description, string GLAccount, double amount)
        {
            CustomerTransaction CustTran = new CustomerTransaction();
            CustTran.Customer = new Customer(customerCode);
            CustTran.TransactionCode = new TransactionCode(Module.AR, "IN");
            CustTran.TaxRate = new TaxRate("1");
            CustTran.Amount = amount;
            CustTran.Reference = reference;
            CustTran.Description = description;
            CustTran.OverrideCreditAccount = new GLAccount(GLAccount);
            CustTran.Post();

            return CustTran;
        }
*/
        public SalesOrder PostSalesOrder(string CustomerCode, List<OrderItem> OrderItems)
        {
            SalesOrder order = new SalesOrder();
            Customer cust = new Customer(CustomerCode);
            order.Customer = cust;

            foreach (var oi in OrderItems)
            {
                OrderDetail OD = new OrderDetail
                {
                    TaxMode = oi.TaxMode,
                    InventoryItem = new InventoryItem(oi.InventoryItem),
                    Quantity = oi.Quantity,
                    ToProcess = oi.Quantity,
                    UnitSellingPrice = oi.UnitSellingPrice
                };
                order.Detail.Add(OD);
                OD.GLAccount = new GLAccount(oi.GLAccount);
            }

            order.Save();

            return order;
        }

        public SalesOrder CompleteSalesOrder(string OrderNo)
        {
            SalesOrder so = new SalesOrder(OrderNo);
            so.Complete();

            return so;
        }
    }

}
