CREATE TABLE [dbo].[BulletinBoard] (
    [BulletinBoardSeq] INT             IDENTITY (1, 1) NOT NULL,
    [Content]          NVARCHAR (1000) NOT NULL,
    [SubjectId]        INT             NOT NULL,
    [Enable]           BIT             NOT NULL,
    CONSTRAINT [PK_BulletinBoard] PRIMARY KEY CLUSTERED ([BulletinBoardSeq] ASC),
    CONSTRAINT [FK_BulletinBoard_GroupGradeSubject] FOREIGN KEY ([SubjectId]) REFERENCES [dbo].[GroupGradeSubject] ([ID])
);



