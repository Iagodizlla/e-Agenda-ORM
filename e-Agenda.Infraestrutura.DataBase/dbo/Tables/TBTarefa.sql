CREATE TABLE [dbo].[TBTarefa] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
    [Titulo]        NVARCHAR (100)   NOT NULL,
    [Prioridade]    INT              NOT NULL,
    [DataCriacao]   DATETIME2 (7)    NOT NULL,
    [DataConclusao] DATETIME2 (7)    NULL,
    [Concluida]     BIT              NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

