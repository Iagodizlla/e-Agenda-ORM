SELECT
	COMP.[Id],
	COMP.[Assunto],
	COMP.[Data],
	COMP.[HoraInicio],
	COMP.[HoraTermino],
	COMP.[Tipo],
	COMP.[Local],
	COMP.[Link],
	COMP.[Contato_Id],
	CT.[NOME],
	CT.[EMAIL],
	CT.[TELEFONE],
	CT.[CARGO],
	CT.[EMPRESA]
FROM
	[TBCompromisso] AS COMP LEFT JOIN
	[TBContato] AS CT
ON
	CT.Id = COMP.Contato_Id;