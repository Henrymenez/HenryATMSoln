using ATM.DAL;
using ATM.DAL.models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ATM.BL
{
    public class ATMServices : IAtmServices
    {
        private readonly AtmDBConnect _dbContext;
        private bool _disposed;

        public ATMServices(AtmDBConnect atmDBConnect)
        {
            _dbContext = atmDBConnect;
        }
        public async Task<userViewModel> CheckCardNumber(string cardnumber)
        {
            userViewModel user = new userViewModel();
            try
            {
                SqlConnection sqlConn = await _dbContext.OpenConnection();

                string getUserInfo = $"SELECT Users.name,Users.Id,Users.Pin FROM Users WHERE CardNumber = @cardnumber";
                await using SqlCommand command = new SqlCommand(getUserInfo, sqlConn);
                command.Parameters.AddRange(new SqlParameter[]
                {
                new SqlParameter
                {
                    ParameterName = "@cardnumber",
                    Value = cardnumber,
                    SqlDbType = SqlDbType.VarChar,
                    Direction = ParameterDirection.Input

                }
                });


                using (SqlDataReader dataReader = await command.ExecuteReaderAsync())
                {
                    while (dataReader.Read())
                    {
                        user.Name = dataReader["name"].ToString();
                        user.userId = Convert.ToInt32(dataReader["Id"]);
                        user.cardPin = dataReader["Pin"].ToString();
                    }
                }

                return user;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

            }
            return user;
        }

        public async Task<userViewModel> CheckAccountNumber(string accountnumber)
        {
            userViewModel user = new userViewModel();
            try
            {
                SqlConnection sqlConn = await _dbContext.OpenConnection();

                string getUserInfo = $"SELECT Users.name,Users.Id,Users.Pin FROM Users WHERE AccountNo = @accountnumber";
                await using SqlCommand command = new SqlCommand(getUserInfo, sqlConn);
                command.Parameters.AddRange(new SqlParameter[]
                {
                new SqlParameter
                {
                    ParameterName = "@accountnumber",
                    Value = accountnumber,
                    SqlDbType = SqlDbType.VarChar,
                    Direction = ParameterDirection.Input

                }
                });


                using (SqlDataReader dataReader = await command.ExecuteReaderAsync())
                {
                    while (dataReader.Read())
                    {
                        user.Name = dataReader["name"].ToString();
                        user.userId = Convert.ToInt32(dataReader["Id"]);
                        user.cardPin = dataReader["Pin"].ToString();
                    }
                }

                return user;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

            }
            return user;
        }
        public async Task deposit(int id, decimal amount)
        {
            try
            {
                SqlConnection sqlConn = await _dbContext.OpenConnection();

                string getUserInfo = $"SELECT Users.balance FROM Users WHERE Id = @UserId";
                await using SqlCommand command = new SqlCommand(getUserInfo, sqlConn);
                command.Parameters.AddRange(new SqlParameter[]
                {
                new SqlParameter
                {
                    ParameterName = "@UserId",
                    Value = id,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input,

                }
                });
                userViewModel user = new userViewModel();
                using (SqlDataReader dataReader = await command.ExecuteReaderAsync())
                {
                    while (dataReader.Read())
                    {
                        user.balance = (decimal)dataReader["balance"];
                    }
                }

                user.balance = user.balance + amount;

                command.CommandText = $"UPDATE  Users SET balance = {user.balance}  WHERE Id = @UserId";

                var result = await command.ExecuteNonQueryAsync();
                if (result > 0)
                {
                    DateTime myDateTime = DateTime.Now;
                    string sqlFormat = myDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    string desc = $"Deposited a sum of {amount} into your account, you current balance is {user.balance}";
                    command.CommandText = $"INSERT INTO Transactions (userId,receiverId,transactionType,desctiption,amount,status,createdAt)" +
                         $" VALUES (@sendId,null,'Deposit',@desc,@amount,1,@date)";
                    command.Parameters.AddRange(new SqlParameter[]
               {
                 new SqlParameter
                 {
                     ParameterName = "@sendId",
                     Value = id,
                     SqlDbType = SqlDbType.Int,
                     Direction = ParameterDirection.Input
                 },
                  new SqlParameter
                 {
                     ParameterName = "@desc",
                     Value = desc,
                     SqlDbType = SqlDbType.NText,
                     Direction = ParameterDirection.Input,

                 },
                   new SqlParameter
                 {
                     ParameterName = "@amount",
                     Value = amount,
                     SqlDbType = SqlDbType.Decimal,
                     Direction = ParameterDirection.Input,

                 },
                    new SqlParameter
                 {
                     ParameterName = "@date",
                     Value = sqlFormat,
                     SqlDbType = SqlDbType.DateTime,
                     Direction = ParameterDirection.Input,

                 }

               });
                    await command.ExecuteNonQueryAsync();
                    Console.WriteLine(id);
                    Console.WriteLine(user.balance);
                    Console.WriteLine(result);
                    Console.WriteLine("Deposit Was successful");
                }
                else
                {
                    Console.WriteLine("Unsuccessful deposit");

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

            }
        }

        public async Task withdraw(int id, decimal amount)
        {
            try
            {
                SqlConnection sqlConn = await _dbContext.OpenConnection();

                string getUserInfo = $"SELECT Users.balance FROM Users WHERE Id = @UserId";
                await using SqlCommand command = new SqlCommand(getUserInfo, sqlConn);
                command.Parameters.AddRange(new SqlParameter[]
                {
                new SqlParameter
                {
                    ParameterName = "@UserId",
                    Value = id,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input,
                    Size = 50
                }
                });

                userViewModel user = new userViewModel();
                using (SqlDataReader dataReader = await command.ExecuteReaderAsync())
                {
                    while (dataReader.Read())
                    {
                        user.balance = (decimal)dataReader["balance"];
                    }
                }

                if (amount > user.balance)
                {
                    Console.WriteLine("Insucficient Balance");
                    Environment.Exit(0);
                }

                user.balance = user.balance - amount;

                command.CommandText = $"UPDATE  Users SET balance = {user.balance}  WHERE Id = @UserId";

                var result = await command.ExecuteNonQueryAsync();

                if (result > 0)
                {

                    DateTime myDateTime = DateTime.Now;
                    string sqlFormat = myDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    string desc = $"Withdrew a sum of {amount} from your account, you current balance is {user.balance}";
                    command.CommandText = $"INSERT INTO Transactions (userId,receiverId,transactionType,desctiption,amount,status,createdAt)" +
                         $" VALUES (@sendId,null,'Withdraw',@desc,@amount,1,@date)";
                    command.Parameters.AddRange(new SqlParameter[]
               {
                new SqlParameter
                {
                    ParameterName = "@sendId",
                    Value = id,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                },
                 new SqlParameter
                {
                    ParameterName = "@desc",
                    Value = desc,
                    SqlDbType = SqlDbType.NText,
                    Direction = ParameterDirection.Input,

                },
                  new SqlParameter
                {
                    ParameterName = "@amount",
                    Value = amount,
                    SqlDbType = SqlDbType.Decimal,
                    Direction = ParameterDirection.Input,

                },
                   new SqlParameter
                {
                    ParameterName = "@date",
                    Value = sqlFormat,
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,

                }

               });
                    await command.ExecuteNonQueryAsync();
                    Console.WriteLine($"Withdrawal Successful");
                }
                else
                {
                    Console.WriteLine("Unsuccessful Withdrawal");
                    Environment.Exit(0);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

        }


        public async Task transfer(int sender, int receiver, decimal amount)
        {
            try
            {
                if (sender == receiver) throw new Exception("You can not send money to yourself");
                //open connection
                SqlConnection sqlConn = await _dbContext.OpenConnection();

                //senders info
                string senderInfo = $"SELECT Users.balance FROM Users WHERE Id = @senderId";
                await using SqlCommand command = new SqlCommand(senderInfo, sqlConn);
                command.Parameters.AddRange(new SqlParameter[]
                {
                new SqlParameter
                {
                    ParameterName = "@senderId",
                    Value = sender,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input,
                    Size = 50
                }
                });

                userViewModel senderObj = new userViewModel();
                using (SqlDataReader dataReaderSender = await command.ExecuteReaderAsync())
                {
                    while (dataReaderSender.Read())
                    {
                        senderObj.balance = (decimal)dataReaderSender["balance"];
                    }
                }

                if (amount > senderObj.balance)
                {
                    Console.WriteLine("Insucficient Balance");
                    Environment.Exit(0);
                }

                //Receivers Info

                string receiverInfo = $"SELECT Users.balance,Users.name FROM Users WHERE Id = @receiverId";
                await using SqlCommand command2 = new SqlCommand(receiverInfo, sqlConn);
                command2.Parameters.AddRange(new SqlParameter[]
                {
                new SqlParameter
                {
                    ParameterName = "@receiverId",
                    Value = receiver,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input,
                    Size = 50
                }
                });

                userViewModel receiverObj = new userViewModel();
                using (SqlDataReader dataReaderReceiver = await command2.ExecuteReaderAsync())
                {
                    while (dataReaderReceiver.Read())
                    {
                        receiverObj.balance = (decimal)dataReaderReceiver["balance"];
                        receiverObj.Name = (string)dataReaderReceiver["name"];
                    }
                }

                // do transfer

                senderObj.balance = senderObj.balance - amount;
                receiverObj.balance = receiverObj.balance + amount;

                //update sender
                command.CommandText = $"UPDATE  Users SET balance = {senderObj.balance}  WHERE Id = @senderId";

                var result = await command.ExecuteNonQueryAsync();

                if (result > 0)
                {
                    //update receiver
                    command2.CommandText = $"UPDATE  Users SET balance = {receiverObj.balance}  WHERE Id = @receiverId";

                    var result2 = await command2.ExecuteNonQueryAsync();

                    if (result2 > 0)
                    {
                        DateTime myDateTime = DateTime.Now;
                        string sqlFormat = myDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        string desc = $"Transfered a sum of {amount} from your account to {receiverObj.Name}, your current balance is {senderObj.balance}";
                        command.CommandText = $"INSERT INTO Transactions (userId,receiverId,transactionType,desctiption,amount,status,createdAt)" +
                             $" VALUES (@sendId,@receiverId,'Transfer',@desc,@amount,1,@date)";
                        command.Parameters.AddRange(new SqlParameter[]
                   {
                new SqlParameter
                {
                    ParameterName = "@sendId",
                    Value = sender,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                },
                 new SqlParameter
                {
                    ParameterName = "@receiverId",
                    Value = receiver,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                },
                 new SqlParameter
                {
                    ParameterName = "@desc",
                    Value = desc,
                    SqlDbType = SqlDbType.NText,
                    Direction = ParameterDirection.Input,

                },
                  new SqlParameter
                {
                    ParameterName = "@amount",
                    Value = amount,
                    SqlDbType = SqlDbType.Decimal,
                    Direction = ParameterDirection.Input,

                },
                   new SqlParameter
                {
                    ParameterName = "@date",
                    Value = sqlFormat,
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,

                }

                   });
                        await command.ExecuteNonQueryAsync();
                        Console.WriteLine($"Transfer Successful");
                    }
                    else
                    {
                        Console.WriteLine("system Error: Unable to complete transfer");
                    }
                }
                else
                {
                    Console.WriteLine("Unsuccessful Withdrawal");
                    Environment.Exit(0);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

        }


        public async Task checkBalance(int id)
        {
            try
            {
                SqlConnection sqlConn = await _dbContext.OpenConnection();

                string getUserInfo = $"SELECT Users.balance FROM Users WHERE Id = @UserId";
                await using SqlCommand command = new SqlCommand(getUserInfo, sqlConn);
                command.Parameters.AddRange(new SqlParameter[]
                {
                new SqlParameter
                {
                    ParameterName = "@UserId",
                    Value = id,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input,
                    Size = 50
                }
                });
                userViewModel user = new userViewModel();
                using (SqlDataReader dataReader = await command.ExecuteReaderAsync())
                {
                    while (dataReader.Read())
                    {
                        user.balance = (decimal)dataReader["balance"];
                    }
                }

                Console.WriteLine($"Your Balance is ${user.balance}");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

            }
        }

        public async Task checkStatment(int id)
        {
            try
            {
                SqlConnection sqlConn = await _dbContext.OpenConnection();

                string getTransactionInfo = $"SELECT Transactions.userId,Transactions.receiverId,Transactions.desctiption,Transactions.amount,Transactions.transactionType,Transactions.status,Transactions.createdAt FROM Transactions WHERE userId = @UserId";
                await using SqlCommand command = new SqlCommand(getTransactionInfo, sqlConn);
                command.Parameters.AddRange(new SqlParameter[]
                {
                 new SqlParameter
                 {
                     ParameterName = "@UserId",
                     Value = id,
                     SqlDbType = SqlDbType.Int,
                     Direction = ParameterDirection.Input,
                     Size = 50
                 }
                });
                List<transactionViewModel> transactions = new List<transactionViewModel>();
                using (SqlDataReader dataReader = await command.ExecuteReaderAsync())
                {
                    while (dataReader.Read())
                    {
                        transactions.Add(new transactionViewModel()
                        {
                            userId = (int)dataReader["userId"],
                            ReceiverId = (string)dataReader["receiverId"].ToString() ?? "Null",
                            Description = dataReader["desctiption"].ToString(),
                            TransactionType = dataReader["transactionType"].ToString(),
                            Amount = (decimal)dataReader["amount"],
                            Status = (bool)dataReader["status"],
                            CreatedAt = (DateTime)dataReader["createdAt"]
                        });


                    }
                }

                Console.WriteLine(transactions);
               /* foreach (transactionViewModel transaction in transactions)
                {
                   
                    Console.WriteLine($" {transaction.Description ?? "No Transaction Yet"}, Type: {transaction.TransactionType},\n" +
                        $"Amount: {transaction.Amount}, Status: {transaction.Status}, Date: {transaction.CreatedAt}    \n");
                    Console.WriteLine("-----------------------------------------------------------------------------");
                }*/


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _dbContext.Dispose();
            }

            _disposed = true;
        }
        public void Dispose()
        {

            Dispose(true);
            GC.SuppressFinalize(this);
        }


    }
}
