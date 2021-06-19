CREATE TABLE [dbo].[LogError] (
    [Seq]        INT            IDENTITY (1, 1) NOT NULL,
    [ErrMsg]     NVARCHAR (MAX) NOT NULL,
    [StackTrace] VARCHAR (MAX)  NOT NULL,
    [CreateDate] DATETIME       NOT NULL,
    CONSTRAINT [PK_ErrorTable] PRIMARY KEY CLUSTERED ([Seq] ASC)
);

