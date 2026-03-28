using System;
using System.Collections.Generic;

namespace PharmacyApp.Models
{
    public class User
    {
        public int Id { get; private set; }
        public string Email {  get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public bool IsAdmin { get; set; }
        public string Username { get; set; }
        public bool IsDisabled { get; set; }



        public DateOnly StartPeriodDate { get; set; }
        public int CycleDays { get; set; }
        public int PeriodLasts { get; set; }
        public int PMSOption { get; set; }
        public bool WantsToBePregnant { get; set; }
        public Dictionary<int, Tuple<string, bool>> PeriodNotes { get; private set; }

        public List<int> StockAlerts { get; private set; }
        public List<int> FavoriteItems { get; private set; }
        public Dictionary<int, float> UserDiscounts { get; private set; }
        public Dictionary<int, int> Basket { get; private set; }

        public bool DiscountNotifications { get; set; }
        public int LoyaltyPoints { get; set; }



        public User(int id, string email, string phoneNumber,
                    string passwordHash, bool isAdmin, bool isDisabled,
                    string userName, bool discountNotifications,
                    int loyaltyPoints, DateOnly startPeriodDate= new DateOnly(),
                    int cycleDays=0, int periodLasts=0, int pmsOption=0,
                    bool wantsToBePregnant=false)
        {
            Id = id;
            Email = email;
            PhoneNumber = phoneNumber;
            PasswordHash = passwordHash;
            IsAdmin = isAdmin;
            IsDisabled = isDisabled;
            Username = userName;
            DiscountNotifications = discountNotifications;
            LoyaltyPoints = loyaltyPoints;
            StartPeriodDate = startPeriodDate;
            CycleDays = cycleDays;
            PeriodLasts = periodLasts;
            PMSOption = pmsOption;
            WantsToBePregnant = wantsToBePregnant;
            PeriodNotes = new Dictionary<int, Tuple<string, bool>>();
            StockAlerts = new List<int>();
            FavoriteItems = new List<int>();
            UserDiscounts = new Dictionary<int, float>();
            Basket = new Dictionary<int, int>();
        }


        public void AddStockAlert(int newItemId)
        {
            if (StockAlerts.Contains(newItemId))
                throw new ArgumentException("Item #" + newItemId + " already in stock alert");
            StockAlerts.Add(newItemId);
        }

        public void RemoveStockAlert(int itemIdToRemove)
        {
            if (!StockAlerts.Contains(itemIdToRemove))
                throw new ArgumentException("Item #" + itemIdToRemove + " not in stock alert");
            StockAlerts.Remove(itemIdToRemove);
        }


        public void AddFavoriteItem(int newItemId)
        {
            if (FavoriteItems.Contains(newItemId))
                throw new ArgumentException("Item #" + newItemId + " already in favorites");
            FavoriteItems.Add(newItemId);
        }

        public void RemoveFavoriteItem(int itemIdToRemove)
        {
            if (!FavoriteItems.Contains(itemIdToRemove))
                throw new ArgumentException("Item #" + itemIdToRemove + " not in favorites");
            FavoriteItems.Remove(itemIdToRemove);
        }


        public void AddUserDiscount(int newItemId, float discount)
        {
            if (UserDiscounts.ContainsKey(newItemId))
                throw new ArgumentException("Item #" + newItemId + " already has a discount on this user");
            UserDiscounts[newItemId] = discount;
        }

        public void ChangeUserDiscount(int itemId, float newDiscount)
        {
            if (!UserDiscounts.ContainsKey(itemId))
                throw new ArgumentException("Item #" + itemId + " doesn't have a discount on this user");
            UserDiscounts[itemId] = newDiscount;
        }

        public void RemoveUserDiscount(int itemIdToRemove)
        {
            if (!UserDiscounts.ContainsKey(itemIdToRemove))
                throw new ArgumentException("Item #" + itemIdToRemove + " doesn't have a discount on this user");
            UserDiscounts.Remove(itemIdToRemove);
        }


        public void AddItemToBasket(int newItemId, int quantityToGet)
        {
            if (Basket.ContainsKey(newItemId))
                throw new ArgumentException("Item #" + newItemId + " already in user's basket");
            Basket[newItemId] = quantityToGet;
        }

        public void ChangeItemInBasket(int itemId, int newQuantityToGet)
        {
            if (!Basket.ContainsKey(itemId))
                throw new ArgumentException("Item #" + itemId + " is not in user's basket");
            Basket[itemId] = newQuantityToGet;
        }

        public void RemoveItemFromBasket(int itemIdToRemove)
        {
            if (!Basket.ContainsKey(itemIdToRemove))
                throw new ArgumentException("Item #" + itemIdToRemove + " is not in user's basket");
            Basket.Remove(itemIdToRemove);
        }


        // Note to Alex: sry if the implementation is not good/absent
        // I'll leave it to you, cuz these functions won't be used elsewhere

        public void SetPeriodTracker(DateOnly startPeriodDate, int cycleDays,
                                     int periodLasts, int pmsOption, bool wantsToBePregnant)
        {
            StartPeriodDate = startPeriodDate;
            CycleDays = cycleDays;
            PeriodLasts = periodLasts;
            PMSOption = pmsOption;
            WantsToBePregnant = wantsToBePregnant;
        }

        public void AddPeriodNote(int noteId, string noteBody, bool isDone)
        {
            if (PeriodNotes.ContainsKey(noteId))
                throw new ArgumentException("Note #" + noteId + " is already exists");
            PeriodNotes[noteId] = new Tuple<string, bool>(noteBody, isDone);
        }
        public void RemovePeriodNote(int noteIdToRemove)
        {
            if (!PeriodNotes.ContainsKey(noteIdToRemove))
                throw new ArgumentException("Note #" + noteIdToRemove + " is not created");
            PeriodNotes.Remove(noteIdToRemove);
        }

    }
}
