using Microsoft.AspNetCore.Mvc;
using SampleApp.Models;
using System.Diagnostics;
using Fluid;
using ClosedXML.Excel;


namespace SampleApp.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		public IActionResult GenerateFluid()
		{
			var Company = new Company
			{
				Name = "Acme Co.",
				Address = "123 Main St",
				City = "Anytown",
				State = "CA",
				ZipCode = "12345",
				LogoUrl = "http://example.com/logo.png"
			};

			var Customer = new Customer
			{
				Name = "John Doe",
				Address = "456 Broadway St",
				City = "Anytown",
				State = "CA",
				ZipCode = "12345",
				Email = "john.doe@example.com"
			};

			var items = new List<WorkOrderItem>
{
	new WorkOrderItem
	{
		ProductName = "Product 1",
		UnitPrice = 10.00m,
		Quantity = 2,
		RecurringOrOneTime = "One-Time"
	},
	new WorkOrderItem
	{
		ProductName = "Product 2",
		UnitPrice = 5.00m,
		Quantity = 3,
		RecurringOrOneTime = "Recurring"
	},
	new WorkOrderItem
	{
		ProductName = "Product 3",
		UnitPrice = 7.50m,
		Quantity = 1,
		RecurringOrOneTime = "One-Time"
	},
	new WorkOrderItem
	{
		ProductName = "Product 4",
		UnitPrice = 20.00m,
		Quantity = 1,
		RecurringOrOneTime = "One-Time"
	},
	new WorkOrderItem
	{
		ProductName = "Product 5",
		UnitPrice = 15.00m,
		Quantity = 2,
		RecurringOrOneTime = "Recurring"
	}
};

			var productData = new List<Product>
{
	new Product { ProductName = "Product 1", Description = "This is the first product" },
	new Product { ProductName = "Product 2", Description = "This is the second product" },
	new Product { ProductName = "Product 3", Description = "This is the third product" },
	new Product { ProductName = "Product 4", Description = "This is the fourth product" },
	new Product { ProductName = "Product 5", Description = "This is the fifth product" }
};

			var workOrder = new WorkOrder
			{
				Company = Company,
				Customer = Customer,
				Items = items,
				TermsAndConditions = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed non risus. Suspendisse lectus tortor, dignissim sit amet, adipiscing nec, ultricies sed, dolor. Cras elementum ultrices diam. Maecenas ligula massa, varius a, semper congue, euismod non, mi. Proin porttitor, orci nec nonummy molestie, enim est eleifend mi, non fermentum diam nisl sit amet erat.",
				Products = productData
			};

			var parser = new FluidParser();
			
			if(parser.TryParse(FluidTemplate, out var template, out string error))
			{
				var options = new TemplateOptions();
				options.MemberAccessStrategy = new UnsafeMemberAccessStrategy();

				var context = new TemplateContext( workOrder, options);				
				var result = template.Render(context);

				return Content(result, "text/html");

			}

			else
			{
				return Content(error);
			}

			return null;
	

		}

		public IActionResult GenerateExcel()
		{
			var Company = new Company
			{
				Name = "Acme Co.",
				Address = "123 Main St",
				City = "Anytown",
				State = "CA",
				ZipCode = "12345",
				LogoUrl = "https://res.cloudinary.com/crunchbase-production/image/upload/c_lpad,h_170,w_170,f_auto,b_white,q_auto:eco,dpr_1/v1398384448/onoi8gdjjxsin78cxd82.png"
			};

			var Customer = new Customer
			{
				Name = "John Doe",
				Address = "456 Broadway St",
				City = "Anytown",
				State = "CA",
				ZipCode = "12345",
				Email = "john.doe@example.com"
			};

			var items = new List<WorkOrderItem>
{
	new WorkOrderItem
	{
		ProductName = "Product 1",
		UnitPrice = 10.00m,
		Quantity = 2,
		RecurringOrOneTime = "One-Time"
	},
	new WorkOrderItem
	{
		ProductName = "Product 2",
		UnitPrice = 5.00m,
		Quantity = 3,
		RecurringOrOneTime = "Recurring"
	},
	new WorkOrderItem
	{
		ProductName = "Product 3",
		UnitPrice = 7.50m,
		Quantity = 1,
		RecurringOrOneTime = "One-Time"
	},
	new WorkOrderItem
	{
		ProductName = "Product 4",
		UnitPrice = 20.00m,
		Quantity = 1,
		RecurringOrOneTime = "One-Time"
	},
	new WorkOrderItem
	{
		ProductName = "Product 5",
		UnitPrice = 15.00m,
		Quantity = 2,
		RecurringOrOneTime = "Recurring"
	}
};

			var productData = new List<Product>
{
	new Product { ProductName = "Product 1", Description = "This is the first product" },
	new Product { ProductName = "Product 2", Description = "This is the second product" },
	new Product { ProductName = "Product 3", Description = "This is the third product" },
	new Product { ProductName = "Product 4", Description = "This is the fourth product" },
	new Product { ProductName = "Product 5", Description = "This is the fifth product" }
};

			var workOrder = new WorkOrder
			{
				Company = Company,
				Customer = Customer,
				Items = items,
				TermsAndConditions = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed non risus. Suspendisse lectus tortor, dignissim sit amet, adipiscing nec, ultricies sed, dolor. Cras elementum ultrices diam. Maecenas ligula massa, varius a, semper congue, euismod non, mi. Proin porttitor, orci nec nonummy molestie, enim est eleifend mi, non fermentum diam nisl sit amet erat.",
				Products = productData
			};


			var wb = new XLWorkbook();
			var ws = wb.Worksheets.Add("Work Order");

			// Add company logo to worksheet
			//var logo = ws.AddPicture(workOrder.Company.LogoUrl)
			//			 .MoveTo(ws.Cell(1, 1))
			//			 .WithSize(100, 100);

			// Add company name and address to worksheet
			ws.Cell(1, 2).Value = workOrder.Company.Name;
			ws.Cell(2, 2).Value = workOrder.Company.Address;
			ws.Cell(3, 2).Value = workOrder.Company.City + ", " + workOrder.Company.State + " " + workOrder.Company.ZipCode;

			// Add customer information to worksheet
			ws.Cell(5, 1).Value = "Customer Information";
			ws.Cell(6, 1).Value = "Name:";
			ws.Cell(6, 2).Value = workOrder.Customer.Name;
			ws.Cell(7, 1).Value = "Address:";
			ws.Cell(7, 2).Value = workOrder.Customer.Address;
			ws.Cell(8, 1).Value = "Email:";
			ws.Cell(8, 2).Value = workOrder.Customer.Email;

			// Add work order items to worksheet
			ws.Cell(10, 1).Value = "Work Order Items";
			ws.Cell(11, 1).Value = "Product Name";
			ws.Cell(11, 2).Value = "Unit Price";
			ws.Cell(11, 3).Value = "Quantity";
			ws.Cell(11, 4).Value = "Recurring/One-Time";
			ws.Cell(11, 5).Value = "Subtotal";

			var rowIndex = 12;
			foreach (var item in workOrder.Items)
			{
				ws.Cell(rowIndex, 1).Value = item.ProductName;
				ws.Cell(rowIndex, 2).Value = item.UnitPrice;
				ws.Cell(rowIndex, 3).Value = item.Quantity;
				ws.Cell(rowIndex, 4).Value = item.RecurringOrOneTime;
				ws.Cell(rowIndex, 5).FormulaA1 = string.Format("B{0}*C{0}", rowIndex);
				rowIndex++;
			}

			// Add subtotal to worksheet
			ws.Cell(rowIndex, 4).Value = "Subtotal:";
			ws.Cell(rowIndex, 5).FormulaA1 = string.Format("SUM(E12:E{0})", rowIndex - 1);

			// Add terms and conditions to worksheet
			ws.Cell(rowIndex + 2, 1).Value = "Terms and Conditions:";
			ws.Cell(rowIndex + 2, 2).Value = workOrder.TermsAndConditions;

			// Add signature block to worksheet
			ws.Cell(rowIndex + 4, 1).Value = "Signature:";
			ws.Cell(rowIndex + 4, 2).Value = "______________________";

			// Save the workbook
			var ms = new MemoryStream();
			wb.SaveAs(ms);

			return File(ms.ToArray(), "application/download", "workbook.xlsx");
		}



		public const string FluidTemplate = @"<!DOCTYPE html>
<html>
  <head>
    <meta charset=""utf-8"">
    <title>Work Order</title>
<style>
  body {
    font-family: Arial, sans-serif;
  }

  .header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 20px;
  }

  .logo img {
    max-height: 100px;
  }

  .Company-info p {
    margin: 0;
  }

  .Customer-info {
    margin-bottom: 20px;
  }

  .items {
    margin-bottom: 20px;
  }

  .items table {
    width: 100%;
    border-collapse: collapse;
  }

  .items table th,
  .items table td {
    border: 1px solid #ddd;
    padding: 8px;
  }

  .items table th {
    text-align: left;
    background-color: #f2f2f2;
  }

  .subtotal {
    margin-top: 20px;
  }

  .terms {
    margin-bottom: 20px;
  }

  .signature {
    margin-bottom: 20px;
  }

  .page-break {
    page-break-after: always;
  }

  .products table {
    width: 100%;
    border-collapse: collapse;
  }

  .products table th,
  .products table td {
    border: 1px solid #ddd;
    padding: 8px;
  }

  .products table th {
    text-align: left;
    background-color: #f2f2f2;
  }
</style>

  </head>
  <body>
    <div class=""header"">
      <div class=""logo"">
        <img src=""{{ Company.LogoUrl }}"" alt=""{{ Company.Name }} logo"">
      </div>
      <div class=""Company-info"">
        <p>{{ Company.Name }}</p>
        <p>{{ Company.Address }}, {{ Company.City }}, {{ Company.State }} {{ Company.ZipCode }}</p>
      </div>
    </div>
    <div class=""Customer-info"">
      <h2>Customer Information</h2>
      <p><strong>Name:</strong> {{ Customer.Name }}</p>
      <p><strong>Address:</strong> {{ Customer.Address }}, {{ Customer.City }}, {{ Customer.State }} {{ Customer.ZipCode }}</p>
      <p><strong>Email:</strong> {{ Customer.Email }}</p>
    </div>
    <div class=""items"">
      <h2>Work Order Items</h2>
      <table>
        <thead>
          <tr>
            <th>Product Name</th>
            <th>Unit Price</th>
            <th>Quantity</th>
            <th>Recurring/One-Time</th>
            <th>Subtotal</th>
          </tr>
        </thead>
        <tbody>
          {% for item in Items %}
          <tr>
            <td>{{ item.ProductName }}</td>
            <td>${{ item.UnitPrice }}</td>
            <td>{{ item.Quantity }}</td>
            <td>{{ item.RecurringOrOneTime }}</td>
            <td>${ item.UnitPrice * item.Quantity }</td>
          </tr>
          {% endfor %}
        </tbody>
      </table>
      <div class=""subtotal"">
        <p>Subtotal: ${{ Items | map: ""UnitPrice"" | times: ""Quantity"" | sum }}</p>
      </div>
    </div>
    <div class=""terms"">
      <p><strong>Terms and Conditions:</strong> {{ order.TermsAndConditions }}</p>
    </div>
    <div class=""signature"">
      <label for=""signature"">Signature:</label>
      <input type=""text"" id=""signature"" name=""signature"">
      <button>Submit</button>
    </div>
    <div class=""page-break""></div>
    <div class=""products"">
      <h2>Products</h2>
      <table>
        <thead>
          <tr>
            <th>Product Name</th>
            <th>Description</th>
          </tr>
        </thead>
        <tbody>
          {% for product in Products %}
          <tr>
            <td>{{ product.ProductName }}</td>
            <td>{{ product.Description }}</td>
          </tr>
          {% endfor %}
        </tbody>
      </table>
    </div>
  </body>
</html>";
	}

	#region DataModel
	public class Company
	{
		public string Name { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string ZipCode { get; set; }
		public string LogoUrl { get; set; }
	}

	public class Customer
	{
		public string Name { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string ZipCode { get; set; }
		public string Email { get; set; }
	}

	public class WorkOrderItem
	{
		public string ProductName { get; set; }
		public decimal UnitPrice { get; set; }
		public int Quantity { get; set; }
		public string RecurringOrOneTime { get; set; }
	}

	public class WorkOrder
	{
		public Company Company { get; set; }
		public Customer Customer { get; set; }
		public List<WorkOrderItem> Items { get; set; }
		public string TermsAndConditions { get; set; }
	public List<Product> Products { get;  set; }
}

	public class Product
	{
		public string ProductName { get; set; }
		public string Description { get; set; }
	}
	#endregion
}