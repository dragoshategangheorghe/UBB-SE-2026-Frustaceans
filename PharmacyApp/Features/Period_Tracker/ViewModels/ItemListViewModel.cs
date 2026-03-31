using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PharmacyApp.Features.Orders.Logic;
using Syncfusion.UI.Xaml.Core;

namespace PharmacyApp.Features.Period_Tracker.ViewModels
{
    public class ItemListViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));
        }

        public ICommand AddItemToBasket { get; set; }

        private List<ItemViewModel> _items;

        public List<ItemViewModel> Items
        {
            get { return _items; }
            set { _items = value;
                OnPropertyChanged();
            }
        }

        public ItemListViewModel()
        {
            Items = new List<ItemViewModel>();
            AddItemToBasket = new DelegateCommand(OnAddItemToBasketCommand);
        }

        public void OnAddItemToBasketCommand(object obj)
        {
            int itemindex = int.Parse((string)obj);
            int itemId = Items[itemindex].Id;
            OrderService orderService = new OrderService();
            orderService.AddToBasket(itemId, 1);
            


        }
    }
}
