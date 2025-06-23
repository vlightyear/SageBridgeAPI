namespace SageBridgeAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SageBridgeAPI.Services;
    using Pastel.Evolution;
    using System.Dynamic;

    [ApiController]
    [Route("api/[controller]")]
    public class SageController : ControllerBase
    {
        private readonly SageService _sageService;

        public SageController(SageService sageService)
        {
            _sageService = sageService;
        }

        [HttpPost("customer")]
        public IActionResult CreateCustomer([FromBody] CustomerRequest request)
        {
            dynamic response = new ExpandoObject();
            try
            {
                Customer customer = _sageService.CreateCustomer(request.CustomerID, request.CustomerName);
                response.status = "Ok";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.status = "Error";
                response.Exception = ex.Message;
                return BadRequest(response);
            }
        }

        [HttpPost("CustomerTransaction")]
        public IActionResult PostCustomerTransaction([FromBody] CustomerTransactionRequest request)
        {
            dynamic response = new ExpandoObject();
            try
            {
                Customer customer = _sageService.CreateCustomer(request.CustomerCode, request.CustomerName);
                response.status = "Ok";
            }
            catch (Exception ex)
            {
                response.status = "Error";
            }

            try
            {
                CustomerTransaction CustTran = _sageService.PostCustomerTransaction(request.CustomerCode, request.Reference, request.Description, request.GLAccount, request.Amount);
                response.status = "Ok";
                response.sage_id = CustTran.ID;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.status = "Error";
                response.Exception = ex.Message;
                return BadRequest(response);
            }
        }

        [HttpPost("SalesOrderTest")]
        public IActionResult SalesOrderTest([FromBody] SalesOrderRequest request)
        {
            dynamic response = new ExpandoObject();
            try
            {
                SalesOrder order = _sageService.PostSalesOrder(request.CustomerCode, request.OrderItems);
                response.status = "Ok";
                response.SalesOrderNumber = order.OrderNo;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.status = "Error";
                response.Exception = ex.Message;
                return BadRequest(response);
            }
        }
    }

    public class CustomerRequest
    {
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
    }

    public class CustomerTransactionRequest
    {
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string Reference { get; set; }
        public string Description { get; set; }
        public string GLAccount { get; set; }
        public double Amount { get; set; }
    }

    public class SalesOrderRequest
    {
        public string CustomerCode { get; set; }
        public string? CustomerName { get; set; }
        public string? Reference { get; set; }
        public string? Description { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }

    public class OrderItem
    {
        public TaxMode TaxMode { get; set; } = TaxMode.Inclusive;
        public string InventoryItem { get; set; }
        public int Quantity { get; set; } = 1;
        public double UnitSellingPrice { get; set; } = 1;
        public string GLAccount { get; set; }
    }
}
