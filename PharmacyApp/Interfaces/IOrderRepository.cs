using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmacyApp.Models;

namespace PharmacyApp.Interfaces
{
    internal interface IOrderRepository
    {
        bool addOrder(Order newOrder);
        bool removeOrder(string idToBeRemoved);
        Order getOrder(string id);
        List<Order> getOrdersOfClient(string clientId);
        bool changeOrderInfo(string id, Order newOrder);
        bool checkOrderExists(string id);
    }
}
