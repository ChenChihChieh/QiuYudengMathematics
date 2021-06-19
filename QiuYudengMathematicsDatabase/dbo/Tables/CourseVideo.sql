CREATE TABLE [dbo].[CourseVideo] (
    [CourseSeq]  INT           IDENTITY (1, 1) NOT NULL,
    [CourseName] NVARCHAR (50) NOT NULL,
    [Url]        VARCHAR (MAX) NOT NULL,
    [SubjectId]  INT           NOT NULL,
    [Enable]     BIT           NOT NULL,
    CONSTRAINT [PK_CourseVideo] PRIMARY KEY CLUSTERED ([CourseSeq] ASC)
);



