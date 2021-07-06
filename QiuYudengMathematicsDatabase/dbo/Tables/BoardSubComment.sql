CREATE TABLE [dbo].[BoardSubComment] (
    [Seq]        INT            NOT NULL,
    [CommentSeq] INT            NOT NULL,
    [Account]    VARCHAR (10)   NOT NULL,
    [Comment]    NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_BoardSubLeaveComment] PRIMARY KEY CLUSTERED ([Seq] ASC),
    CONSTRAINT [FK_BoardSubComment_BoardComment] FOREIGN KEY ([CommentSeq]) REFERENCES [dbo].[BoardComment] ([Seq])
);

