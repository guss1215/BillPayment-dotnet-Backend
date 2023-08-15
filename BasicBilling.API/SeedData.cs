using System;
using System.Collections.Generic;
using BasicBilling.API.Models; // Adjust the namespace

namespace BasicBilling.API
{
    public static class SeedData
    {
        public static List<Client> GetClients()
        {
            return new List<Client>
            {
                new Client { Id = 100, Name = "Joseph Carlton" },
                new Client { Id = 200, Name = "Maria Juarez" },
                new Client { Id = 300, Name = "Albert Kenny" },
                new Client { Id = 400, Name = "Jessica Phillips" },
                new Client { Id = 500, Name = "Charles Johnson" }
            };
        }

        public static List<Bill> GetBills(List<Client> clients)
        {
            var bills = new List<Bill>();
            var categories = new List<string> { "WATER", "ELECTRICITY", "SEWER" };
            var currentDate = DateTime.UtcNow.AddMonths(-2); // Start from 2 months ago

            foreach (var client in clients)
            {
                foreach (var category in categories)
                {
                    currentDate = DateTime.UtcNow.AddMonths(-2);
                    for (int i = 0; i < 2; i++) // Generate bills for 2 months
                    {
                        var bill = new Bill
                        {
                            ClientId = client.Id,
                            Category = category,
                            Period = int.Parse(currentDate.ToString("yyyyMM")), // Convert string to int
                            PaymentStatus = "Pending"
                        };
                        bills.Add(bill);

                        currentDate = currentDate.AddMonths(1);
                    }
                }
            }

            return bills;
        }

        public static void Initialize(BillingDbContext context)
        {
            //context.Database.Migrate(); // Apply any pending migrations

            if (!context.Clients.Any())
            {
                var clients = GetClients();
                context.Clients.AddRange(clients);
                context.SaveChanges();
            }

            //ClearBills(context);

            if (!context.Bills.Any())
            {
                var clients = context.Clients.ToList(); // Fetch existing clients from the database
                var bills = GetBills(clients);
                context.Bills.AddRange(bills);
                context.SaveChanges();
            }
        }

        private static void ClearBills(BillingDbContext context)
        {
            context.Bills.RemoveRange(context.Bills);
            context.SaveChanges();
        }

        // Define similar method for generating payments
    }
}
