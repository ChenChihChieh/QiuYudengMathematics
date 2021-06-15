CREATE TABLE [dbo].[StudentDevice] (
    [Account] VARCHAR (10)  NOT NULL,
    [Device]  VARCHAR (500) NOT NULL,
    CONSTRAINT [PK_StudentDevice] PRIMARY KEY CLUSTERED ([Account] ASC, [Device] ASC),
    CONSTRAINT [FK_StudentDevice_Student] FOREIGN KEY ([Account]) REFERENCES [dbo].[Student] ([Account])
);

