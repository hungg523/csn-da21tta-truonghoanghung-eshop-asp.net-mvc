using HungApple.Data;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using HungApple.Infrastructure;

namespace HungApple.Models
{
	public class Cart
	{
		public List<CartLine> Lines { get; set; } = new List<CartLine>();

		public void AddItem(Product product, int quantity)
		{
			CartLine? line = Lines
			.Where(p => p.Product.Id == product.Id)
			.FirstOrDefault();
			if (line == null)
			{
				Lines.Add(new CartLine
				{
					Product = product,
					Quantity = quantity
				});
			}
			else
			{
				line.Quantity += quantity;
			}
		}

		public void RemoveLine(Product product) => Lines.RemoveAll(l => l.Product.Id == product.Id);

		public decimal ComputeTotalValue() => Lines.Sum(e => (e.Product.Price * e.Quantity) - (e.Quantity * e.Product.PriceDiscount));

		public void Clear() => Lines.Clear();
	}
	public class CartLine
	{
		public int CartLineId { get; set; }
		public Product Product { get; set; } = new();
		public int Quantity { get; set; }
	}
}
