using System.Collections.Generic;
using System.Linq;

namespace MvpVendingMachineApp.Common.Utilities
{
    public class BusinessLogic
    {
        public static decimal[] CalculateChange(decimal amount)
        {
            var change = new List<decimal>();

            while (amount > 0)
            {

                if (amount - 100 >= 0)
                {
                    change.Add(100);

                    amount -= 100;
                }
                else if (amount - 50 >= 0)
                {
                    change.Add(50);
                    amount -= 50;
                }
                else if (amount - 20 >= 0)
                {
                    change.Add(20);
                    amount -= 20;
                }
                else if (amount - 10 >= 0)
                {
                    change.Add(10);
                    amount -= 10;
                }
                else if (amount - 5 >= 0)
                {
                    change.Add(5);
                    amount -= 5;
                }
            }
            return change.OrderByDescending(r => r).ToArray();

        }
    }
}
