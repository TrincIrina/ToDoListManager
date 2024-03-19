using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Markup;

namespace ToDoListManager.DB
{
    internal class DBOperation : IDealsRepository
    {
        private readonly string connectionString;
        public DBOperation()
        {
            // считаем из конфига строку подключения
            connectionString = ConfigurationManager.
                ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        // Добавление дела в список
        public void AddCase(Deals deals)
        {
            using (SqlConnection connection = OpenConnection())
            {
                SqlCommand command = new SqlCommand("INSERT INTO ToDoList VALUES " +
                    "(@case_f, @priority_f, @execution_f) ", connection);
                command.Parameters.Add("@case_f", SqlDbType.NVarChar).Value = deals.Case;
                command.Parameters.Add("@priority_f", SqlDbType.Int).Value = deals.Priority;
                command.Parameters.Add("@execution_f", SqlDbType.Date).Value = deals.Execution;
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected != 1)
                {
                    throw new InvalidOperationException("Case not added");
                }
            }
        }
        // Удаление дела
        public void DeleteCase(int id)
        {
            using (SqlConnection connection = OpenConnection())
            {
                // 1. формирование запроса
                SqlCommand command = new SqlCommand($"DELETE FROM ToDoList WHERE id = @id",
                    connection);
                command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                // 2. выполнить запрос
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected != 1)
                {
                    throw new InvalidOperationException("Case not deleted");
                }
            }
        }
        // Редактирование дела
        public void EditCase(Deals deals)
        {
            using(SqlConnection connection = OpenConnection())
            {
                SqlCommand command = new SqlCommand("UPDATE ToDoList SET " +
                    "case_f=@case_f, priority_f=@priority_f, execution_f=@execution_f " +
                    "WHERE id=@id;", connection);
                command.Parameters.Add("@id", SqlDbType.Int).Value = deals.Id;
                command.Parameters.Add("@case_f", SqlDbType.NVarChar).Value = deals.Case;
                command.Parameters.Add("@priority_f", SqlDbType.Int).Value = deals.Priority;
                command.Parameters.Add("@execution_f", SqlDbType.Date).Value = deals.Execution;
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected != 1)
                {
                    throw new InvalidOperationException("Case not updated");
                }
            }
        }
        // Вывод списка
        public List<Deals> GetAll()
        {
            using (SqlConnection connection = OpenConnection())
            using (SqlCommand command = new SqlCommand("SELECT * FROM ToDoList", connection))
            using (SqlDataReader reader = command.ExecuteReader())
            {                
                List<Deals> deals = new List<Deals>();
                while (reader.Read())
                {
                    deals.Add(ReadingDeals(reader));
                }
                return deals;                
            }
        }

        public List<Deals> OrderByDeadline()
        {
            using (SqlConnection connection = OpenConnection())
            using (SqlCommand command = new SqlCommand(
                "SELECT * FROM ToDoList ORDER BY execution_f;", connection))
            using (SqlDataReader reader = command.ExecuteReader())
            {
                List<Deals> deals = new List<Deals>();
                while (reader.Read())
                {
                    deals.Add(ReadingDeals(reader));
                }
                return deals;
            }
        }

        public List<Deals> OrderByPriority()
        {
            using (SqlConnection connection = OpenConnection())
            using (SqlCommand command = new SqlCommand(
                "SELECT * FROM ToDoList ORDER BY priority_f;", connection))
            using (SqlDataReader reader = command.ExecuteReader())
            {
                List<Deals> deals = new List<Deals>();
                while (reader.Read())
                {                    
                    deals.Add(ReadingDeals(reader));
                }
                return deals;
            }
        }

        private SqlConnection OpenConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        private Deals ReadingDeals(SqlDataReader reader)
        {
            Deals deal = new Deals();
            deal.Id = reader.GetInt32(0);
            deal.Case = reader.GetString(1);
            deal.Priority = reader.GetInt32(2);
            deal.Execution = reader.GetDateTime(3);
            return deal;
        }
    }
}
