using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmacyApp.Common.Repositories;

namespace PharmacyApp.Features.Period_Tracker.ViewModels
{
    public class PeriodTrackerViewModel
    {
        public IUsersRepository UsersRepository { get; set; } = new SQLUsersRepository();

        public PeriodTrackerViewModel() {}



    }

    public class Calendar1Converter : IValueConverter
    {
        Dictionary<DateTimeOffset, string> SpecialDates;
        public Calendar1Converter()
        {
            SpecialDates = new Dictionary<DateTimeOffset, string>();
            SpecialDates.Add(DateTimeOffset.Now.AddMonths(-1).AddDays(1), "SingleEvent_1");
            SpecialDates.Add(DateTimeOffset.Now.AddMonths(-1).AddDays(5), "DoubleEvent_1");
            SpecialDates.Add(DateTimeOffset.Now.AddMonths(-1).AddDays(-2), "TripleEvent_2");
            SpecialDates.Add(DateTimeOffset.Now.AddDays(1), "TripleEvent_1");
            SpecialDates.Add(DateTimeOffset.Now.AddDays(5), "SingleEvent_2");
            SpecialDates.Add(DateTimeOffset.Now.AddDays(7), "DoubleEvent_2");
            SpecialDates.Add(DateTimeOffset.Now.AddDays(9), "SingleEvent_1");
            SpecialDates.Add(DateTimeOffset.Now.AddDays(12), "TripleEvent_2");
            SpecialDates.Add(DateTimeOffset.Now.AddDays(-4), "DoubleEvent_1");
            SpecialDates.Add(DateTimeOffset.Now.AddMonths(1).AddDays(1), "DoubleEvent_3");
            SpecialDates.Add(DateTimeOffset.Now.AddMonths(1).AddDays(3), "SingleEvent_2");
            SpecialDates.Add(DateTimeOffset.Now.AddMonths(1).AddDays(-5), "DoubleEvent_2");
        }
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            DateTimeOffset dateTimeOffset = SpecialDates.Keys.FirstOrDefault(x => x.Date == (DateTime)value);

            if (dateTimeOffset != DateTimeOffset.MinValue) // MinValue is the default value
            {
                string template = SpecialDates[dateTimeOffset];
                StackPanel stackPanel;
                switch (template)
                {
                    case "SingleEvent_1":
                        return new List<Brush>() { new SolidColorBrush(Colors.DeepPink) };
                    case "SingleEvent_2":
                        return new List<Brush>() { new SolidColorBrush(Colors.Cyan) };
                    case "DoubleEvent_1":
                        return new List<Brush>() { new SolidColorBrush(Colors.Violet), new SolidColorBrush(Colors.Orange) };
                    case "DoubleEvent_2":
                        return new List<Brush>() { new SolidColorBrush(Colors.Gold), new SolidColorBrush(Colors.Green) };
                    case "DoubleEvent_3":
                        return new List<Brush>() { new SolidColorBrush(Colors.Brown), new SolidColorBrush(Colors.Blue) };
                    case "TripleEvent_1":
                        return new List<Brush>() { new SolidColorBrush(Colors.Green), new SolidColorBrush(Colors.DeepSkyBlue), new SolidColorBrush(Colors.Orange) };
                    case "TripleEvent_2":
                        return new List<Brush>() { new SolidColorBrush(Colors.Red), new SolidColorBrush(Colors.Green), new SolidColorBrush(Colors.Gold) };
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }

    public class Calendar2Converter : IValueConverter
    {
        Dictionary<DateTimeOffset, string> SpecialDates;
        public Calendar2Converter()
        {
            SpecialDates = new Dictionary<DateTimeOffset, string>();
            SpecialDates.Add(DateTimeOffset.Now.AddMonths(-1).AddDays(1), "SingleEvent_1");
            SpecialDates.Add(DateTimeOffset.Now.AddMonths(-1).AddDays(5), "DoubleEvent_1");
            SpecialDates.Add(DateTimeOffset.Now.AddMonths(-1).AddDays(-2), "TripleEvent_2");
            SpecialDates.Add(DateTimeOffset.Now.AddDays(1), "TripleEvent_1");
            SpecialDates.Add(DateTimeOffset.Now.AddDays(5), "SingleEvent_2");
            SpecialDates.Add(DateTimeOffset.Now.AddDays(7), "DoubleEvent_2");
            SpecialDates.Add(DateTimeOffset.Now.AddDays(9), "SingleEvent_1");
            SpecialDates.Add(DateTimeOffset.Now.AddDays(12), "TripleEvent_2");
            SpecialDates.Add(DateTimeOffset.Now.AddDays(-4), "DoubleEvent_1");
            SpecialDates.Add(DateTimeOffset.Now.AddMonths(1).AddDays(1), "DoubleEvent_3");
            SpecialDates.Add(DateTimeOffset.Now.AddMonths(1).AddDays(3), "SingleEvent_2");
            SpecialDates.Add(DateTimeOffset.Now.AddMonths(1).AddDays(-5), "DoubleEvent_2");
        }
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            DateTimeOffset dateTimeOffset = SpecialDates.Keys.FirstOrDefault(x => x.Date == (DateTime)value);

            if (dateTimeOffset != DateTimeOffset.MinValue) // MinValue is the default value
            {
                string template = SpecialDates[dateTimeOffset];
                StackPanel stackPanel;
                switch (template)
                {
                    case "SingleEvent_1":
                        return new List<Brush>() { new SolidColorBrush(Colors.DeepPink) };
                    case "SingleEvent_2":
                        return new List<Brush>() { new SolidColorBrush(Colors.Cyan) };
                    case "DoubleEvent_1":
                        return new List<Brush>() { new SolidColorBrush(Colors.Violet), new SolidColorBrush(Colors.Orange) };
                    case "DoubleEvent_2":
                        return new List<Brush>() { new SolidColorBrush(Colors.Gold), new SolidColorBrush(Colors.Green) };
                    case "DoubleEvent_3":
                        return new List<Brush>() { new SolidColorBrush(Colors.Brown), new SolidColorBrush(Colors.Blue) };
                    case "TripleEvent_1":
                        return new List<Brush>() { new SolidColorBrush(Colors.Green), new SolidColorBrush(Colors.DeepSkyBlue), new SolidColorBrush(Colors.Orange) };
                    case "TripleEvent_2":
                        return new List<Brush>() { new SolidColorBrush(Colors.Red), new SolidColorBrush(Colors.Green), new SolidColorBrush(Colors.Gold) };
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }

    public class Calendar3Converter : IValueConverter
    {
        Dictionary<DateTimeOffset, string> SpecialDates;
        public Calendar3Converter()
        {
            SpecialDates = new Dictionary<DateTimeOffset, string>();
            SpecialDates.Add(DateTimeOffset.Now.AddMonths(-1).AddDays(1), "SingleEvent_1");
            SpecialDates.Add(DateTimeOffset.Now.AddMonths(-1).AddDays(5), "DoubleEvent_1");
            SpecialDates.Add(DateTimeOffset.Now.AddMonths(-1).AddDays(-2), "TripleEvent_2");
            SpecialDates.Add(DateTimeOffset.Now.AddDays(1), "TripleEvent_1");
            SpecialDates.Add(DateTimeOffset.Now.AddDays(5), "SingleEvent_2");
            SpecialDates.Add(DateTimeOffset.Now.AddDays(7), "DoubleEvent_2");
            SpecialDates.Add(DateTimeOffset.Now.AddDays(9), "SingleEvent_1");
            SpecialDates.Add(DateTimeOffset.Now.AddDays(12), "TripleEvent_2");
            SpecialDates.Add(DateTimeOffset.Now.AddDays(-4), "DoubleEvent_1");
            SpecialDates.Add(DateTimeOffset.Now.AddMonths(1).AddDays(1), "DoubleEvent_3");
            SpecialDates.Add(DateTimeOffset.Now.AddMonths(1).AddDays(3), "SingleEvent_2");
            SpecialDates.Add(DateTimeOffset.Now.AddMonths(1).AddDays(-5), "DoubleEvent_2");
        }
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            DateTimeOffset dateTimeOffset = SpecialDates.Keys.FirstOrDefault(x => x.Date == (DateTime)value);

            if (dateTimeOffset != DateTimeOffset.MinValue) // MinValue is the default value
            {
                string template = SpecialDates[dateTimeOffset];
                StackPanel stackPanel;
                switch (template)
                {
                    case "SingleEvent_1":
                        return new List<Brush>() { new SolidColorBrush(Colors.DeepPink) };
                    case "SingleEvent_2":
                        return new List<Brush>() { new SolidColorBrush(Colors.Cyan) };
                    case "DoubleEvent_1":
                        return new List<Brush>() { new SolidColorBrush(Colors.Violet), new SolidColorBrush(Colors.Orange) };
                    case "DoubleEvent_2":
                        return new List<Brush>() { new SolidColorBrush(Colors.Gold), new SolidColorBrush(Colors.Green) };
                    case "DoubleEvent_3":
                        return new List<Brush>() { new SolidColorBrush(Colors.Brown), new SolidColorBrush(Colors.Blue) };
                    case "TripleEvent_1":
                        return new List<Brush>() { new SolidColorBrush(Colors.Green), new SolidColorBrush(Colors.DeepSkyBlue), new SolidColorBrush(Colors.Orange) };
                    case "TripleEvent_2":
                        return new List<Brush>() { new SolidColorBrush(Colors.Red), new SolidColorBrush(Colors.Green), new SolidColorBrush(Colors.Gold) };
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}

