CREATE TABLE [dbo].[StudentPosition] (
    [Account]    VARCHAR (10) NOT NULL,
    [MacAddress] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_StudentPosition] PRIMARY KEY CLUSTERED ([Account] ASC, [MacAddress] ASC),
    CONSTRAINT [FK_StudentPosition_Student] FOREIGN KEY ([Account]) REFERENCES [dbo].[Student] ([Account])
);

