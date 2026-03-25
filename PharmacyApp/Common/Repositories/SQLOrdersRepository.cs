using PharmacyApp.Models;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;

namespace PharmacyApp.Common.Repositories
{
    public class SQLOrdersRepository : IOrdersRepository
    {
        public SQLOrdersRepository()
        {
        }

        public void AddOrder(int clientId, DateOnly pickUpDate, bool isCompleted = false, bool isExpired = false)
        {
            string connString = SQLUtility.GetConnectionString();
            string pickUpDateString = $"{pickUpDate.Year}-{pickUpDate.Month}-{pickUpDate.Day}";
            string insertCommandString = "INSERT INTO Orders (clientId, isCompleted, isExpired, pickUpDate) " +
                                        $"VALUES ({clientId}, '{isCompleted}', '{isExpired}', '{pickUpDateString}')";

            using SqlConnection conn = new SqlConnection(connString);

            SqlCommand insertOrderCommand = new SqlCommand(insertCommandString, conn);
            conn.Open();
            insertOrderCommand.ExecuteNonQuery();
        }

        public void RemoveOrder(int orderIdToBeRemoved)
        {
            string connString = SQLUtility.GetConnectionString();
            string deleteItemsInOrderString = $"DELETE FROM OrderItems WHERE orderId = {orderIdToBeRemoved}";
            string deleteCommandString = $"DELETE FROM Orders WHERE orderId = {orderIdToBeRemoved}";

            using SqlConnection conn = new SqlConnection(connString);

            SqlCommand deleteItemsInOrderCommand = new SqlCommand(deleteItemsInOrderString, conn);
            SqlCommand deleteOrderCommand = new SqlCommand(deleteCommandString, conn);

            // first we have to delete the entries in OrderItems for this orderId
            // i.e. the items that are included in this order
            // because of foreign key constraints

            conn.Open();
            deleteItemsInOrderCommand.ExecuteNonQuery();
            deleteOrderCommand.ExecuteNonQuery();
        }

        public void UpdateOrder(Order newOrder)
        {
            string connString = SQLUtility.GetConnectionString();
            string pickUpDateString = $"{newOrder.PickUpDate.Year}-{newOrder.PickUpDate.Month}-{newOrder.PickUpDate.Day}";
            string updateCommandString = $"UPDATE Orders " +
                                        $"SET clientId = {newOrder.ClientId}, " +
                                        $"isCompleted = '{newOrder.IsCompleted}', " +
                                        $"isExpired = '{newOrder.IsExpired}', " +
                                        $"pickUpDate = '{pickUpDateString}' " +
                                        $"WHERE orderId = {newOrder.Id}";

            using SqlConnection conn = new SqlConnection(connString);

            SqlCommand updateOrderCommand = new SqlCommand(updateCommandString, conn);
            conn.Open();
            updateOrderCommand.ExecuteNonQuery();


            // we have to also update the items, their quantities and final prices
            // that are inside this order for the table OrderItems

            string deleteItemsInOrderCommandString = $"DELETE FROM OrderItems WHERE orderId = {newOrder.Id}";
            SqlCommand deleteItemsInOrderCommand = new SqlCommand(deleteItemsInOrderCommandString, conn);
            deleteItemsInOrderCommand.ExecuteNonQuery();

            foreach (KeyValuePair<int, Tuple<int, float>> itemInOrder in newOrder.ItemQuantitiesWithFinalPrice)
            {
                int itemId = itemInOrder.Key;
                int itemQuantity = itemInOrder.Value.Item1;
                float finalPrice = itemInOrder.Value.Item2;

                string insertItemsInOrderCommandString = 
                    $"INSERT INTO OrderItems (orderId, itemId, orderQuantity, price) " +
                    $"VALUES ({newOrder.Id}, {itemId}, {itemQuantity}, {finalPrice})";
                SqlCommand insertItemsInOrderCommand = new SqlCommand(insertItemsInOrderCommandString, conn);
                insertItemsInOrderCommand.ExecuteNonQuery();
            }
        }

        public Order GetOrder(int orderId)
        {
            // we have to get the items of the particular order from OrderItems
            // in order to put those into the Order object's dictionary for items
            // therefore create the Order object fully

            string connString = SQLUtility.GetConnectionString();
            string selectOrderCommandString = $"SELECT * FROM Orders WHERE orderId = {orderId}";
            string selectItemsInOrderCommandString = $"SELECT itemId, orderQuantity, price FROM OrderItems WHERE orderId = {orderId}";

            using SqlConnection conn = new SqlConnection(connString);

            SqlDataAdapter orderAdapter = new SqlDataAdapter(selectOrderCommandString, conn);
            SqlDataAdapter itemsInOrderAdapter = new SqlDataAdapter(selectItemsInOrderCommandString, conn);
            DataSet orderDataFromDb = new DataSet();

            conn.Open();
            orderAdapter.Fill(orderDataFromDb, "Orders");
            itemsInOrderAdapter.Fill(orderDataFromDb, "OrderItems");


            // we expect only one order as a result (cuz orderId is unique for every entry)

            DataRow resultingRow = orderDataFromDb.Tables["Orders"].Rows[0];

            int resultingOrderId = (int)resultingRow["orderId"];
            int resultingClientId = (int)resultingRow["clientId"];
            bool resultingCompletedStatus = (bool)resultingRow["isCompleted"];
            bool resultingExpiredStatus = (bool)resultingRow["isExpired"];
            DateOnly resultingPickUpDate = DateOnly.FromDateTime((DateTime)resultingRow["pickUpDate"]);

            Order resultingOrder = new Order(resultingOrderId, resultingClientId, resultingPickUpDate, resultingCompletedStatus, resultingExpiredStatus);


            // inserting the items that were already inside this order at the time of querying

            foreach (DataRow itemInOrderRow in orderDataFromDb.Tables["OrderItems"].Rows)
            {
                int itemId = (int)itemInOrderRow["itemId"];
                int itemQuantity = (int)itemInOrderRow["orderQuantity"];
                float finalPrice = (float)(decimal)itemInOrderRow["price"];
                resultingOrder.AddItemToOrder(itemId, itemQuantity, finalPrice);
            }

            return resultingOrder;
        }

        public List<Order> GetOrdersOfClient(int clientId)
        {
            List<Order> ordersByClient = new List<Order>();

            string connString = SQLUtility.GetConnectionString();
            string selectOrdersCommandString = $"SELECT * FROM Orders WHERE clientId = {clientId}";

            using SqlConnection conn = new SqlConnection(connString);
            SqlDataAdapter orderAdapter = new SqlDataAdapter(selectOrdersCommandString, conn);
            DataSet orderInfoFromDb = new DataSet();

            conn.Open();
            orderAdapter.Fill(orderInfoFromDb, "Orders");
            

            // at this point we're just going to do what we do in getOrder
            // for every row in our previous query
            // ...
            // actually I could've just used getOrder to not write the same piece
            // of code twice, fucking hell, me stoopid
            // altho SqlConnection throws an exception if we try to open the same
            // connection twice without closing the first one
            // hmmm
            // 
            // TODO refactor this part later, so that we use getOrder(), avoiding repetition

            foreach (DataRow orderRow in orderInfoFromDb.Tables["Orders"].Rows)
            {
                int resultingOrderId = (int)orderRow["orderId"];
                int resultingClientId = (int)orderRow["clientId"];
                bool resultingCompletedStatus = (bool)orderRow["isCompleted"];
                bool resultingExpiredStatus = (bool)orderRow["isExpired"];
                DateOnly resultingPickUpDate = DateOnly.FromDateTime((DateTime)orderRow["pickUpDate"]);

                Order resultingOrder = new Order(resultingOrderId, resultingClientId, resultingPickUpDate, resultingCompletedStatus, resultingExpiredStatus);

                string selectItemsInOrderCommandString = 
                    $"SELECT itemId, orderQuantity, price FROM OrderItems " +
                    $"WHERE orderId = {resultingOrder.Id}";

                SqlDataAdapter itemsInOrderAdapter = new SqlDataAdapter(selectItemsInOrderCommandString, conn);
                DataSet itemDataFromDb = new DataSet();
                itemsInOrderAdapter.Fill(itemDataFromDb, "OrderItems");

                foreach (DataRow itemInOrderRow in itemDataFromDb.Tables["OrderItems"].Rows)
                {
                    int itemId = (int)itemInOrderRow["itemId"];
                    int itemQuantity = (int)itemInOrderRow["orderQuantity"];
                    float finalPrice = (float)(decimal)itemInOrderRow["price"];
                    resultingOrder.AddItemToOrder(itemId, itemQuantity, finalPrice);
                }

                ordersByClient.Add(resultingOrder);
            }
            

            return ordersByClient;
        }

        public bool OrderExists(int orderId)
        {
            string connString = SQLUtility.GetConnectionString();
            string selectCommandString = $"SELECT * FROM Orders WHERE orderId = {orderId}";

            using SqlConnection conn = new SqlConnection(connString);

            SqlDataAdapter ordersAdapter = new SqlDataAdapter(selectCommandString, conn);
            DataSet orders = new DataSet();

            conn.Open();
            ordersAdapter.Fill(orders, "Orders");
            if (orders.Tables["Orders"].Rows.Count > 0)
                return true;

            return false;
        }
    }
}
