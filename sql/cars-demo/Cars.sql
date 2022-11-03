CREATE TABLE [dbo].[Cars]
(
  [Id] [uniqueidentifier] NOT NULL,
  [Make] [nvarchar](100) NOT NULL,
  [Model] [nvarchar](100) NOT NULL,
  [Year] [int] NOT NULL,
  [Color] [nvarchar](100) NOT NULL,
  PRIMARY KEY CLUSTERED ([Id] ASC)
)
