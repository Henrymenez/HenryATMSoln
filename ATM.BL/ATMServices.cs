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

        public async Task deposit(int id, decimal amount)
        {
            try
            {
                SqlConnection sqlConn = await _dbContext.OpenConnection();

                string getUserInfo = $"SELECT Users.balance,Users.userId FROM Users WHERE Id = @UserId";
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
                        user.UserId = (Guid)dataReader["userId"];
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
                    Value = user.UserId,
                    SqlDbType = SqlDbType.UniqueIdentifier,
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

                string getUserInfo = $"SELECT Users.balance,Users.userId FROM Users WHERE Id = @UserId";
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
                        user.UserId = (Guid)dataReader["userId"];
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
                    Value = user.UserId,
                    SqlDbType = SqlDbType.UniqueIdentifier,
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
                //open connection
                SqlConnection sqlConn = await _dbContext.OpenConnection();

                //senders info
                string senderInfo = $"SELECT Users.balance,Users.userId FROM Users WHERE Id = @senderId";
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
                        senderObj.UserId = (Guid)dataReaderSender["userId"];
                    }
                }

                if (amount > senderObj.balance)
                {
                    Console.WriteLine("Insucficient Balance");
                    Environment.Exit(0);
                }

                //Receivers Info

                string receiverInfo = $"SELECT Users.balance,Users.userId,Users.name FROM Users WHERE Id = @receiverId";
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
                        receiverObj.UserId = (Guid)dataReaderReceiver["userId"];
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
                    Value = senderObj.UserId,
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input
                },
                 new SqlParameter
                {
                    ParameterName = "@receiverId",
                    Value = receiverObj.UserId,
                    SqlDbType = SqlDbType.UniqueIdentifier,
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

                string getUserInfo = $"SELECT Users.balance,Users.userId FROM Users WHERE Id = @UserId";
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
                        user.UserId = (Guid)dataReader["userId"];
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

       /* public async Task checkStatment(Guid id)
        {
            try
            {
                SqlConnection sqlConn = await _dbContext.OpenConnection();

                string getTransactionInfo = $"SELECT Transactions.userId,Transactions.receiverId,Transactions.desctiption,Transactions.amount,Transactions.transactionType,Transactions.status,Transactions.createdAt FROM Transactions WHERE userId = @UserId";
                await using SqlCommand command = new SqlCommand(getTransactionInfo, sqlConn);
                command.Parameters.AddRange(new SqlParameter);[]
                {
                new SqlParameter
                {
                    ParameterName = "@UserId",
                    Value = id,
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    Size = 50
                }
                }
                List<transactionViewModel> transactions = new List<transactionViewModel>();
                using (SqlDataReader dataReader = await command.ExecuteReaderAsync())
                {
                    while (dataReader.Read())
                    {
                        transactions.Add(new transactionViewModel()
                        {
                            UserId = (Guid)dataReader["userId"],
                            ReceiverId = dataReader["receiverId"].ToString() ?? " ",
                            Description = dataReader["desctiption"].ToString(),
                            TransactionType = dataReader["transactionType"].ToString(),
                            Amount = (decimal)dataReader["amount"],
                            Status = (bool)dataReader["status"],
                            CreatedAt = (DateTime)dataReader["createdAt"]
                        });


                    }
                }

                foreach (transactionViewModel transaction in transactions)
                {
                    Console.WriteLine($"User: {transaction.UserId}, Receiver: {transaction.ReceiverId}, " +
                        $"Description: {transaction.Description}, Type: {transaction.TransactionType}, " +
                        $"Amount: ${transaction.Amount}, Status: {transaction.Status}, Date: {transaction.CreatedAt}    \n");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

            }
        }*/

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
