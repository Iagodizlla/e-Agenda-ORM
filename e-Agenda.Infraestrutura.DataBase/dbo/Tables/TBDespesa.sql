CREATE TABLE [dbo].[TBDespesa] (
    [Id]             UNIQUEIDENTIFIER NOT NULL,
    [Descricao]      NVARCHAR (100)   NOT NULL,
    [Valor]          DECIMAL (18, 2)  NOT NULL,
    [DataOcorrencia] DATETIME2 (7)    NOT NULL,
    [FormaPagamento] INT              NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

