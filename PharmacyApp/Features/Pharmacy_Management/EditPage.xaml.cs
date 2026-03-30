using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PharmacyApp.Common.Repositories;
using PharmacyApp.Common.Services;
using PharmacyApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PharmacyApp.Features.Pharmacy_Management
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditPage : Page
    {
        private AdminService adminService;
        private IItemsRepository itemsRepository;
        private ISubstancesRepository substancesRepository;

        //helper classes -------------------------------------------------
        public class ActiveSubstance
        {
            public string Name { get; set; }
            public float Concentration { get; set; }
        }
        public Dictionary<string, float> ActiveSubstancesDict { get; set; } = new Dictionary<string, float>();

        public class BatchItem
        {
            public DateOnly Date { get; set; }
            public int Packs { get; set; }
        }
        public Dictionary<DateOnly, int> BatchesDict { get; set; } = new Dictionary<DateOnly, int>();
        public EditPage()
        {
            InitializeComponent();
            itemsRepository = new SQLItemsRepository();
            //itemsRepository.GetAllItems();
            substancesRepository = new SQLSubstancesRepository();
            

            adminService = new AdminService(itemsRepository, substancesRepository);

            SubstanceListButtons.Visibility = Visibility.Collapsed;
            SubstanceBottomButtons.Visibility = Visibility.Collapsed;
            AddSubstanceGrid.Visibility = Visibility.Collapsed;
            UpdateSubstanceGrid.Visibility = Visibility.Collapsed;
        }

        //search box -------------------------------------------------------------------
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string query = SearchBox.Text.ToLower();

            var filtered = itemsRepository.GetAllItems()
                .Where(p => p.Name.ToLower().Contains(query))
                .ToList();

            ItemList.ItemsSource = filtered;
        }

        //private void SearchBox_TextChangedSubstance(object sender, TextChangedEventArgs e)
        //{
        //    string query = SearchBox.Text.ToLower();

        //    var filtered = substancesRepository.GetAllItems()
        //        .Where(p => p.Name.ToLower().Contains(query))
        //        .ToList();

        //    ItemList.ItemsSource = filtered;
        //}

        private void OnItemClick(object sender, RoutedEventArgs e)
        {
            //]       ItemsGrid.Visibility = Visibility.Collapsed;
            ItemListButtons.Visibility = Visibility.Visible;
            ItemBottomButtons.Visibility = Visibility.Visible;


            SubstanceListButtons.Visibility = Visibility.Collapsed;
            SubstanceBottomButtons.Visibility = Visibility.Collapsed;
            AddSubstanceGrid.Visibility = Visibility.Collapsed;
            UpdateSubstanceGrid.Visibility = Visibility.Collapsed;

            //TODO: SUBSTANCE GRID AND ALL OTHER UPDATE/ADD GRID
        }

        private void OnSubstancesClick(object sender, RoutedEventArgs e)
        {
            ItemListButtons.Visibility = Visibility.Collapsed;
            ItemBottomButtons.Visibility = Visibility.Collapsed;
            AddItemGrid.Visibility = Visibility.Collapsed;
            UpdateItemGrid.Visibility = Visibility.Collapsed;

            SubstanceListButtons.Visibility = Visibility.Visible;
            SubstanceBottomButtons.Visibility = Visibility.Visible;
        }

        private void clearItemAddBoxes()
        {
            NameBox.Text = string.Empty;
            ProducerBox.Text = string.Empty;
            PriceBox.Text = string.Empty;
            CategoryBox.Text = string.Empty;
            ImagePathBox.Text = string.Empty;
            NumberOfPillsBox.Text = string.Empty;
            QuantityBox.Text = string.Empty;
            LabelBox.Text = string.Empty;
            DescriptionBox.Text = string.Empty;
            DiscountBox.Text = string.Empty;
            SubstanceNameBox.Text = string.Empty;
            PacksBox.Text = string.Empty;


        }

        private void clearItemUpdateBoxes()
        {
            IdBox.Text = string.Empty;
            NameBoxUpdate.Text = string.Empty;
            ProducerBoxUpdate.Text = string.Empty;
            PriceBoxUpdate.Text = string.Empty;
            CategoryBoxUpdate.Text = string.Empty;
            ImagePathBoxUpdate.Text = string.Empty;
            NumberOfPillsBoxUpdate.Text = string.Empty;
            QuantityBoxUpdate.Text = string.Empty;
            LabelBoxUpdate.Text = string.Empty;
            DescriptionBoxUpdate.Text = string.Empty;
            DiscountBoxUpdate.Text = string.Empty;
            SubstanceNameBoxUpdate.Text = string.Empty;
            PacksBoxUpdate.Text = string.Empty;
        }

        private void clearSubstanceBoxes()
        {
            NameBoxSubstance.Text = string.Empty;
            LethalDoseBoxSubstance.Text = string.Empty;
            DescriptionBoxSubstance.Text = string.Empty;
        }

        private void clearSubstanceUpdateBoxes()
        {
            NameBoxSubstanceUpdate.Text = string.Empty;
            LethalDoseBoxSubstanceUpdate.Text = string.Empty;
            DescriptionBoxSubstanceUpdate.Text = string.Empty;
        }

        private void OnItemAddClick(object sender, RoutedEventArgs e)
        {

            if (AddItemGrid.Visibility == Visibility.Visible)
            {
                AddItemGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                AddItemGrid.Visibility = Visibility.Visible;
                UpdateItemGrid.Visibility = Visibility.Collapsed;
            }
        }
        private void OnAddItemClick(object sender, RoutedEventArgs e)
        {
            if (!float.TryParse(PriceBox.Text, out float price))
            {
                throw new ArgumentException("Invalid format");
            }

            if (!int.TryParse(NumberOfPillsBox.Text, out int numberOfPills))
            {
                throw new ArgumentException("Invalid format");
            }

            if (!int.TryParse(QuantityBox.Text, out int quantity))
            {
                throw new ArgumentException("Invalid format");
            }

            if (!float.TryParse(DiscountBox.Text, out float discount))
            {
                throw new ArgumentException("Invalid format");
            }

            string name = NameBox.Text;
            string producer = ProducerBox.Text;
            string category = CategoryBox.Text;
            string imagePath = ImagePathBox.Text;
            string label = LabelBox.Text;
            string description = DescriptionBox.Text;

            //active substance add
            if (ActiveSubstancesDict.Count == 0)
            {
                throw new ArgumentException("At least one active substance is required");
            }

            //id should not be in the constructorrrr + need the lists of active substances and batches
            //Item newItem = new Item(name, producer, category, price, numberOfPills, quantity, label, description, imagePath, discount, ActiveSubstancesDict, BatchesDict);

            Item newItem = new Item(name, producer, category, price, numberOfPills, BatchesDict, ActiveSubstancesDict, label, description, imagePath, discount);


            adminService.AddItem(newItem);
            System.Diagnostics.Debug.WriteLine("Added item");

            clearItemAddBoxes();
            ActiveSubstancesDict.Clear();
            BatchesDict.Clear();
        }

        private void RefreshActiveSubstancesList()
        {
            var list = ActiveSubstancesDict
                .Select(kvp => new ActiveSubstance
                {
                    Name = kvp.Key,
                    Concentration = kvp.Value
                })
                .ToList();

            ActiveSubstancesList.ItemsSource = list;
        }

        private void AddSubstance_Click(object sender, RoutedEventArgs e)
        {
            string name = SubstanceNameBox.Text;

            if (!float.TryParse(ConcentrationBox.Text, out float concentration))
                throw new ArgumentException("invalid");

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("invalid");

            ActiveSubstancesDict[name] = concentration;


            RefreshActiveSubstancesList();


            SubstanceNameBox.Text = string.Empty;
            ConcentrationBox.Text = string.Empty;
        }

        private void RemoveSubstance_Click(object sender, RoutedEventArgs e)
        {
            string name = SubstanceNameBox.Text;

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("invalid");

            if (ActiveSubstancesDict.ContainsKey(name))
            {
                ActiveSubstancesDict.Remove(name);
            }

            // Refresh
            RefreshActiveSubstancesList();


            SubstanceNameBox.Text = string.Empty;
            //ConcentrationBox.Text = string.Empty;
        }

        //batches
        private void RefreshBatchesList()
        {
            var list = BatchesDict
                .Select(kvp => new BatchItem
                {
                    Date = kvp.Key,
                    Packs = kvp.Value
                })
                .OrderBy(x => x.Date)
                .ToList();

            BatchesList.ItemsSource = list;
        }

        private void AddBatch_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(PacksBox.Text, out int packs))
                throw new ArgumentException("invalid");

            // Convert DatePicker.Date (DateTimeOffset) to DateOnly
            DateOnly date = DateOnly.FromDateTime(BatchDatePicker.Date.Date);

            BatchesDict[date] = packs;

            RefreshBatchesList();

            PacksBox.Text = string.Empty;
        }

        private void RemoveBatch_Click(object sender, RoutedEventArgs e) {

            var selectedBatch = BatchesList.SelectedItem as BatchItem;
   
            if (BatchesDict.ContainsKey(selectedBatch.Date))
            {
                BatchesDict.Remove(selectedBatch.Date);

                RefreshBatchesList();
            }
        }

        private void OnItemCancelClick(object sender, RoutedEventArgs e)
        {
            clearItemAddBoxes();
            clearItemUpdateBoxes();
            AddItemGrid.Visibility = Visibility.Collapsed;
            UpdateItemGrid.Visibility = Visibility.Collapsed;
        }

        private void OnItemRemoveClick(object sender, RoutedEventArgs e)
        {
            var selectedItem = ItemList.SelectedItem as Item;
            if (selectedItem == null)
                throw new ArgumentException("not selected"); // Nothing selected

            int id = selectedItem.Id;

            // Call admin service to remove
            adminService.RemoveItem(id);

        }

        private void OnItemUpdateClick(object sender, RoutedEventArgs e)
        {

            if (UpdateItemGrid.Visibility == Visibility.Visible)
            {
                UpdateItemGrid.Visibility = Visibility.Collapsed;

            }
            else
            {
                UpdateItemGrid.Visibility = Visibility.Visible;
                AddItemGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void OnUpdateItemClick(object sender, RoutedEventArgs e)
        //TODO everything. i cant .
        {
            Item itemToBeUpdated = itemsRepository.GetItem(int.Parse(IdBox.Text));
            if (!int.TryParse(IdBox.Text, out int id))
            {
                throw new ArgumentException("Invalid format");
            }

            if (!float.TryParse(PriceBoxUpdate.Text, out float price))
            {
                price = itemToBeUpdated.Price;
            }

            if (!int.TryParse(NumberOfPillsBoxUpdate.Text, out int numberOfPills))
            {
                numberOfPills = itemToBeUpdated.NumberOfPills;
            }

            if (!int.TryParse(QuantityBoxUpdate.Text, out int quantity))
            {
                quantity = itemToBeUpdated.Quantity;
            }

            if (!float.TryParse(DiscountBoxUpdate.Text, out float discount))
            {
                discount = itemToBeUpdated.DiscountPercentage;
            }

            // Strings: replace if empty
            string name = string.IsNullOrWhiteSpace(NameBoxUpdate.Text)
                ? itemToBeUpdated.Name
                : NameBoxUpdate.Text;

            string producer = string.IsNullOrWhiteSpace(ProducerBoxUpdate.Text)
                ? itemToBeUpdated.Producer
                : ProducerBoxUpdate.Text;

            string category = string.IsNullOrWhiteSpace(CategoryBoxUpdate.Text)
                ? itemToBeUpdated.Category
                : CategoryBoxUpdate.Text;

            string imagePath = string.IsNullOrWhiteSpace(ImagePathBoxUpdate.Text)
                ? itemToBeUpdated.ImagePath
                : ImagePathBoxUpdate.Text;

            string label = string.IsNullOrWhiteSpace(LabelBoxUpdate.Text)
                ? itemToBeUpdated.Label
                : LabelBoxUpdate.Text;

            string description = string.IsNullOrWhiteSpace(DescriptionBoxUpdate.Text)
                ? itemToBeUpdated.Description
                : DescriptionBoxUpdate.Text;

            adminService.UpdateItem(id, new Item(name, producer, category, price, numberOfPills, BatchesDict, ActiveSubstancesDict, label, description, imagePath, discount));
            System.Diagnostics.Debug.WriteLine("Added item");

            clearItemAddBoxes();
            ActiveSubstancesDict.Clear();
            BatchesDict.Clear();
        }


        private void OnSubstanceAddClick(object sender, RoutedEventArgs e)
        {

            if (AddSubstanceGrid.Visibility == Visibility.Visible)
            {
                AddSubstanceGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                AddSubstanceGrid.Visibility = Visibility.Visible;
                UpdateSubstanceGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void OnSubstanceUpdateClick(object sender, RoutedEventArgs e)
        {

            if (UpdateSubstanceGrid.Visibility == Visibility.Visible)
            {
                UpdateSubstanceGrid.Visibility = Visibility.Collapsed;

            }
            else
            {
                UpdateSubstanceGrid.Visibility = Visibility.Visible;
                AddSubstanceGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void OnSubstanceRemoveClick(object sender, RoutedEventArgs e)
        {
            var selectedItem = SubstanceList.SelectedItem as Substance;
            if (selectedItem == null)
                throw new ArgumentException("not selected"); // Nothing selected

            //string name = selectedItem.Name;

            // Call admin service to remove
            adminService.RemoveSubstance(selectedItem);

        }

        private void OnSubstanceCancelClick(object sender, RoutedEventArgs e)
        {
            clearSubstanceBoxes();
            clearSubstanceUpdateBoxes();
            AddSubstanceGrid.Visibility = Visibility.Collapsed;
            UpdateSubstanceGrid.Visibility = Visibility.Collapsed;
        }

        private void OnAddSubstanceClick(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(LethalDoseBoxSubstance.Text, out int lethalDose))
            {
                throw new ArgumentException("Invalid format");
            }

            string name = NameBoxSubstance.Text;
            //int lethalDose = int.Parse(LethalDoseBox.Text);
            string description = DescriptionBoxSubstance.Text;

            Substance newSubstance = new Substance(name, lethalDose, description);


            //AdminService.Add(...)
            System.Diagnostics.Debug.WriteLine("Added substance");

            clearSubstanceBoxes();
        }

        private void OnUpdateSubstanceClick(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(LethalDoseBoxSubstanceUpdate.Text, out int lethalDose))
            {
                throw new ArgumentException("Invalid format");
            }

            string name = NameBoxSubstanceUpdate.Text;
            //int lethalDose = int.Parse(LethalDoseBox.Text);
            string description = DescriptionBoxSubstanceUpdate.Text;

            Substance updatedSubstance = new Substance(name, lethalDose, description);


            //AdminService.Add(...)
            System.Diagnostics.Debug.WriteLine("Updated substance");

            clearSubstanceBoxes();
        }

    }
}


