using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using PharmacyApp.Models;

namespace PharmacyApp.Features.Period_Tracker.ViewModels
{
    public class ItemViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value; 
                OnPropertyChanged();
            }
        }

        private float Price { get; set; }

        private string _priceString;
        public string PriceString
        {
            get { return _priceString; }
            set
            {
                _priceString = value;
                OnPropertyChanged();
            }
        }

        private string _priceDiscountedString;
        public string PriceDiscountedString
        {
            get { return _priceDiscountedString; }
            set
            {
                _priceDiscountedString = value;
                OnPropertyChanged();
            }
        }

        private string _priceColor;
        public string PriceColor
        {
            get { return _priceColor; }
            set
            {
                _priceColor = value;
                OnPropertyChanged();
            }
        }

        private string _imagePath;
        public string ImagePath {

            get { return _imagePath; }
            set
            {
                _imagePath = value;
                OnPropertyChanged();
            }
        }


        public ItemViewModel(Item item)
        {
            Name = item.Name;
            Price = item.Price * (1.0f - item.DiscountPercentage/100.0f);
            if (PeriodTrackerUser.CurrentUser.UserDiscounts.ContainsKey(item.Id)) //user has discount for this item
                Price *= (1.0f - PeriodTrackerUser.CurrentUser.UserDiscounts[item.Id]);

            if (Price != item.Price) // it is discounted
            {
                PriceDiscountedString = $"{item.Price}"; //the original price striked through
                PriceColor = "Red";
            }
            else
            {
                PriceDiscountedString = "";
                PriceColor = "Black";
            }


            PriceString = $"{Price} RON";

            
            ImagePath = $"ms-appx:///Assets/{item.ImagePath}";
            if (!File.Exists(ImagePath))
                ImagePath = "ms-appx:///Assets/placeholder.png";

        }
    }
}
