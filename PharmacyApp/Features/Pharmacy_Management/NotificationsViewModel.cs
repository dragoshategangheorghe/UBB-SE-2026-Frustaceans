using PharmacyApp.Common.Repositories;
using PharmacyApp.Features.Accounts.Logic;
using PharmacyApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace PharmacyApp.Features.Pharmacy_Management
{
    public class NotificationsViewModel
    {
        public List<NotificationViewModel> Notifications { get; set; }

        public NotificationsViewModel()
        {
            Notifications = new List<NotificationViewModel>();
        }

        public void PopulateNotifications()
        {
            IItemsRepository itemsRepository = new SQLItemsRepository();

            // admin expired notifications
            if (ServiceWrapper.UserAccountService.CurrentUser.IsAdmin)
            {
                List<Item> items = itemsRepository.GetAllItems();
                foreach (Item item in items)
                {
                    foreach (KeyValuePair<DateOnly, int> batch in item.Batches)
                    {
                        if (new DateTime(batch.Key.Year, batch.Key.Month, batch.Key.Day) <= DateTime.Today)
                        {
                            // literally expired <= today
                            Notifications.Add(new NotificationViewModel("Expired Items", $"{item.Name} has an expired batch", "Go fix it"));
                            break; // break for
                        }
                    }
                }
            }


            // user stock alert notifications
            foreach (int itemId in ServiceWrapper.UserAccountService.CurrentUser.StockAlerts)
            {
                Item item = itemsRepository.GetItem(itemId);

                foreach (KeyValuePair<DateOnly, int> batch in item.Batches)
                {
                    if (new DateTime(batch.Key.Year, batch.Key.Month, batch.Key.Day) > DateTime.Today)
                    {
                        //  found one that is NOT expired, add it 
                        Notifications.Add(new NotificationViewModel("Stock Alert", $"{item.Name} is in stock", "Go to products"));
                        break;
                    }
                }
                    
            }

        }
    }
}
