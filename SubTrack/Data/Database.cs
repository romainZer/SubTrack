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
                )");
            };
        }

        private IDbConnection CreateConnection()
        {
            return new SqliteConnection($"Data Source={_dbPath};Cache=Shared;");
        }

        #endregion

        #region CRUD
        /// <summary>
        /// Récupère l'ensemble des dépenses
        /// </summary>
        /// <returns>La liste contenant toutes les dépenses</returns>
        public async Task<List<Expense>> GetAllExpensesAsync()
        {
            using (var connection = CreateConnection())
            {
                var query = @"
                SELECT 
                    Id,
                    Title AS ExpenseTitle,
                    Amount AS ExpenseAmount,
                    Date AS ExpenseDate,
                    Category AS ExpenseCategory,
                    IsRecurrent
                FROM Expenses";
                return (await connection.QueryAsync<Expense>(query)).AsList();
            }
        }


        /// <summary>
        /// Insère en base de données la dépense
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public async Task AddExpenseAsync(Expense e)
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
                await connection.ExecuteAsync(@"DELETE FROM Expenses WHERE Id = @Id ", id);
            }
        }
        #endregion
    }
}
