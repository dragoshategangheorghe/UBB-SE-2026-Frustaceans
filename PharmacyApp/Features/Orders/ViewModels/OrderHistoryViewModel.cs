using PharmacyApp.Common.Commands;
using PharmacyApp.Features.Orders.Logic;
using PharmacyApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static PharmacyApp.Features.Orders.ViewModels.OrderManagementViewModel;

namespace PharmacyApp.Features.Orders.ViewModels
{
    class OrderHistoryViewModel
    {
        OrderService activeUserServ;
        public ICommand CancelCommand { get; private set; }
        public ICommand ResubmitCommand { get; private set; }
        public ICommand GoToDetailPageCommand { get; private set; }
        public ObservableCollection<Order> OrderHistory;

        public OrderHistoryViewModel(OrderService userService)
        {
            activeUserServ = userService;
            CancelCommand = new RelayCommandWithOneParameter<Order>(CancelOrder);
            ResubmitCommand = new RelayCommandWithOneParameter<Order>(ResubmitExpiredOrder);
            GoToDetailPageCommand = new RelayCommandWithOneParameter<Order>(DisplayOrderDetail);
            OrderHistory = new();

            int clientId = activeUserServ.ActiveUser.Id;
            List<Order> userOrders = activeUserServ.OrdersRepo.GetOrdersOfClient(clientId);
            foreach (Order currentOrder in userOrders)
            {
                OrderHistory.Add(currentOrder);
            }
        }

        private void CancelOrder(Order orderToCancel)
        {
            System.Diagnostics.Debug.WriteLine("Cancelled order #" + orderToCancel.Id);
        }

        private void ResubmitExpiredOrder(Order orderToResubmit)
        {
            System.Diagnostics.Debug.WriteLine("Resubmitted order #" + orderToResubmit.Id);
        }

        private void DisplayOrderDetail(Order orderToModify)
        {
            OnClickDetailButton(orderToModify);
        }


        public delegate void SelectedOrder(Tuple<OrderService, Order> args);

        public event SelectedOrder ClickDetailButton;

        public virtual void OnClickDetailButton(Order chosenOrder)
        {
            ClickDetailButton?.Invoke(new Tuple<OrderService, Order>(activeUserServ, chosenOrder));
        }
    }
}
