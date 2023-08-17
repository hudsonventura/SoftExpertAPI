using SoftExpert.Workflow;
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

    internal void Main() {
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
}
