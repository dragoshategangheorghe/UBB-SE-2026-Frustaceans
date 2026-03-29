using PharmacyApp.Features.Orders.Logic;
using PharmacyApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.Features.Orders.ViewModels
{

    public class OrderDetail
    {
        public int OrderID { get; set; }
        public string UserEmail { get; set; }
        public bool IsComplete { get; set; }
        public bool IsExpired { get; set; }
        public DateOnly PickUpDate { get; set; }
        // TODO MAGIC NUMBER DETECTED (why did it take me so long to realize)
        public DateOnly ExpirationDate { get { return PickUpDate.AddDays(7); } }

        public string OrderString { get { return "Order#" + OrderID; } }
        public string PickUpDateString { get { return PickUpDate.ToString("yyyy.MM.dd"); } }
        public string ExpirationDateString { get { return ExpirationDate.ToString("yyyy.MM.dd"); } }


        public OrderDetail(Order orderDetails, string userEmail)
        {
            OrderID = orderDetails.Id;
            UserEmail = userEmail;
            IsComplete = orderDetails.IsCompleted;
            IsExpired = orderDetails.IsExpired;
            PickUpDate = orderDetails.PickUpDate;
        }
    }


    public class OrderManagementViewModel : INotifyPropertyChanged
    {

        OrderService orderServ;

        List<OrderDetail> baseOrderList;
        public ObservableCollection<OrderDetail> FilteredOrderList { get; set; }

        string orderIDInput;
        string userEmailInput;
        bool isCompleteCheckbox;
        bool isExpiredCheckbox;

        // properties for the UI elements to modify
        public string OrderIDInput 
        {
            get { return orderIDInput; }
            set { 
                orderIDInput = value; 
                OnPropertyChanged();
                ReapplyFilters();
            }
        }
        public string UserEmailInput 
        { 
            get { return userEmailInput; }
            set { 
                userEmailInput = value; 
                OnPropertyChanged();
                ReapplyFilters();
            }
        }
        public bool IsCompleteCheckbox 
        { 
            get { return isCompleteCheckbox; }
            set { 
                isCompleteCheckbox = value; 
                OnPropertyChanged();
                ReapplyFilters();
            }
        }
        public bool IsExpiredCheckbox 
        { 
            get { return isExpiredCheckbox; }
            set { 
                isExpiredCheckbox = value; 
                OnPropertyChanged();
                ReapplyFilters();
            }
        }


        public OrderManagementViewModel(OrderService newOrderServ)
        {
            orderServ = newOrderServ;
            baseOrderList = new();
            FilteredOrderList = new();
            
            // TODO move this segment into service!
            foreach (Order currOrder in orderServ.OrdersRepo.GetAllOrders())
            {
                int userID = orderServ.OrdersRepo.GetOrder(currOrder.Id).ClientId;
                string currUserEmail = orderServ.UsersRepo.GetUserById(userID).Email;

                OrderDetail currOrderDetail = new(currOrder, currUserEmail);

                baseOrderList.Add(currOrderDetail);
                FilteredOrderList.Add(currOrderDetail);
            }
        }

        private void ReapplyFilters()
        {
            // TODO maybe not set the filtered order list
            // after each individual filter


        }


        // To handle the changing of the filtering properties automatically
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] String propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
