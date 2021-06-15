CREATE TABLE [dbo].[Student] (
    [Account]  VARCHAR (10)   NOT NULL,
    [Pwd]      VARCHAR (2000) NOT NULL,
    [Name]     NVARCHAR (10)  NOT NULL,
    [Grade]    INT            NOT NULL,
    [Enable]   BIT            NOT NULL,
    [PwdReset] BIT            CONSTRAINT [DF_Student_PwdReset] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([Account] ASC),
    CONSTRAINT [FK_Student_GroupGrade] FOREIGN KEY ([Grade]) REFERENCES [dbo].[GroupGrade] ([ID])
);



