CREATE TABLE [dbo].[BoardComment] (
    [Seq]              INT            NOT NULL,
    [BulletinBoardSeq] INT            NOT NULL,
    [Account]          VARCHAR (10)   NOT NULL,
    [Comment]          NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_BoardLeaveComment] PRIMARY KEY CLUSTERED ([Seq] ASC),
    CONSTRAINT [FK_BoardComment_BulletinBoard] FOREIGN KEY ([BulletinBoardSeq]) REFERENCES [dbo].[BulletinBoard] ([BulletinBoardSeq])
);

