CREATE TABLE Customer(
	Id int not null identity,
	FirstName nvarchar(20) not null,
	LastName nvarchar(20) not null,
	Document nvarchar(20) not null,
	DocumentType int not null,
	Email nvarchar(160) null,
	CONSTRAINT PK_customer PRIMARY KEY (Id)
)
GO