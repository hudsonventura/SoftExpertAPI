using SoftExpert.Workflow;
using SoftExpertAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples;

internal class newAttachmentExample
{
    private SoftExpertWorkflowApi wfAPI;
    public newAttachmentExample(SoftExpertWorkflowApi wfAPI)
    {
        this.wfAPI = wfAPI;
    }

    internal void Exemplo1_ArquivoUnico() {
        string filePath = "120.png";                                           
        byte[] FileContent = File.ReadAllBytes(filePath);   //Binário do arquivo
        string ActivityID = "ATIV-SOLCCF";                  //ID da atividade em que o arquivo será anexado
        string WorkflowID = "CCF202323106";                 //ID da instancia
        Anexo Arquivo = new Anexo() {
            FileName = "logo.png",  //Nome do arquivo com a extensão
            Content = FileContent   //Binário do arquivo
        };

        newAttachmentResponse newAttachment;
        try
        {
            newAttachment = wfAPI.newAttachment(WorkflowID, ActivityID, Arquivo);
        }
        catch (Exception erro)
        {
            Console.WriteLine($"Não foi possivel anexar o arquivo a instancia de Workflow. Erro: {erro.Message}");
            return;
        }
    }

    internal void Exemplo2_VariosArquivos()
    {
        string ActivityID = "ATIV-SOLCCF";                  //ID da atividade em que o arquivo será anexado
        string WorkflowID = "CCF202323106";                 //ID da instancia
        List<Anexo> Arquivos = new List<Anexo>(){ 
            new Anexo()
            {
                FileName = "logo1.png",  //Nome do arquivo com a extensão
                Content = File.ReadAllBytes("120.png")   //Binário do arquivo
            },
            new Anexo()
            {
                FileName = "",  //Nome do arquivo com a extensão
                Content = File.ReadAllBytes("largeLogo4.jpg")   //Binário do arquivo
            }
        };

        newAttachmentResponse newAttachment;
        try
        {
            newAttachment = wfAPI.newAttachment(WorkflowID, ActivityID, Arquivos);
        }
        catch (Exception erro)
        {
            Console.WriteLine($"Não foi possivel anexar o arquivo a instancia de Workflow. Erro: {erro.Message}");
            return;
        }
    }
}
