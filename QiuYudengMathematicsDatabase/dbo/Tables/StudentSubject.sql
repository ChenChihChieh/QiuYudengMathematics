CREATE TABLE [dbo].[StudentSubject] (
    [Account]   VARCHAR (10) NOT NULL,
    [SubjectID] INT          NOT NULL,
    CONSTRAINT [PK_StudentSubject] PRIMARY KEY CLUSTERED ([Account] ASC, [SubjectID] ASC),
    CONSTRAINT [FK_StudentSubject_GroupGradeSubject] FOREIGN KEY ([SubjectID]) REFERENCES [dbo].[GroupGradeSubject] ([ID]),
    CONSTRAINT [FK_StudentSubject_Student] FOREIGN KEY ([Account]) REFERENCES [dbo].[Student] ([Account])
);

