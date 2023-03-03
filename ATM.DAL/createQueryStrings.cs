namespace ATM.DAL
{
    public static class createQueryStrings
    {

        public static string atmTableString { get; } = @"USE AtmDB; if not exists (select * from sysobjects where name='AtmInfo' and xtype='U')CREATE TABLE AtmInfo(
					Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
					Name VARCHAR(60) NOT NULL,
					AvailableCash DECIMAL NOT NULL,
				);";

        public static string UserTableString { get; } = @"USE AtmDB; if not exists (select * from sysobjects where name='Users' and xtype='U') CREATE TABLE Users (
					Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
					Name VARCHAR(100) NOT NULL,
					Email VARCHAR(100) NOT NULL,
					PhoneNumber VARCHAR(20) NOT NULL,
					AccountNo VARCHAR(50) NOT NULL UNIQUE,
					AccountType VARCHAR(50) NOT NULL,
				Pin	VARCHAR(4) NOT NULL,
CardNumber VARCHAR(100) NOT NULL UNIQUE,
					Balance DECIMAL NOT NULL,
status BIT NOT NULL, 
					);";
        public static string transactionTableString { get; } = @"USE AtmDB; if not exists (select * from sysobjects where name='Transactions' and xtype='U') 
CREATE TABLE Transactions( 
					id INT UNIQUE IDENTITY(1,1) NOT NULL,
                    userId INT NOT NULL,
                    receiverId INT NULL, 
                    transactionType VARCHAR(50) NOT NULL,
					desctiption TEXT NOT NULL,
                    amount DECIMAL(38,2) NOT NULL, 
					status BIT NOT NULL, 
                    createdAt DATETIME NOT NULL,
                   FOREIGN KEY (userId) REFERENCES Users (Id),
                   FOREIGN KEY (receiverId) REFERENCES Users (Id),
                   PRIMARY KEY(Id))";
        public static string checkIfEmpty { get; } = @"USE AtmDB; SELECT COUNT(*) FROM Users";
        public static string createUserSql { get; } = @"USE AtmDB; INSERT INTO Users 
             (Name, Email, PhoneNumber, AccountNo, AccountType,Pin, CardNumber,Balance,Status)
               VALUES ('Dav Hart','dav@gmail.com','09096026989','1234554321','Savings','2222','1234567890',500000000.00,1),
                ('Rayn Jim', 'jim@gmail.com','08033277629','6789009876','Savings','5555','0987654321',800000.17,1)";

    }
}
