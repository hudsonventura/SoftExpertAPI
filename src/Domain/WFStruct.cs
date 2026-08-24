using System;

namespace Domain;

public class WFStruct
{
    internal string idprocess { get; set; } = string.Empty;
    internal string idobject { get; set; } = string.Empty;

    public string idstruct { get; set; } = string.Empty;
    public string nmstruct { get; set; } = string.Empty;

    public STStatus fgstatus { get; set; } = 0;
    public DateTime dhenabled { get; set; } = DateTime.MinValue; //TODO: needs implementation
    public DateTime dtestimatedfinish { 
        get{ return _dtestimatedfinish;}
        set{
            _dtestimatedfinish = value;
            
            //por default, o status é em dia
            if(status_prazo == StatusPrazo.Nao_Informado)
                status_prazo = StatusPrazo.Em_Dia;

            //se houver um prazo e agora for maior que o prazo, o status é atraso
            if(_dtestimatedfinish > DateTime.MinValue && DateTime.Now > _dtestimatedfinish)
            {
                status_prazo = StatusPrazo.Em_Atraso;             
            }
        }
    }
    private DateTime _dtestimatedfinish = DateTime.MinValue;

    public DateTime dtexecution { 
        get{ return _dtexecution;}
        set{
            _dtexecution = value;

            //por default, o status é em dia
            if(status_prazo == StatusPrazo.Nao_Informado)
                status_prazo = StatusPrazo.Em_Dia;


            //se houver um prazo e a execução for maior que o prazo, o status é atraso
            if(dtestimatedfinish > DateTime.MinValue && _dtexecution > dtestimatedfinish)
            {
                status_prazo = StatusPrazo.Em_Atraso;             
            }
        }
    }
    public DateTime _dtexecution = DateTime.MinValue;

    public StatusPrazo status_prazo { get; private set; } = StatusPrazo.Nao_Informado;



    public enum WFStatus
    {
        Em_Andamento = 1,
        Cancelado = 3,
        Encerrado = 4,
        Suspenso = 2
    }

    public enum STStatus
    {
        A_Iniciar = 1,
        Em_Andamento = 2,
        Cancelada = 5,
        Executada = 3,
        Postergada = 4,
        Rejeitada = 6,
        Aprovacao_Retornada = 7
        
    }


    public enum StatusPrazo
    {
        Nao_Informado = 0,
        Em_Dia = 1,
        Em_Atraso = 2
    }
}
