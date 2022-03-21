# Moving CLE Data From Azure Synapse Dedicated SQL to Azure SQL

This is to explore how to build Synapse pipeline to copy CLE data from Synapse to Azure SQL with CLE column. 

## Introduction
Data security is a critical task for any organization, especially if you store customer personal data such as Customer contact number, email address, social security number, bank and credit card numbers. The main goal of data security is to protect unauthorized access to data within and outside the organization. To achieve this, we start by providing access to relevant persons. We still have a chance that these authorized people can also misuse the data; therefore, most of the database engines provide encryption solutions. Column level encryption (also known as Cell Level Encryption) can be used to encrypt the data for users who have access to the encryption key to access. 
CLE has been introduced into Azure for Azure SQL as well as Synapse to increase compatibility between on-premises SQL functionality and Azure data engine 

## CLE vs. TDE
**The advantages of CLE**

- Since it is column level encryption, it encrypts only the sensitive information in a table.
With CLE, the data is still encrypted even when it is loaded into memory.
- CLE allows for “explicit key management” giving you greater control over the keys and who has access to them. CLE is highly configurable, giving you a high degree of customization (especially when your applications require it).
- Queries may be faster with CLE if the encrypted column(s) is not referenced in the query. TDE will always decrypt the entire row in the table. CLE will decrypt the column value only IF it is a part of the data that is returned. So in some cases CLE implementations provide much better overall performance.

**The disadvantages of CLE**

- One of the main disadvantages of CLE is the high degree of fully manual application changes needed to use it. TDE, on the other hand, can be very simple to deploy with no changes to the database, tables or columns required.
- CLE can also have high performance penalties if search queries cannot be optimized to avoid encrypted data. “As a rough comparison, performance for a very basic query (that selects and decrypts a single encrypted column) when using cell-level encryption tends to be around 20% worse [than TDE].”

For details on setting up CLE, please read here: [Encrypt a Column of Data - SQL Server & Azure Synapse Analytics & Azure SQL Database & SQL Managed Instance | Microsoft Docs](https://docs.microsoft.com/en-us/sql/relational-databases/security/encryption/encrypt-a-column-of-data?redirectedfrom=MSDN&view=sql-server-ver15)

## Build Synapse pipeline to copy CLE data

A Synapse pipeline can be built to automate the process. The decryption and encryption can be handled within a single pipeline. (To meet the security requirements, for example cannot have decrypted PII data in the data store)

**Step 1:** Configure data source to retrieve CLE data with Symmetric Key from Synapse Dedicated SQL pool table
![](https://github.com/davidma3768/DM_Azure_Analytics/blob/main/docs/images/cle_decryption.jpg)
 
**Step 2:** Create table type and store procedure that can be used by pipeline to write encryption data to sink table

	create type [sales].[CreditCard] as table(
	    [CreditCardID] [int] NOT NULL,
	    [CardType] [nvarchar](50) NOT NULL,
	    [CardNumber] [nvarchar](25) NOT NULL,
	    [ExpMonth] [tinyint] NOT NULL,
	    [ExpYear] [smallint] NOT NULL, 
	    [ModifiedDate] [datetime] NOT NULL ,
		[CardNumber_Decrypted] [nvarchar](25) NULL
	)

	create procedure sales.spCopyCLEData (@cledata [dbo].[CreditCard] READONLY)
	as 
	begin
	OPEN SYMMETRIC KEY CreditCards_Key11  
	   DECRYPTION BY CERTIFICATE customer_cle_cert_01;  

	insert into [sales].[CreditCard]
	select [CreditCardID],[CardType],[CardNumber],[ExpMonth],[ExpYear],[ModifiedDate],
			CONVERT
			(
				varbinary(160),
				EncryptByKey
				(
					Key_GUID('CreditCards_Key11'), [CardNumber_Decrypted], 1, 
					HASHBYTES
					(
						'SHA2_256', CONVERT( varbinary , CreditCardID)
					)
				)
			) as CardNumber_Encrypted
	from @cledata
	end
	go


**Step 3:** Configure data sink to write data into Azure SQL with encryption
![](https://github.com/davidma3768/DM_Azure_Analytics/blob/main/docs/images/cle_encryption.jpg)

 
**Step 4:** Run pipeline and verify the result

	 OPEN SYMMETRIC KEY key_DataShare  
	   DECRYPTION BY CERTIFICATE cert_keyProtection;  

	SELECT CreditCardID,CardNumber, CardNumber_Encrypted  

	    AS 'Encrypted card number', CONVERT(nvarchar,  
	    DecryptByKey(CardNumber_Encrypted, 1 ,   
	    HASHBYTES('SHA2_256', CONVERT(varbinary, CreditCardID))))  
	    AS 'Decrypted card number' 
	FROM Sales.CreditCard_SharedCert;  
	GO
![](https://github.com/davidma3768/DM_Azure_Analytics/blob/main/docs/images/cle_result.jpg)

**Benifit of the solution:** all the decryption and encryption operations happen within the pipeline transant, no need to write decrypted data into a temporary table or sort. 

## Alternative

There is also option to [create identical symmetric key on two different servers](https://github.com/davidma3768/DM_Azure_Analytics/blob/main/docs/images/cle_encryption.jpg) to avoid the decrypt and encrypt CLE data within pipeline, but just read and write the encrypted data as is within the pipeline. **However, using the same symmetric key in multiple locations is not the best practice depending on customers’ security practice.**

The parameters, **KEY_SOURCE** and **IDENTITY_VALUE** can be used to create identical Symmetric key on two different data stores. 

	CREATE SYMMETRIC KEY [key_DataShare] WITH  
	    KEY_SOURCE = 'My key generation bits. This is a shared secret!',  
	    ALGORITHM = AES_256,   
	    IDENTITY_VALUE = 'Key Identity generation bits. Also a shared secret'  
	    ENCRYPTION BY CERTIFICATE [cert_keyProtection];  
	GO

With the identical Symmetric key on source data store and sink data store, a simple COPY pipeline can be used to move data without decryption first. 

![](https://github.com/davidma3768/DM_Azure_Analytics/blob/main/docs/images/cle_noencryption.jpg)
 
 
To verify the result

	OPEN SYMMETRIC KEY key_DataShare  
	   DECRYPTION BY CERTIFICATE cert_keyProtection;  

	SELECT CreditCardID,CardNumber, CardNumber_Encrypted   
	    AS 'Encrypted card number', CONVERT(nvarchar,  
	    DecryptByKey(CardNumber_Encrypted, 1 ,   
	    HASHBYTES('SHA2_256', CONVERT(varbinary, CreditCardID))))  
	    AS 'Decrypted card number' 
	FROM Sales.CreditCard_SharedCert;  
	GO
![](https://github.com/davidma3768/DM_Azure_Analytics/blob/main/docs/images/cle_result.jpg)
