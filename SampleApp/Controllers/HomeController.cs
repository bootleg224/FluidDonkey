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
		private readonly IWebHostEnvironment _host;

		public HomeController(ILogger<HomeController> logger, IWebHostEnvironment host)
		{
			_logger = logger;
			_host = host;
		}

	
		public IActionResult Index(string templateName)
		{
			var workOrder = WorkOrder.GetSampleWorkOrder();

			if (string.IsNullOrEmpty(templateName))
				templateName = "WorkOrder";
						
			var FluidTemplate = System.IO.File.ReadAllText(Path.Combine(_host.ContentRootPath, @$"Templates\{templateName}.html"));

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


			var workOrder = WorkOrder.GetSampleWorkOrder();


			var wb = new XLWorkbook();
			var ws = wb.Worksheets.Add("Work Order");

			// Add company logo to worksheet
			//var logo = ws.AddPicture(workOrder.Company.LogoUrl)
			//			 .MoveTo(ws.Cell(1, 1))
			//			 .WithSize(100, 100);

			
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
				ws.Cell(rowIndex, 4).Value = item.RecurringStyle == ProductRevenueRecurringStyle.Recurring ? "Recurring" : "One-Time";
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
	}	
}