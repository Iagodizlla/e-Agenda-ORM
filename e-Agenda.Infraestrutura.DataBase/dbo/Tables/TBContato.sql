CREATE TABLE [dbo].[TBContato] (
    [Id]       UNIQUEIDENTIFIER NOT NULL,
    [Nome]     NVARCHAR (100)   NOT NULL,
    [Telefone] NVARCHAR (20)    NOT NULL,
    [Email]    NVARCHAR (100)   NOT NULL,
    [Empresa]  NVARCHAR (100)   NULL,
    [Cargo]    NVARCHAR (100)   NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

