CREATE TABLE [dbo].[CourseVIdeoProgress] (
    [Account]   VARCHAR (10)    NOT NULL,
    [CourseSeq] INT             NOT NULL,
    [Progress]  DECIMAL (18, 2) NOT NULL,
    CONSTRAINT [PK_CourseVIdeoProgress] PRIMARY KEY CLUSTERED ([Account] ASC, [CourseSeq] ASC),
    CONSTRAINT [FK_CourseVIdeoProgress_CourseVideo] FOREIGN KEY ([CourseSeq]) REFERENCES [dbo].[CourseVideo] ([CourseSeq]),
    CONSTRAINT [FK_CourseVIdeoProgress_Student] FOREIGN KEY ([Account]) REFERENCES [dbo].[Student] ([Account])
);

