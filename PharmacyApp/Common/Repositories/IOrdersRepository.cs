using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmacyApp.Models;

namespace PharmacyApp.Common.Repositories
{
    internal interface IOrdersRepository
    {
        void AddOrder(Order newOrder);
        void RemoveOrder(string idToBeRemoved);
        Order GetOrder(string id);
        List<Order> GetOrdersOfClient(string clientId);
        void UpdateOrder(string id, Order newOrder);
        bool OrderExists(string id);
    }
}
