using Dapper;
using Microsoft.Data.Sqlite;
using SubTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubTrack.Data
{
    public class Database
    {
        #region Attributes
        private static readonly Lazy<Database> _instance = new(() => new Database());
        private readonly string _dbPath;
        #endregion

        #region Properties
        /// <summary>
        /// Instance de la base de données
        /// </summary>
        public static Database Instance => _instance.Value;
        #endregion

        #region Constructors
        private Database()
        {
            _dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "expenses.db");
            InitializeDatabase();
        }
        #endregion

        #region Methods
        private void InitializeDatabase()
        {
            using (var connection = CreateConnection())
            {
                connection.Execute(@"
                CREATE TABLE IF NOT EXISTS Expenses (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL,
                    Amount REAL NOT NULL,
                    Date TEXT NOT NULL,
                    Category TEXT,
                    IsRecurrent INTEGER NOT NULL
                );

                CREATE TABLE IF NOT EXISTS MonthlyBudgets (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Month INTEGER NOT NULL,  
                    Year INTEGER NOT NULL,
                    Budget REAL NOT NULL
                );


                CREATE TABLE IF NOT EXISTS MonthlyIncomes (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL,
                    Amount REAL NOT NULL,
                    Month INTEGER NOT NULL,
                    Year INTEGER NOT NULL
                );");
            }
            ;
        }

        private IDbConnection CreateConnection()
        {
            return new SqliteConnection($"Data Source={_dbPath};Cache=Shared;");
        }

        #endregion

        #region Expenses
        /// <summary>
        /// Récupère l'ensemble des dépenses
        /// </summary>
        /// <returns>La liste contenant toutes les dépenses</returns>
        public async Task<List<FinancialOperation>> GetAllExpensesAsync()
        {
            using (var connection = CreateConnection())
            {
                var query = @"
                SELECT 
                    Id AS ExpenseId,
                    Title AS ExpenseTitle,
                    Amount AS ExpenseAmount,
                    Date AS ExpenseDate,
                    Category AS ExpenseCategory,
                    IsRecurrent
                FROM Expenses";
                return (await connection.QueryAsync<FinancialOperation>(query)).AsList();
            }
        }


        /// <summary>
        /// Insère en base de données la dépense
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public async Task AddExpenseAsync(FinancialOperation e)
        {
            using (var connection = CreateConnection())
            {
                await connection.ExecuteAsync(@"
                    INSERT INTO Expenses(Title, Amount, Date, Category, IsRecurrent)
                    VALUES (@ExpenseTitle, @ExpenseAmount, @ExpenseDate, @ExpenseCategory, @IsRecurrent)", e);
            }
        }

        /// <summary>
        /// Supprime
        /// </summary>
        /// <param name="id">Id de la dépense</param>
        /// <returns></returns>
        public async Task DeleteExpenseByIdAsync(int id)
        {
            using (var connection = CreateConnection())
            {
                await connection.ExecuteAsync(@"DELETE FROM Expenses WHERE Id = @Id", new { Id = id });
            }
        }
        #endregion

        #region Budgets

        public async Task SetMonthlyBudgetAsync(int month, int year, double budget)
        {
            using (var connection = CreateConnection())
            {
                await connection.ExecuteAsync(@"
                    INSERT INTO MonthlyBudgets (Month, Year, Budget) 
                    VALUES (@Month, @Year, @Budget)
                    ON CONFLICT(Month, Year) DO UPDATE SET Budget = @Budget;",
                    new { Month = month, Year = year, Budget = budget });
            }
        }

        public async Task<double?> GetMonthlyBudgetAsync(int month, int year)
        {
            using (var connection = CreateConnection())
            {
                return await connection.ExecuteScalarAsync<double?>(@"
                    SELECT Budget FROM MonthlyBudgets WHERE Month = @Month AND Year = @Year",
                    new { Month = month, Year = year });
            }
        }


        #endregion

        #region Incomes

        public async Task AddIncomeAsync(string title, double amount, int month, int year)
        {
            using (var connection = CreateConnection())
            {
                await connection.ExecuteAsync(@"
                    INSERT INTO MonthlyIncomes (Title, Amount, Month, Year) 
                    VALUES (@Title, @Amount, @Month, @Year);",
                    new { Title = title, Amount = amount, Month = month, Year = year });
            }
        }

        public async Task<double> GetTotalMonthlyIncomeAsync(int month, int year)
        {
            using (var connection = CreateConnection())
            {
                return await connection.ExecuteScalarAsync<double>(@"
                    SELECT COALESCE(SUM(Amount), 0) FROM MonthlyIncomes 
                    WHERE Month = @Month AND Year = @Year;",
                    new { Month = month, Year = year });
            }
        }

        #endregion
    }
}
