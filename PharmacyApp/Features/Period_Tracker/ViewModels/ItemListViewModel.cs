using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.Features.Period_Tracker.ViewModels
{
    public class ItemListViewModel
    {
        public List<ItemViewModel> Items { get; set; }

        public ItemListViewModel()
        {
            Items = new List<ItemViewModel>();
        }
    }
}
