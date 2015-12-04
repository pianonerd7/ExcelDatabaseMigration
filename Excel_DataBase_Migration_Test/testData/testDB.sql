IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'testDB') CREATE DATABASE testDB;
If Exists(Select object_id From sys.tables Where name = 'testDBTable') Drop Table testDBTable;
USE testDB;
CREATE TABLE testDBTable (RowID int IDENTITY (1,1) PRIMARY KEY, Name text, Gender text, Salary text);