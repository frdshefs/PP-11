using System;
using Microsoft.Data.SqlClient;

namespace CarServiceApp
{
    class Program
    {
        // ПОМЕНЯЙ ТВОЕ_ИМЯ_СЕРВЕРА НА СВОЕ!
        static string connectionString = "Server=F;Database=carservice_db;Trusted_Connection=True;TrustServerCertificate=True;";

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine("   CAR SERVICE MANAGEMENT SYSTEM (CSMS) ");
                Console.WriteLine("========================================");
                Console.WriteLine("1. Показать список активных ремонтов (Отчет)");
                Console.WriteLine("2. Показать дефицитные запчасти (Отчет)");
                Console.WriteLine("3. Добавить нового клиента (CRUD - Create)");
                Console.WriteLine("4. Показать всех клиентов (CRUD - Read)");
                Console.WriteLine("5. Выйти из программы");
                Console.WriteLine("========================================");
                Console.Write("Выбери пункт меню: ");

                string choice = Console.ReadLine();

                if (choice == "1") ShowActiveRepairs();
                else if (choice == "2") ShowLowStockParts();
                else if (choice == "3") AddNewCustomer();
                else if (choice == "4") ShowAllCustomers();
                else if (choice == "5") return;
            }
        }

        static void ShowActiveRepairs()
        {
            Console.Clear();
            Console.WriteLine("--- Активные ремонты ---");
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT order_id, customer_name, status, total_cost FROM view_active_repairs;", conn);
                try {
                    conn.Open();
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read()) 
                            Console.WriteLine($"Заказ №{r["order_id"]} | {r["customer_name"]} | {r["status"]} | {r["total_cost"]} руб.");
                    }
                } catch (Exception ex) { Console.WriteLine("Ошибка БД: " + ex.Message); }
            }
            Console.WriteLine("\nНажмите любую клавишу для возврата...");
            Console.ReadKey();
        }

        static void ShowLowStockParts()
        {
            Console.Clear();
            Console.WriteLine("--- Дефицитные запчасти ---");
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT part_number, name, stock_quantity FROM view_low_stock_parts;", conn);
                try {
                    conn.Open();
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read()) 
                            Console.WriteLine($"[{r["part_number"]}] {r["name"]} - остаток {r["stock_quantity"]} шт.");
                    }
                } catch (Exception ex) { Console.WriteLine("Ошибка БД: " + ex.Message); }
            }
            Console.WriteLine("\nНажмите любую клавишу для возврата...");
            Console.ReadKey();
        }

        static void AddNewCustomer()
        {
            Console.Clear();
            Console.WriteLine("--- Добавление нового клиента ---");
            Console.Write("Имя: "); string fName = Console.ReadLine();
            Console.Write("Фамилия: "); string lName = Console.ReadLine();
            Console.Write("Телефон: "); string phone = Console.ReadLine();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO customers (first_name, last_name, phone) VALUES (@f, @l, @p);", conn);
                cmd.Parameters.AddWithValue("@f", fName);
                cmd.Parameters.AddWithValue("@l", lName);
                cmd.Parameters.AddWithValue("@p", phone);
                try { 
                    conn.Open(); 
                    cmd.ExecuteNonQuery(); 
                    Console.WriteLine("\n[Успех] Данные успешно записаны в SQL Server!"); 
                }
                catch (Exception ex) { Console.WriteLine("Ошибка БД: " + ex.Message); }
            }
            Console.WriteLine("\nНажмите любую клавишу для возврата...");
            Console.ReadKey();
        }

        static void ShowAllCustomers()
        {
            Console.Clear();
            Console.WriteLine("--- Список всех клиентов ---");
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT id, first_name, last_name, phone FROM customers WHERE isdeleted = 0;", conn);
                try {
                    conn.Open();
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read()) 
                            Console.WriteLine($"ID: {r["id"]} | {r["last_name"]} {r["first_name"]} | Тел: {r["phone"]}");
                    }
                } catch (Exception ex) { Console.WriteLine("Ошибка БД: " + ex.Message); }
            }
            Console.WriteLine("\nНажмите любую клавишу для возврата...");
            Console.ReadKey();
        }
    }
}