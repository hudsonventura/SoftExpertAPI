using System;
using System.Collections.Generic;



namespace SoftExpert.Workflow
{
    public class SoftExpertWorkflowApi
    {
        private readonly string _url;
        private readonly string _uriModule;
        private readonly Dictionary<string, string> _headers;

        public SoftExpertWorkflowApi(string url, string uriModule, Dictionary<string, string> headers)
        {
            _url = url;
            _uriModule = uriModule;
            _headers = headers;
        }

        /// <summary>
        /// Este método cria uma nova instância de processo de Workflow
        /// </summary>
        /// <param name="ProcessID">ID do processo</param>
        /// <param name="WorkflowTitle">Titulo da instância</param>
        /// <param name="UserID">Matrícula do usuário iniciador da instância</param>
        /// <returns>newWorkflowResponse, objeto com os campos Status, Code, Detail, RecordKey e RecordID. Se Code = 1 entao RecordID conterá o ID da intância gerada. Se Code != 1, uma SoftExpertException é gerada</returns>
        /// <exception cref="SoftExpertException"></exception>
        public newWorkflowResponse newWorkflow(string ProcessID, string WorkflowTitle, string? UserID)
        {

            string body = $@"
                <soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:urn='urn:workflow'>
                   <soapenv:Header/>
                   <soapenv:Body>
                      <urn:newWorkflow>
                         <!--You may enter the following 3 items in any order-->
                         <urn:ProcessID>{ProcessID}</urn:ProcessID>
                         <urn:WorkflowTitle>{WorkflowTitle}</urn:WorkflowTitle>
                         <!--Optional:-->
                         <urn:UserID>{UserID}</urn:UserID>
                      </urn:newWorkflow>
                   </soapenv:Body>
                </soapenv:Envelope>"
            ;
            try
            {
                var retorno = Rest.request("POST", _url, _uriModule, _headers, body);
                return newWorkflowResponse.Parse(retorno.ToString());
            }
            catch (SoftExpertException error)
            {
                error.setXMLSoapSent(body);
                throw error;
            }
            catch (Exception error)
            {
                var erro = newWorkflowResponse.Parse(error.Message);
                erro.setXMLSoapSent(body);
                return erro;
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
                var retorno = Rest.request("POST", _url, _uriModule, _headers, body);
                return editEntityRecordResponse.Parse(retorno.ToString());
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
                var retorno = Rest.request("POST", _url, _uriModule, _headers, body);
                return executeActivityResponse.Parse(retorno.ToString());
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


    }

}