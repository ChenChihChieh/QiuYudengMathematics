CREATE TABLE [dbo].[GroupGrade] (
    [ID]     INT           IDENTITY (1, 1) NOT NULL,
    [Grade]  NVARCHAR (10) NOT NULL,
    [Enable] BIT           CONSTRAINT [DF_GroupGrade_Enable] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_GroupGrade] PRIMARY KEY CLUSTERED ([ID] ASC)
);

