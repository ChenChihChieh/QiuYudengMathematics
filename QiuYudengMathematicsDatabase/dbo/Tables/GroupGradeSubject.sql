CREATE TABLE [dbo].[GroupGradeSubject] (
    [ID]      INT           IDENTITY (1, 1) NOT NULL,
    [GradeID] INT           NOT NULL,
    [Subject] NVARCHAR (10) NOT NULL,
    [Enable]  BIT           CONSTRAINT [DF_GroupGradeSubject_Enable] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_GroupGradeSubject] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_GroupGradeSubject_GroupGrade] FOREIGN KEY ([GradeID]) REFERENCES [dbo].[GroupGrade] ([ID])
);

