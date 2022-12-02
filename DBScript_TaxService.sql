CREATE DATABASE TaxService;
GO

USE TaxService;
GO

BEGIN
BEGIN TRY
	BEGIN TRAN 
		CREATE TABLE Tax (
			TaxID int IDENTITY(1,1) NOT NULL,
			Municipality varchar(100) NULL,
			TaxType varchar(100) NULL,
			TaxRule int NOT NULL DEFAULT(1),
			FromDate Date NULL,
			ToDate Date NULL,
			IndividualDates varchar(100) NULL,
			TaxApplied decimal(2,1) NOT NULL,
			PRIMARY KEY(TaxID)
		);

		INSERT INTO Tax (Municipality,TaxType,TaxRule,FromDate,ToDate,IndividualDates,TaxApplied) VALUES 
		('Vilnius','Yearly',2,'2020-01-01','2020-12-31',null,0.2),
		('Vilnius','Monthly',2,'2020-05-01','2020-05-31',null,0.4),
		('Vilnius','Daily',2,null,null,'2020-01-01,2020-12-25',0.1),
		('Kaunas','Yearly',1,'2020-01-01','2020-12-31',null,0.3),
		('Kaunas','Monthly',1,'2020-01-01','2020-01-31',null,0.2),
		('Kaunas','Weekly',1,'2020-01-06','2020-01-12',null,0.3)

	COMMIT TRAN 

END TRY
BEGIN catch 
    IF ( Xact_state() ) = -1 
      BEGIN 
          -- PRINT N'The transaction is in an uncommittable state. Rolling back transaction.' 
          ROLLBACK TRANSACTION; 
      END; 

    IF ( Xact_state() ) = 1 
      BEGIN 
          -- PRINT N'The transaction is committable. Committing transaction.' 
          COMMIT TRANSACTION; 
      END; 
	  
    THROW ;
END catch 
END