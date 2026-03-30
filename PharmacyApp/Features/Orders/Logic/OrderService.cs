using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using PharmacyApp.Common.Repositories;
using PharmacyApp.Models;

namespace PharmacyApp.Features.Orders.Logic
{
    public class OrderService
    {
        public ISubstancesRepository SubstancesRepo { get; private set; }
        public IItemsRepository ItemsRepo { get; private set; }
        public IUsersRepository UsersRepo { get; private set; }
        public IOrdersRepository OrdersRepo { get; private set; }
        public User ActiveUser { get; private set; }

        public OrderService(User loggedInUser)
        {
            SubstancesRepo = new SQLSubstancesRepository();
            ItemsRepo = new SQLItemsRepository();
            UsersRepo = new SQLUsersRepository();
            OrdersRepo = new SQLOrdersRepository();
            ActiveUser = loggedInUser;

            AddToBasket(5, 1);
            AddToBasket(7, 2);
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
                    (1 - currentItem.DiscountPercentage) *
                    (1 - ActiveUser.UserDiscounts[currentItem.Id]);

                itemInfoForOrder.Add(currentItem.Id, new Tuple<int, float>(currentItemQuantity, finalPrice));
            }

            OrdersRepo.AddOrderWithItems(ActiveUser.Id, chosenPickUpDate, itemInfoForOrder);

            // empty the basket after successfully creating the order

            ActiveUser.Basket.Clear();
        }

        public Dictionary<int, int> FillBasketFromPrescription(string prescriptionId)
        {
            float error = 5f;
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
                DataRow entryRow = resultsAcrossQueries.Tables["Substitutes"].Rows[0];
                if ((int)entryRow["quantity"] != 0)
                {
                    items.Add((int)entryRow["itemId"], 1);
                    return items;
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
                int bestItemId = -1;
                int bestItemQuantity = -1;
                float bestPrice = 10000000f;

                foreach (DataRow entryRow in resultsAcrossQueries.Tables["Multiplies"].Rows)
                {
                    int iterQuantity = (int)Math.Ceiling((decimal)nrOfRequiredPills / (int)entryRow["numberOfPills"]);

                    float iterPrice = iterQuantity * (float)(decimal)entryRow["price"];
                    if (iterPrice < bestPrice)
                    {
                        bestPrice = iterPrice;
                        bestItemId = (int)entryRow["itemId"];
                        bestItemQuantity = iterQuantity;
                    }
                }

                items.Add(bestItemId, bestItemQuantity);
                return items;
            }

            throw new ArgumentException("No adequate medicine found");
        }
    }
}
