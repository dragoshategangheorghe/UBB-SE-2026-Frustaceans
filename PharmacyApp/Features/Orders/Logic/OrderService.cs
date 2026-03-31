using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using PharmacyApp.Common.Repositories;
using PharmacyApp.Features.Accounts.Logic;
using PharmacyApp.Models;

namespace PharmacyApp.Features.Orders.Logic
{
    public class OrderService
    {
        public ISubstancesRepository SubstancesRepo { get; private set; }
        public IItemsRepository ItemsRepo { get; private set; }
        public IUsersRepository UsersRepo { get; private set; }
        public IOrdersRepository OrdersRepo { get; private set; }
        public User ActiveUser { get { return ServiceWrapper.UserAccountService.CurrentUser; } }

        public OrderService()
        {
            SubstancesRepo = new SQLSubstancesRepository();
            ItemsRepo = new SQLItemsRepository();
            UsersRepo = new SQLUsersRepository();
            OrdersRepo = new SQLOrdersRepository();
        }

        public void AddToBasket(int itemId, int quantityToBuy)
        {
            ActiveUser.AddItemToBasket(itemId, quantityToBuy);
        }

        public void UpdateBasketItemQuantity(int itemId, int newQuantityToBuy)
        {
            ActiveUser.Basket[itemId] = newQuantityToBuy;

            if (ActiveUser.Basket[itemId] <= 0)
                ActiveUser.RemoveItemFromBasket(itemId);
        }

        public void RemoveFromBasket(int itemIdToRemove)
        {
            ActiveUser.RemoveItemFromBasket(itemIdToRemove);
        }

        public void CompleteOrder(int orderID, Dictionary<int, Tuple<int, float>> updatedQuantities)
        {
            Order orderToComplete = OrdersRepo.GetOrder(orderID);
            DateTime timeNow = DateTime.Now;
            DateOnly currentDate = new DateOnly(timeNow.Year, timeNow.Month, timeNow.Day);

            // first we have to validate the updated quantities
            // if we have enough of everything, otherwise error
            foreach (var itemQuantityEntry in updatedQuantities)
            {
                int itemID = itemQuantityEntry.Key;
                int preferredItemQuantity = itemQuantityEntry.Value.Item1;
                Item itemToVerify = ItemsRepo.GetItem(itemID);

                // I guess we check for batches using the current date...?
                if (itemToVerify.QuantityAtSpecifiedDate(currentDate) < preferredItemQuantity)
                    throw new ArgumentException("We don't have enough of " + itemToVerify.Name + 
                        " - " + itemToVerify.Producer + "; " +
                        "delete the item from the order if you wish to complete it");
            }

            // after validation we need to update the Order entity, and save
            // its changes into the database
            orderToComplete.IsCompleted = true;

            foreach (var itemEntryInOrder in orderToComplete.ItemQuantitiesWithFinalPrice)
                orderToComplete.RemoveItemFromOrder(itemEntryInOrder.Key);

            foreach (var itemQuantityEntry in updatedQuantities)
                orderToComplete.AddItemToOrder(itemQuantityEntry.Key,
                                                itemQuantityEntry.Value.Item1,
                                                itemQuantityEntry.Value.Item2);

            OrdersRepo.UpdateOrder(orderToComplete);

            // after this we have to subtract the quantities from each item
            foreach (var itemQuantityEntry in updatedQuantities)
            {
                int itemID = itemQuantityEntry.Key;
                int itemQuantityToSubtract = itemQuantityEntry.Value.Item1;
                Item itemToUpdate = ItemsRepo.GetItem(itemID);

                itemToUpdate.RemoveQuantity(itemQuantityToSubtract, currentDate);
                ItemsRepo.UpdateItem(itemToUpdate);
            }
        }

        public void ModifyIncompleteOrder(int orderIDToModify,
            Dictionary<int, Tuple<int, float>> updatedQuantities, 
            DateOnly updatedPickUpDate)
        {

            Order orderToModify = OrdersRepo.GetOrder(orderIDToModify);

            // all this copy-pasting might kick me in the ass

            // first we have to validate the updated pick up date
            // (later than the current date or on the current date)

            DateOnly today = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            if (updatedPickUpDate <= today)
                throw new ArgumentException("The new pick-up date must be later than the current date");


            // then we have to validate the updated quantities
            // if we have enough of everything, otherwise error
            foreach (var itemQuantityEntry in updatedQuantities)
            {
                int itemID = itemQuantityEntry.Key;
                int preferredItemQuantity = itemQuantityEntry.Value.Item1;
                Item itemToVerify = ItemsRepo.GetItem(itemID);
                int availableItemQuantityOnDate = itemToVerify.QuantityAtSpecifiedDate(updatedPickUpDate);

                if (availableItemQuantityOnDate < preferredItemQuantity)
                    throw new ArgumentException("On " + updatedPickUpDate.ToString("yyyy.MM.dd") + ", " +
                                                "we will have only " + availableItemQuantityOnDate + " boxes " +
                                                "of " + itemToVerify.Name + " by " + itemToVerify.Producer + " " +
                                                "instead of " + preferredItemQuantity + ".");
            }


            // after validation, we have to modify the Order object
            // and send the updates to the database (items associated with it AND new pick up date)
            foreach (var itemEntryInOrder in orderToModify.ItemQuantitiesWithFinalPrice)
                orderToModify.RemoveItemFromOrder(itemEntryInOrder.Key);

            foreach (var itemQuantityEntry in updatedQuantities)
                orderToModify.AddItemToOrder(itemQuantityEntry.Key,
                                             itemQuantityEntry.Value.Item1,
                                             itemQuantityEntry.Value.Item2);

            orderToModify.PickUpDate = updatedPickUpDate;
            OrdersRepo.UpdateOrder(orderToModify);
        }

        public void PlaceOrderFromBasket(DateOnly chosenPickUpDate)
        {
            // for every item inside the basket, we have to verify
            // if we have enough boxes on stock that expire after
            // the chosen pick-up date

            // after validation we have to calculate the final price
            // for every item, to put it alongside the items in OrderItems

            Dictionary<int, Tuple<int, float>> itemInfoForOrder = new();

            foreach (KeyValuePair<int, int> basketEntry in ActiveUser.Basket)
            {
                Item currentItem = ItemsRepo.GetItem(basketEntry.Key);
                int currentItemQuantity = basketEntry.Value;
                int itemQuantityAtPickUpDate = currentItem.QuantityAtSpecifiedDate(chosenPickUpDate);

                if (currentItemQuantity > itemQuantityAtPickUpDate)
                    throw new ArgumentException("On " + chosenPickUpDate.ToString("yyyy.MM.dd") + ", " +
                                                "we will have only " + itemQuantityAtPickUpDate + " boxes " +
                                                "of " + currentItem.Name + " by " + currentItem.Producer + " " +
                                                "instead of " + currentItemQuantity + ".");

                float finalPrice = currentItemQuantity * currentItem.Price *
                    (1 - currentItem.DiscountPercentage);

                if (ActiveUser.UserDiscounts.ContainsKey(currentItem.Id))
                    finalPrice = finalPrice * (1 - ActiveUser.UserDiscounts[currentItem.Id]);

                itemInfoForOrder.Add(currentItem.Id, new Tuple<int, float>(currentItemQuantity, finalPrice));
            }

            OrdersRepo.AddOrderWithItems(ActiveUser.Id, chosenPickUpDate, itemInfoForOrder);

            // empty the basket after successfully creating the order

            ActiveUser.Basket.Clear();
        }


        public void ResubmitExpiredOrder(int orderIDToResubmit, DateOnly chosenPickUpDate)
        {
            // we have to verify if we have enough boxes on stock
            // that expire after the chosen pick-up date

            Order expiredOrder = OrdersRepo.GetOrder(orderIDToResubmit);
            Dictionary<int, Tuple<int, float>> itemInfoForOrder = expiredOrder.ItemQuantitiesWithFinalPrice;

            foreach (KeyValuePair<int, Tuple<int, float>> orderItemEntry in itemInfoForOrder)
            {
                Item currentItem = ItemsRepo.GetItem(orderItemEntry.Key);
                int currentItemQuantity = orderItemEntry.Value.Item1;
                int itemQuantityAtPickUpDate = currentItem.QuantityAtSpecifiedDate(chosenPickUpDate);

                if (currentItemQuantity > itemQuantityAtPickUpDate)
                    throw new ArgumentException("On " + chosenPickUpDate.ToString("yyyy.MM.dd") + ", " +
                                                "we will have only " + itemQuantityAtPickUpDate + " boxes " +
                                                "of " + currentItem.Name + " by " + currentItem.Producer + " " +
                                                "instead of " + currentItemQuantity + ".");

            }

            OrdersRepo.AddOrderWithItems(ActiveUser.Id, chosenPickUpDate, itemInfoForOrder);

        }


        public Dictionary<int, int> FillBasketFromPrescription(string prescriptionId)
        {
            Dictionary<int, int> items = new();

            // here we are supposed to query for the prescription, but
            // for now we're gonna check it against one id, and we're
            // supposed to receive the same item name and number of pills
            if (!prescriptionId.Equals("testPrescription"))
                throw new ArgumentException("Invalid prescription ID");

            string itemName = "prescript1";
            int nrOfRequiredPills = 40;

            Dictionary<string, float> searchedActiveSubstances = new();
            string connString = SQLUtility.GetConnectionString();
            string selectExactItemsCommandString =
                $"SELECT * FROM Items " +
                $"WHERE name = '{itemName}' " +
                $"AND numberOfPills = {nrOfRequiredPills} " +
                $"ORDER BY price";

            /*$"SELECT ISub.name, concentration " +
            $"FROM ItemSubstances ISub " +
            $"INNER JOIN Items I ON ISub.itemId = I.itemId " +
            $"WHERE I.name = '{itemName}'";*/

            DataSet resultsAcrossQueries = new();

            using SqlConnection conn = new(connString);
            SqlDataAdapter exactFinderAdapter = new(selectExactItemsCommandString, conn);

            // we verify if we get a result right away, exact name and pills
            conn.Open();
            exactFinderAdapter.Fill(resultsAcrossQueries, "ExactNameAndPills");


            // I only need this right now for testing
            // assuming that the item is always going to be found
            Item preferredItem = ItemsRepo.GetItemsByName(itemName)[0];
            int numberOfRequiredSubstances = preferredItem.ActiveSubstances.Count;


            if (resultsAcrossQueries.Tables["ExactNameAndPills"].Rows.Count != 0)
            {
                DataRow entryRow = resultsAcrossQueries.Tables["ExactNameAndPills"].Rows[0];
                if ((int)entryRow["quantity"] != 0)
                {
                    items.Add((int)entryRow["itemId"], 1);
                    return items;
                }

            }


            // then we have to see, same concentration and pills
            string selectExactSubstitutesCommandString =
                "SELECT * FROM Items I " +
                "WHERE I.itemId IN (" +
                    "SELECT DISTINCT ISub.itemId " +
                    "FROM ItemSubstances ISub " +
                    "WHERE NOT EXISTS ( " +
                        "(SELECT ISub1.name, ISub1.concentration FROM ItemSubstances ISub1 " +
                        "INNER JOIN Items I ON ISub1.itemId = I.itemId " +
                        $"WHERE I.name = '{itemName}') " +
                        "EXCEPT " +
                        "(SELECT ISub2.name, ISub2.concentration FROM ItemSubstances ISub2 " +
                        "WHERE ISub.itemId = ISub2.itemId)" +
                    ")" +
                $") AND I.numberOfPills = {nrOfRequiredPills} " +
                "ORDER BY I.price";

            SqlDataAdapter substituteFinderAdapter = new(selectExactSubstitutesCommandString, conn);
            substituteFinderAdapter.Fill(resultsAcrossQueries, "Substitutes");

            if (resultsAcrossQueries.Tables["Substitutes"].Rows.Count != 0)
            {
                // we have to filter out the results where there are
                // other substances besides the ones we searched for,
                // meaning we have to filter out the results where
                // the number of substances inside them are more than the
                // the number of substances for the original

                // TODO do something with these initial numbers
                int cheapestItemID = -1;
                float cheapestPrice = 99999999f;

                foreach (DataRow substituteCandidateEntry in resultsAcrossQueries.Tables["Substitutes"].Rows)
                {
                    int currItemID = (int)substituteCandidateEntry["itemId"];
                    Item currItem = ItemsRepo.GetItem(currItemID);

                    // this means that the item contains ONLY the required substances
                    // and nothing more
                    if (currItem.ActiveSubstances.Count == numberOfRequiredSubstances &&
                        currItem.Quantity != 0)
                    {
                        // calculate the price with discounts
                        float initialPrice = currItem.Price;
                        float itemDiscount = currItem.DiscountPercentage;
                        float userDiscount;
                        if (ActiveUser.UserDiscounts.ContainsKey(currItem.Id))
                            userDiscount = ActiveUser.UserDiscounts[currItem.Id];
                        else
                            userDiscount = 0f;

                        float finalPrice = initialPrice * (1 - itemDiscount) * (1 - userDiscount);


                        if (finalPrice < cheapestPrice)
                        {
                            cheapestPrice = finalPrice;
                            cheapestItemID = currItem.Id;
                        }
                    }

                }

                if (cheapestItemID != -1)
                {
                    if (ItemsRepo.GetItem(cheapestItemID).Quantity != 0)
                    {
                        items.Add(cheapestItemID, 1);
                        return items;
                    }
                }
            }


            // then we have to see, same concentration and less pills (calculating price with multipliers)
            string selectMultipliedSubstitutesCommandString =
                "SELECT * FROM Items I " +
                "WHERE I.itemId IN (" +
                    "SELECT DISTINCT ISub.itemId " +
                    "FROM ItemSubstances ISub " +
                    "WHERE NOT EXISTS ( " +
                        "(SELECT ISub1.name, ISub1.concentration FROM ItemSubstances ISub1 " +
                        "INNER JOIN Items I ON ISub1.itemId = I.itemId " +
                        $"WHERE I.name = '{itemName}') " +
                        "EXCEPT " +
                        "(SELECT ISub2.name, ISub2.concentration FROM ItemSubstances ISub2 " +
                        "WHERE ISub.itemId = ISub2.itemId)" +
                    ")" +
                $") AND I.numberOfPills < {nrOfRequiredPills} " +
                "ORDER BY I.price";

            SqlDataAdapter multipliedSubstituteFinderAdapter = new(selectMultipliedSubstitutesCommandString, conn);
            multipliedSubstituteFinderAdapter.Fill(resultsAcrossQueries, "Multiplies");

            if (resultsAcrossQueries.Tables["Multiplies"].Rows.Count != 0)
            {
                // same here, we have to filter out the results
                // that contain other substances alongside the desired ones

                int cheapestItemId = -1;
                int cheapestItemQuantity = -1;
                float cheapestPrice = 10000000f;

                foreach (DataRow substituteCandidateEntry in resultsAcrossQueries.Tables["Multiplies"].Rows)
                {
                    int currItemID = (int)substituteCandidateEntry["itemId"];
                    Item currItem = ItemsRepo.GetItem(currItemID);

                    if (currItem.ActiveSubstances.Count == numberOfRequiredSubstances)
                    {
                        // calculate the prices with discounts
                        int requiredQuantity = (int)Math.Ceiling((decimal)nrOfRequiredPills / currItem.NumberOfPills);

                        float initialPrice = currItem.Price * requiredQuantity;
                        float itemDiscount = currItem.DiscountPercentage;
                        float userDiscount;
                        if (ActiveUser.UserDiscounts.ContainsKey(currItem.Id))
                            userDiscount = ActiveUser.UserDiscounts[currItem.Id];
                        else
                            userDiscount = 0f;

                        float finalPrice = initialPrice * (1 - itemDiscount) * (1 - userDiscount);


                        if (finalPrice < cheapestPrice &&
                            requiredQuantity <= currItem.Quantity)
                        {
                            cheapestPrice = finalPrice;
                            cheapestItemId = currItem.Id;
                            cheapestItemQuantity = requiredQuantity;
                        }
                    }
                }

                
                if (cheapestItemId != -1)
                {
                    Item cheapestItem = ItemsRepo.GetItem(cheapestItemId);

                    if (cheapestItem.Quantity >= cheapestItemQuantity)
                    {
                        items.Add(cheapestItemId, cheapestItemQuantity);
                        return items;
                    }
                }
            }

            throw new ArgumentException("No adequate medicine found in stock");
        }
    }
}
