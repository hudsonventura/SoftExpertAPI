using System;

namespace Domain;

public class WFStruct
{
    public string idstruct { get; set; } = string.Empty;
    public string nmstruct { get; set; } = string.Empty;

    public WFStatus fgstatus { get; set; } = 0;
    public DateTime dhenabled { get; set; } = DateTime.MinValue; //TODO: needs implementation
    public DateTime dtestimatedfinish { get; set; } = DateTime.MinValue; //TODO: needs implementation
    public DateTime dtexecution { get; set; } = DateTime.MinValue; //TODO: needs implementation




    public enum WFStatus
    {
        Atividade_Nao_Iniciada = 1,
        Em_Execucao = 2,
        Executada = 3,
        Suspenso = 4,
        Cancelado = 5,
        Recusado = 6,
        AprovaçãoRetorno = 7,
    }
}
