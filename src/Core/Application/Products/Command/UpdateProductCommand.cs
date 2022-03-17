using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvpVendingMachineApp.Application.Products.Command
{
    public class UpdateProductCommand : IRequest<Unit>
    {
        public int ProductId { get; set; }
        public int SellerId { get; set; }
        public string ProductName { get; set; }

        public decimal? Cost { get; set; }

        public int? AmountAvailable { get; set; }
    }
}
