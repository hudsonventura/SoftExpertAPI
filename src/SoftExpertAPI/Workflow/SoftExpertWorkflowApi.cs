using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using RestSharp;


namespace SoftExpert.Workflow
{
    public class SoftExpertWorkflowApi
    {
        private readonly string _uriModule = "/apigateway/se/ws/wf_ws.php";
        private readonly RestClient restClient;

        /// <summary>
        /// Construtor. Necessário passar a URL completa do ambiente do SE e os headers. Header Authorization é necessário
        /// </summary>
        /// <param name="url">URL completa do ambiente. Ex.: https://se.example.com.br</param>
        /// <param name="headers">Passar os headers a serem enviados na requisição. Não esqueça do header Authorization</param>
        public SoftExpertWorkflowApi(string url, Dictionary<string, string> headers)
        {
            restClient = new RestClient(url);
            restClient.AddDefaultHeaders(headers);
            restClient.AddDefaultHeader("Host", url.Split("://")[1].Split(":")[0]);
        }

        /// <summary>
        /// Construtor. Necessário passar a URL completa do ambiente do SE e a string to Authorization incluindo o 'Basic ....'
        /// </summary>
        /// <param name="url">URL completa do ambiente. Ex.: https://se.example.com.br</param>
        /// <param name="authorization">Basic no formato base64("dominio\usuario:senha") Ex.: Basic dmMgPyB1bSBjdXJpb3Nv</param>
        public SoftExpertWorkflowApi(string url, string authorization)
        {
            restClient = new RestClient(url);
            restClient.AddDefaultHeader("Authorization", authorization);
            restClient.AddDefaultHeader("Host", url.Split("://")[1].Split(":")[0]);
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
        /// Este método edita os valores do formulário de uma instância de Workflow
        /// </summary>
        /// <param name="WorkflowID">ID da instância</param>
        /// <param name="EntityID">ID da tabela do formulário</param>
        /// <param name="EntityAttributeList">Dicionário (tipo Dictionary do dotnet) contendo os campos do formulário no formato chave - valor (pode ser nulo)</param>
        /// /// <param name="RelationshipList">Dicionário (tipo Dictionary do dotnet) contendo os campos do formulário no formato chave - valor dentro de um dicionário com o ID do relacionamento na chave do dicionário superior. (pode ser nulo) </param>
        /// <returns>editEntityRecordResponse, objeto com os campos Status, Code, Detail. Se Code = 1 entao houve sucesso. Se Code != 1, uma SoftExpertException é gerada</returns>
        /// <exception cref="SoftExpertException"></exception>
        public editEntityRecordResponse editEntityRecord(string WorkflowID, string EntityID, Dictionary<string, string> EntityAttributeList = null, Dictionary<string, Dictionary<string, string>> RelationshipList = null)
        {

            string camposForm = "";
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

            string camposRelacionamento = "";
            if (RelationshipList is not null)
            {
                camposRelacionamento += $@"<urn:RelationshipList>";
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
                camposRelacionamento += $@"</urn:RelationshipList>";
            }




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
                               {camposRelacionamento}
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
        /// <param name="FileName">Nome do arquivo com a extensão. Ex.: importante.docx</param>
        /// <param name="FileContent">byte[] FileConten array de bytes. Abra com: byte[] FileContent = File.ReadAllBytes(filePath); </param>
        /// <returns></returns>
        public newAttachmentResponse newAttachment(string WorkflowID, string ActivityID, string FileName, byte[] FileContent)
        {
            

            return new newAttachmentResponse();
        }
    }

}