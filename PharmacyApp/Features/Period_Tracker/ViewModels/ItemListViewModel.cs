using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
        }
    }
}
