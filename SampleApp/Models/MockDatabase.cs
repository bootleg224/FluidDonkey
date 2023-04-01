namespace SampleApp.Models
{
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

		public ProductRevenueStyle RevenueStyle { get; set; }
		public string RevenueStyleStr { get { return RevenueStyle.ToString(); } }

		public ProductQuantityStyle QuantityStyle { get; set; }
		public string QuantityStyleStr { get { return QuantityStyle.ToString(); } }

		public ProductRevenueRecurringStyle RecurringStyle { get; set; }
		public string RecurringStyleStr { get { return RecurringStyle.ToString(); } }

		public decimal UnitPrice { get; set; }
		public decimal Quantity { get; set; }

		public decimal LineDiscount { get; set; }
		public decimal ComputedLinePrice { get
			{
				return (UnitPrice * Quantity) - LineDiscount;
			} 
		}

		public string SortKey { get; set; }
		
	}

	public class WorkOrder
	{
		
		public string WorkOrderId { get; set; }
		public Customer Customer { get; set; }
		public List<WorkOrderItem> Items { get; set; }
		public string TermsAndConditions { get; set; }
		public List<Product> Products { get; set; }
		public DealInformation DealInfo { get; set; }
		public DealSummary DealSummary { get; set; }

		public static WorkOrder GetSampleWorkOrder()
		{
			var Customer = new Customer
			{
				Name = "Software Distributors",
				Address = "456 Broadway St",
				City = "Anytown",
				State = "CA",
				ZipCode = "12345",
				Email = "john.doe@example.com"
			};

			var DealInfo = new DealInformation
			{
				Currency = "USD",
				StartDate = new DateTime(2023, 4, 1),
				EndDate = new DateTime(2023, 3, 31),
				Term = "1 Year Agreement",
				SalesPerson = "Brian Murphy",
				UserCount = 23
			};

			var items = new List<WorkOrderItem>();

			items.Add(new WorkOrderItem
			{
				ProductName = "iMIS EMS Enterprise",
				Quantity = 23M,
				 QuantityStyle = ProductQuantityStyle.ByUserCount,
				 RevenueStyle = ProductRevenueStyle.QuantityBased,
				 RecurringStyle = ProductRevenueRecurringStyle.Recurring,
				 SortKey = "00_IMIS__01",
				 LineDiscount = 0M,
				 UnitPrice = 230.0M			
			});

			items.Add(new WorkOrderItem
			{
				ProductName = "iMIS Campaign Management",
				Quantity = 23M,
				QuantityStyle = ProductQuantityStyle.ByUserCount,
				RevenueStyle = ProductRevenueStyle.QuantityBased,
				RecurringStyle = ProductRevenueRecurringStyle.Recurring,
				SortKey = "00_IMIS__02",
				LineDiscount = -150M,
				UnitPrice = 63.0M
			});



			items.Add(new WorkOrderItem
			{
				ProductName = "OpenWater Software Platform Fee",
				Quantity = 1M,
				QuantityStyle = ProductQuantityStyle.Normal,
				RevenueStyle = ProductRevenueStyle.QuantityBased,
				RecurringStyle = ProductRevenueRecurringStyle.Recurring,
				SortKey = "10_OPENWATER_001",
				LineDiscount = 0M,
				UnitPrice = 4950
			});

			items.Add(new WorkOrderItem
			{
				ProductName = "OpenWater Software Per Program Fee",
				Quantity = 2M,
				QuantityStyle = ProductQuantityStyle.Normal,
				RevenueStyle = ProductRevenueStyle.QuantityBased,
				RecurringStyle = ProductRevenueRecurringStyle.Recurring,
				SortKey = "10_OPENWATER_002",
				LineDiscount = -650M,
				UnitPrice = 1250
			});

			items.Add(new WorkOrderItem
			{
				ProductName = "Integrations Hub",
				Quantity = 2M,
				QuantityStyle = ProductQuantityStyle.Normal,
				RevenueStyle = ProductRevenueStyle.Fixed,
				RecurringStyle = ProductRevenueRecurringStyle.Recurring,
				SortKey = "10_OPENWATER_003",
				LineDiscount = 0M,
				UnitPrice = 1250
			});

			items.Add(new WorkOrderItem
			{
				ProductName = "Consulting Project",
				Quantity = 1M,
				QuantityStyle = ProductQuantityStyle.Normal,
				RevenueStyle = ProductRevenueStyle.Fixed,
				RecurringStyle = ProductRevenueRecurringStyle.OneTime,
				SortKey = "10_OPENWATER_004",
				LineDiscount = 0M,
				UnitPrice = 25000
			});

			//summarize deal
			var dealSummary = new DealSummary();
			dealSummary.TotalOneTime = items.Where(c=>c.RecurringStyle == ProductRevenueRecurringStyle.OneTime).Sum(c => c.ComputedLinePrice);
			dealSummary.TotalRecurring= items.Where(c => c.RecurringStyle == ProductRevenueRecurringStyle.Recurring).Sum(c => c.ComputedLinePrice);

			if (DealInfo.UserCount > 0)
			{
				var totalUserSpecificCharges = items.Where(c => c.QuantityStyle == ProductQuantityStyle.ByUserCount).Sum(c => c.ComputedLinePrice);
				dealSummary.TotalRecurringForUserSpecificItems = totalUserSpecificCharges / DealInfo.UserCount;
			}



			var products = new List<Product>();
			products.Add(new Product { ProductName = "iMIS EMS Enterprise", Description = "Description of Product" });
			products.Add(new Product { ProductName = "iMIS Campaign Management", Description = "Description of Product" });
			products.Add(new Product { ProductName = "OpenWater Software Platform Fee", Description = "Description of Product" });
			products.Add(new Product { ProductName = "OpenWater Software Per Program Fee", Description = "Description of Product" });
			products.Add(new Product { ProductName = "Integrations Hub", Description = "Description of Product" });
			products.Add(new Product { ProductName = "Consulting Project", Description = "Description of Product" });




			var workOrder = new WorkOrder
			{				
				DealInfo = DealInfo,
				Customer = Customer,
				Items = items.OrderBy(s=>s.SortKey).ToList(),
				TermsAndConditions = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed non risus. Suspendisse lectus tortor, dignissim sit amet, adipiscing nec, ultricies sed, dolor. Cras elementum ultrices diam. Maecenas ligula massa, varius a, semper congue, euismod non, mi. Proin porttitor, orci nec nonummy molestie, enim est eleifend mi, non fermentum diam nisl sit amet erat.",
				Products = products,
				DealSummary = dealSummary
			};

			return workOrder;
		}
	}

	public class DealSummary
	{
		public decimal TotalRecurring { get; set; }
		public decimal TotalOneTime { get; set; }
		public decimal ManualTotalDiscount { get; set; }

		public decimal Total { get { return TotalOneTime + TotalRecurring - ManualTotalDiscount; } }

		public decimal TotalRecurringForUserSpecificItems { get; set; }
	}

	public class Product
	{
		public string ProductName { get; set; }
		public string Description { get; set; }
	}

	public class DealInformation
	{
		public DateTime StartDate { get; set; }
		public string Term { get; set; }
		public DateTime EndDate { get; set; }

		public string Currency { get; set; }

		public string SalesPerson { get; set; }
	public int UserCount { get; set; }
}

	public enum ProductRevenueStyle
	{
		QuantityBased,
		Fixed
	}

	public enum ProductRevenueRecurringStyle
	{
		OneTime,
		Recurring
	}

	public enum ProductQuantityStyle
	{
		Normal,
		ByUserCount
	}
}
