﻿using SoftExpert.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples;

internal class editEntityRecordExample
{
    private SoftExpertWorkflowApi wfAPI;

    public editEntityRecordExample(SoftExpertWorkflowApi wfAPI)
    {
        this.wfAPI = wfAPI;
    }

    
    internal void Main()
    {
        string EntityID = "SOLCLIENTEFORNE";    //ID da tabela (entidade)
        string WorkflowID = "CCF202323106";     //ID da instancia

        //os campos do fomrulário devem ser um dictionay de strings/strings, sendo nomeCampo/valorCampo
        Dictionary<string, string> formulario = new Dictionary<string, string>();
        formulario.Add("possuiendereco", "1"); //id do campo do formulário e valor (em string)
        formulario.Add("ramal", "N/A");

        //os relacionamentos (selectbox) a serem prenchidos
        Dictionary<string, Dictionary<string, string>> relacionamentos = new Dictionary<string, Dictionary<string, string>>();
        relacionamentos.Add("tipocliente", //idrelacionamento
            new Dictionary<string, string>() {
            //{ "campodoformdorelacionamento", "valor" },
            { "tipo", "PESSOA JURIDICA (CNPJ)" },
            }
        );

        

        editEntityRecordResponse entityResponse;
        try
        {
            entityResponse = wfAPI.editEntityRecord(WorkflowID, EntityID, formulario, relacionamentos);
        }
        catch (Exception erro)
        {
            Console.WriteLine($"Não foi possivel editar o formulário. Erro: {erro.Message}");
            return;
        }
        int sucessoEntity = entityResponse.Code;
        string detalhesEntity = entityResponse.Detail;
    }
}