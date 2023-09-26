using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using RestSharp;
using SoftExpertAPI.Domain;
using SoftExpertAPI.Interfaces;
using SoftExpertAPI.Services;
using static SoftExpert.SoftExpertResponse;


namespace SoftExpert.Workflow;

public class SoftExpertWorkflowApi
{
    private readonly string _uriModule = "/apigateway/se/ws/wf_ws.php";
    private readonly RestClient restClient;

    private IDataBase db { get; set; }

    /// <summary>
    /// Construtor. Necessário passar a URL completa do ambiente do SE e os headers. Header Authorization é necessário
    /// </summary>
    /// <param name="url">URL completa do ambiente. Ex.: https://se.example.com.br</param>
    /// <param name="headers">Passar os headers a serem enviados na requisição. Não esqueça do header Authorization</param>
    /// <param name="db">Classe concreta que implemente a inteface SoftExpertAPI.Interfaces.IDataBase</param>
    public SoftExpertWorkflowApi(string url, Dictionary<string, string> headers, SoftExpertAPI.Interfaces.IDataBase db = null)
    {
        restClient = new RestClient(url);
        restClient.AddDefaultHeaders(headers);
        restClient.AddDefaultHeader("Host", url.Split("://")[1].Split(":")[0]);
        this.db = db;
    }

    /// <summary>
    /// Construtor. Necessário passar a URL completa do ambiente do SE e a string to Authorization incluindo o 'Basic ....'
    /// </summary>
    /// <param name="url">URL completa do ambiente. Ex.: https://se.example.com.br</param>
    /// <param name="authorization">Basic no formato base64("dominio\usuario:senha") Ex.: Basic dmMgPyB1bSBjdXJpb3Nv</param>
    public SoftExpertWorkflowApi(string url, string authorization, SoftExpertAPI.Interfaces.IDataBase db = null)
    {
        restClient = new RestClient(url);
        restClient.AddDefaultHeader("Authorization", authorization);
        restClient.AddDefaultHeader("Host", url.Split("://")[1].Split(":")[0]);
        this.db = db;
    }

    /// <summary>
    /// Este método cria uma nova instância de processo de Workflow
    /// </summary>
    /// <param name="ProcessID">ID do processo</param>
    /// <param name="WorkflowTitle">Titulo da instância</param>
    /// <param name="UserID">Matrícula do usuário iniciador da instância</param>
    /// <returns>newWorkflowResponse, objeto com os campos Status, Code, Detail, RecordKey e RecordID. Se Code = 1 entao RecordID conterá o ID da intância gerada. Se Code != 1, uma SoftExpertException é gerada</returns>
    /// <exception cref="SoftExpertException"></exception>
    public newWorkflowResponse newWorkflow(string ProcessID, string WorkflowTitle, string? UserID = null)
    {

        string body = $@"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:urn='urn:workflow'>
                               <soapenv:Header/>
                               <soapenv:Body>
                                  <urn:newWorkflow>
                                     <urn:ProcessID>{ProcessID}</urn:ProcessID>
                                     <urn:WorkflowTitle>{WorkflowTitle}</urn:WorkflowTitle>
                                     <urn:UserID>{UserID}</urn:UserID>
                                  </urn:newWorkflow>
                               </soapenv:Body>
                            </soapenv:Envelope>"
        ;

        try
        {
            RestRequest request = new RestRequest(_uriModule, Method.Post);
            request.AddHeader("SOAPAction", "urn:workflow#newWorkflow");
            request.AddParameter("text/xml", body.Trim(), ParameterType.RequestBody);

            var response = restClient.Execute(request);
            return newWorkflowResponse.Parse(response.Content);
        }
        catch (SoftExpertException error)
        {
            error.setXMLSoapSent(body);
            throw error;
        }
        catch (Exception error2)
        {
            var erro = new SoftExpertException(error2.Message);
            erro.setXMLSoapSent(body);
            throw erro;
        }

    }









    /// <summary>
    /// Este método edita os valores do formulário de uma instância de Workflow. Anexos podem ser adicionados nos campos do formulário
    /// </summary>
    /// <param name="WorkflowID">ID da instância</param>
    /// <param name="EntityID">ID da tabela do formulário</param>
    /// <param name="EntityAttributeList">Dicionário contendo os campos do formulário no formato chave - valor (pode ser nulo)</param>
    /// <param name="RelationshipList">Dicionário contendo os campos do formulário no formato chave - valor dentro de um dicionário com o ID do relacionamento na chave do dicionário superior. (pode ser nulo) </param>
    /// <param name="EntityAttributeFileList">Dicionário contendo os arquivos no formato chave - valor (byte[])</param>
    /// <returns>editEntityRecordResponse, objeto com os campos Status, Code, Detail. Se Code = 1 entao houve sucesso. Se Code != 1, uma SoftExpertException é gerada</returns>
    /// <exception cref="SoftExpertException"></exception>
    public editEntityRecordResponse editEntityRecord(string WorkflowID, string EntityID, Dictionary<string, string> EntityAttributeList = null, Dictionary<string, Dictionary<string, string>> RelationshipList = null, Dictionary<string, Anexo> EntityAttributeFileList = null)
    {
        string camposForm = Gerar_EntityAttributeList(EntityAttributeList);
        string camposRelacionamento = Gerar_RelationshipList(RelationshipList);
        string anexos = Gerar_EntityAttributeFileList(EntityAttributeFileList);
        string body = $@"
                <soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:urn='urn:workflow'>
                   <soapenv:Header/>
                   <soapenv:Body>
                      <urn:editEntityRecord>
                         <urn:WorkflowID>{WorkflowID}</urn:WorkflowID>
                         <urn:EntityID>{EntityID}</urn:EntityID>

                         <urn:EntityAttributeList>
                            {camposForm}
                         </urn:EntityAttributeList>

                         <urn:RelationshipList>
                            {camposRelacionamento}
                         </urn:RelationshipList>

                         <EntityAttributeFileList>
                            {anexos}
                        </EntityAttributeFileList> 

                      </urn:editEntityRecord>
                   </soapenv:Body>
                </soapenv:Envelope>"
        ;

        try
        {
            RestRequest request = new RestRequest(_uriModule, Method.Post);
            request.AddHeader("SOAPAction", "urn:workflow#newWorkflow");
            request.AddParameter("text/xml", body.Trim(), ParameterType.RequestBody);

            var response = restClient.Execute(request);
            return editEntityRecordResponse.Parse(response.Content);
        }
        catch (SoftExpertException error)
        {
            error.setXMLSoapSent(body);
            throw error;
        }
        catch (Exception error)
        {
            var erro = editEntityRecordResponse.Parse(error.Message);
            erro.setXMLSoapSent(body);
            return erro;
        }
    }

    private string Gerar_EntityAttributeFileList(Dictionary<string, Anexo> EntityAttributeFileList)
    {
        string anexos = String.Empty;
        if (EntityAttributeFileList is not null)
        {
            foreach (var arquivo in EntityAttributeFileList)
            {
                string base64 = Convert.ToBase64String(arquivo.Value.Content);
                anexos += $@"
                        <EntityAttributeFile>
                            <EntityAttributeID>{arquivo.Key}</EntityAttributeID>
                            <FileName>{arquivo.Value.FileName}</FileName>
                            <FileContent>{base64}</FileContent>
                        </EntityAttributeFile>
                    ";
            }
        }
        return anexos;
    }

    private string Gerar_RelationshipList(Dictionary<string, Dictionary<string, string>> RelationshipList)
    {
        string camposRelacionamento = String.Empty;
        if (RelationshipList is not null)
        {
            camposRelacionamento += $@"";
            foreach (KeyValuePair<string, Dictionary<string, string>> RelationshipAttribute in RelationshipList)
            {
                camposRelacionamento += $@"
                             <Relationship>
                                     <RelationshipID>{RelationshipAttribute.Key}</RelationshipID>
                            "
                ;

                foreach (KeyValuePair<string, string> Attribute in RelationshipAttribute.Value)
                {
                    camposRelacionamento += $@"
                                     <RelationshipAttribute>
                                             <RelationshipAttributeID>{Attribute.Key}</RelationshipAttributeID>
                                             <RelationshipAttributeValue>{Attribute.Value}</RelationshipAttributeValue>
                                     </RelationshipAttribute>
                            "
                    ;
                }
                camposRelacionamento += $@"
                             </Relationship>
                            "
                ;
            }
        }
        return camposRelacionamento;
    }

    private string Gerar_EntityAttributeList(Dictionary<string, string> EntityAttributeList) {
        string camposForm = String.Empty;
        if (EntityAttributeList is not null)
        {
            foreach (KeyValuePair<string, string> keyValues in EntityAttributeList)
            {
                camposForm += $@"
                            <urn:EntityAttribute>
                                <EntityAttributeID>{keyValues.Key}</EntityAttributeID>            
                                <EntityAttributeValue>{keyValues.Value}</EntityAttributeValue>
                            </urn:EntityAttribute>"
                ;
            }
        }
        return camposForm;
    }








    /// <summary>
    /// Este método executa uma atividade de usuário de uma instância
    /// </summary>
    /// <param name="WorkflowID">ID da instancia</param>
    /// <param name="ActivityID">ID da atividade a ser executada</param>
    /// <param name="ActionSequence">ID / Sequência da ação da atividade. </param>
    /// <param name="UserID">Matrícula do usuario executor</param>
    /// <param name="ActivityOrder"></param>
    /// <returns>executeActivityResponse, objeto com os campos Status, Code, Detail. Se Code = 1 entao houve sucesso. Se Code != 1, uma SoftExpertException é gerada</returns>
    /// <exception cref="SoftExpertException"></exception>
    public executeActivityResponse executeActivity(string WorkflowID, string ActivityID, int ActionSequence, string? UserID = null, int? ActivityOrder = null)
    {
        string body = $@"
                <soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:urn='urn:workflow'>
                   <soapenv:Header/>
                   <soapenv:Body>
                      <urn:executeActivity>
                         <!--You may enter the following 5 items in any order-->
                         <urn:WorkflowID>{WorkflowID}</urn:WorkflowID>
                         <urn:ActivityID>{ActivityID}</urn:ActivityID>
                         <urn:ActionSequence>{ActionSequence}</urn:ActionSequence>
                         <!--Optional:-->
                         <urn:UserID>{UserID}</urn:UserID>
                         <!--Optional:-->
                         <urn:ActivityOrder>{ActivityOrder}</urn:ActivityOrder>
                      </urn:executeActivity>
                   </soapenv:Body>
                </soapenv:Envelope>"
        ;
        try
        {
            RestRequest request = new RestRequest(_uriModule, Method.Post);
            request.AddHeader("SOAPAction", "urn:workflow#newWorkflow");
            request.AddParameter("text/xml", body.Trim(), ParameterType.RequestBody);

            var response = restClient.Execute(request);
            return executeActivityResponse.Parse(response.Content);
        }
        catch (SoftExpertException error)
        {
            error.setXMLSoapSent(body);
            throw error;
        }
        catch (Exception error)
        {
            var erro = executeActivityResponse.Parse(error.Message);
            erro.setXMLSoapSent(body);
            return erro;
        }
    }

    /// <summary>
    /// Este método anexa um arquivo no menu de anexo do lado esquerdo de uma instancia
    /// </summary>
    /// <param name="WorkflowID">ID da instancia</param>
    /// <param name="ActivityID">ID da atividade a ser executada</param>
    /// <param name="File">Arquivo a ser anexado</param>
    /// <returns></returns>
    public newAttachmentResponse newAttachment(string WorkflowID, string ActivityID, Anexo File, string UserID = null)
    {
        string base64 = Convert.ToBase64String(File.Content);
        string body = $@"
                <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:urn=""urn:workflow"">
                   <soapenv:Header/>
                   <soapenv:Body>
                      <urn:newAttachment>
                         <urn:WorkflowID>{WorkflowID}</urn:WorkflowID>
                         <urn:ActivityID>{ActivityID}</urn:ActivityID>
                         <urn:FileName>{File.FileName}</urn:FileName>
                         <urn:FileContent>{base64}</urn:FileContent>
                         <!--Optional:-->
                         <urn:UserID>{UserID}</urn:UserID>
                      </urn:newAttachment>
                   </soapenv:Body>
                </soapenv:Envelope>
                "
        ;
        try
        {
            RestRequest request = new RestRequest(_uriModule, Method.Post);
            request.AddHeader("SOAPAction", "urn:workflow#newAttachment");
            request.AddParameter("text/xml", body.Trim(), ParameterType.RequestBody);

            var response = restClient.Execute(request);
            return newAttachmentResponse.Parse(response.Content);
        }
        catch (SoftExpertException error)
        {
            error.setXMLSoapSent(body);
            throw error;
        }
        catch (Exception error)
        {
            var erro = newAttachmentResponse.Parse(error.Message);
            erro.setXMLSoapSent(body);
            return erro;
        }
    }


    /// <summary>
    /// Este método anexa umalista de arquivos no menu de anexo do lado esquerdo de uma instancia
    /// </summary>
    /// <param name="WorkflowID">ID da instancia</param>
    /// <param name="ActivityID">ID da atividade a ser executada</param>
    /// <param name="Files">Lista de arquivos no formato List<Anexo></param>
    /// <returns></returns>
    public newAttachmentResponse newAttachment(string WorkflowID, string ActivityID, List<Anexo> Files, string UserID = null)
    {
        foreach (Anexo File in Files) {
            if (File is null)
            {
                throw new SoftExpertException("Um dos itens da lista é nulo. Então a comunicação com o SE não foi inciada. Verifique sua lista de arquivos e tente novamente.");
            }
            if (File.Content is null || File.Content.Length == 0)
            {
                throw new SoftExpertException("Um dos itens da lista possui o conteúdo nulo ou vazio. Então a comunicação com o SE não foi inciada. Verifique sua lista de arquivos e tente novamente.");
            }
            if (File.FileName is null || File.FileName.Length == 0)
            {
                throw new SoftExpertException("Um dos itens da lista não possui o nome do arquivo (FileName), ou este é vazio. Então a comunicação com o SE não foi inciada. Verifique sua lista de arquivos e tente novamente.");
            }
        }
        newAttachmentResponse retorno = new newAttachmentResponse();
        foreach (Anexo File in Files) {
            try
            {
                retorno = newAttachment(WorkflowID, ActivityID, File, UserID);
            }
            catch (Exception error)
            {
                string detail = $"A iteração parou no arquivo {File.FileName} por conta do erro: {error.Message}";
                throw new SoftExpertException(detail, retorno);
            }
            
        }
        return new newAttachmentResponse() { 
            Code = 1,
            Detail = "Todos os arquivos foram anexados com sucesso",
            Status = SoftExpertResponse.STATUS.SUCCESS
        };
    }







    /// <summary>
    /// Este método permite você criar itens de uma grid de um formulário principal
    /// </summary>
    /// <param name="ProcessID">ID do processo</param>
    /// <param name="WorkflowTitle">Titulo da instância</param>
    /// <param name="UserID">Matrícula do usuário iniciador da instância</param>
    /// <returns>newWorkflowResponse, objeto com os campos Status, Code, Detail, RecordKey e RecordID. Se Code = 1 entao RecordID conterá o ID da intância gerada. Se Code != 1, uma SoftExpertException é gerada</returns>
    /// <exception cref="SoftExpertException"></exception>
    public newChildEntityRecordResponse newChildEntityRecord(string WorkflowID, string MainEntityID, string ChildRelationshipID, Dictionary<string, string> EntityAttributeList = null, Dictionary<string, Dictionary<string, string>> RelationshipList = null, Dictionary<string, Anexo> EntityAttributeFileList = null)
    {
        string camposForm = Gerar_EntityAttributeList(EntityAttributeList);
        string camposRelacionamento = Gerar_RelationshipList(RelationshipList);
        string anexos = Gerar_EntityAttributeFileList(EntityAttributeFileList);
        string body = $@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:urn=""urn:workflow"">
                                <soapenv:Header/>
                                <soapenv:Body>
                                    <urn:newChildEntityRecord>
                                        <!--You may enter the following 6 items in any order-->
                                        <urn:WorkflowID>{WorkflowID}</urn:WorkflowID>
                                        <urn:MainEntityID>{MainEntityID}</urn:MainEntityID>
                                        <urn:ChildRelationshipID>{ChildRelationshipID}</urn:ChildRelationshipID>
                                        
                                        <urn:EntityAttributeList>
                                            {camposForm}
                                        </urn:EntityAttributeList>

                                        <urn:RelationshipList>
                                            {camposRelacionamento}
                                        </urn:RelationshipList>

                                        <EntityAttributeFileList>
                                            {anexos}
                                        </EntityAttributeFileList>
                                        
                                    </urn:newChildEntityRecord>
                                </soapenv:Body>
                            </soapenv:Envelope>"
        ;

        try
        {
            RestRequest request = new RestRequest(_uriModule, Method.Post);
            request.AddHeader("SOAPAction", "urn:workflow#newChildEntityRecord");
            request.AddParameter("text/xml", body.Trim(), ParameterType.RequestBody);

            var response = restClient.Execute(request);
            return newChildEntityRecordResponse.Parse(response.Content);
        }
        catch (SoftExpertException error)
        {
            error.setXMLSoapSent(body);
            throw error;
        }
        catch (Exception error2)
        {
            var erro = new SoftExpertException(error2.Message);
            erro.setXMLSoapSent(body);
            throw erro;
        }

    }

    /// <summary>
    /// Traz os arquivos anexados do lado esquerdo de uma instancia. Pode especificar a atividade em que o arquivo fora anexado. Obrigatório implementar o parametro db.
    /// </summary>
    /// <param name="WorkflowID">ID da instância</param>
    /// <param name="ActivityID">ID da atividade (opcional)</param>
    /// <returns>Lista de objetos da classe Anexo, contendo nome do arquivo, cdfile e conteúdo em byte[]</returns>
    /// <exception cref="SoftExpertException"></exception>
    /// <exception cref="Exception"></exception>
    public List<Anexo> listAttachmentFromInstance(string WorkflowID, string ActivityID = "") {
        if (db is null) {
            throw new SoftExpertException("Uma instancia de banco de dados não foi informada na criação deste objeto. Crie forneça uma conexão com seu banco de dados implementando a interface SoftExpertAPI.Interfaces.IDataBase");
        }

        Dictionary<string, dynamic> parametros = new Dictionary<string, dynamic>();
        if (ActivityID != "") {
            parametros.Add(":ActivityID", ActivityID);
            ActivityID = "and a.idstruct = :ActivityID";
        }

        string sql = @$"SELECT g.NMFILE, g.CDFILE, ANEXO.CDATTACHMENT, anexo.NMUSERUPD,
				        substr(g.NMFILE, 0, INSTR(g.NMFILE, '.', -1)-1) AS NOME,
				        substr(g.NMFILE, INSTR(g.NMFILE, '.', -1)+1) AS EXT,
				        g.NRSIZE, g.FLFILE
                        --
                        from wfprocess p
                        JOIN WFSTRUCT A ON A.IDPROCESS = P.IDOBJECT
                        left JOIN WFPROCATTACHMENT ATAASSOC ON A.IDOBJECT = ATAASSOC.IDSTRUCT
                        left JOIN ADATTACHMENT ANEXO ON ATAASSOC.CDATTACHMENT = ANEXO.CDATTACHMENT
                        join ADATTACHFILE a on ANEXO.CDATTACHMENT = a.CDATTACHMENT
                        join GNCOMPFILECONTCOPY c on a.CDCOMPLEXFILECONT = c.CDCOMPLEXFILECONT
                        join gnfile g on c.CDCOMPLEXFILECONT = g.CDCOMPLEXFILECONT
                        --
                        where ANEXO.CDATTACHMENT IS NOT NULL AND p.idprocess = :WorkflowID {ActivityID}
                        order by P.idprocess DESC";

        parametros.Add(":WorkflowID", WorkflowID);

        DataTable list = null;
        try
        {
            list = db.Query(sql, parametros);
        }
        catch (Exception erro)
        {
            throw new Exception($"Falha ao buscar os arquivo no bando de dados. Erro: {erro.Message}");
        }
        

        List<Anexo> retorno = new List<Anexo>();
        foreach (DataRow arquivo in list.Rows) {
            try
            {
                Anexo anexo = new Anexo();
                var stream = arquivo["FLFILE"];
                
                anexo.FileName = arquivo["NMFILE"].ToString();
                anexo.cdfile = Int64.Parse(arquivo["CDFILE"].ToString());
                anexo.cdattachment = Int64.Parse(arquivo["CDATTACHMENT"].ToString());
                var contentZip = (byte[])stream;
                var content = Zip.UnzipFile(contentZip);
                anexo.Content = content;
                retorno.Add(anexo);
            }
            catch (Exception erro)
            {
                throw new Exception($"Falha ao descompactar o arquivo '{arquivo["NMFILE"]}'. Erro: {erro.Message}");
            }
            
        }
        return retorno;
    }



}


