using System.ComponentModel.DataAnnotations;

namespace eAgenda.Dominio.ModuloDespesa;

public enum FormaPagamento
{
    [Display(Name = "PIX")] Pix,
    [Display(Name = "À Vista")] Dinheiro,
    [Display(Name = "Crédito")] Credito,
    [Display(Name = "Débito")] Debito
}