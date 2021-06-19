CREATE TABLE [dbo].[GroupCourseStudent] (
    [CourseSeq] INT          NOT NULL,
    [Account]   VARCHAR (10) NOT NULL,
    CONSTRAINT [PK_GroupCourseStudent] PRIMARY KEY CLUSTERED ([CourseSeq] ASC, [Account] ASC),
    CONSTRAINT [FK_GroupCourseStudent_CourseVideo] FOREIGN KEY ([CourseSeq]) REFERENCES [dbo].[CourseVideo] ([CourseSeq]),
    CONSTRAINT [FK_GroupCourseStudent_Student] FOREIGN KEY ([Account]) REFERENCES [dbo].[Student] ([Account])
);

