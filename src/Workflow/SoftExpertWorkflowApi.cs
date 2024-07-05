using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Domain;
using Newtonsoft.Json;



namespace SoftExpert.Workflow;

public class SoftExpertWorkflowApi : SoftExpertBaseAPI
{
    public SoftExpertWorkflowApi(string baseUrl, Dictionary<string, string> headers, IDataBase db = null, string login = null, string pass = null, string domain = null) : base(baseUrl, headers, db, login, pass, domain)
    {
    }

    public SoftExpertWorkflowApi(string baseUrl, string authorization, IDataBase db = null, string login = null, string pass = null, string domain = null) : base(baseUrl, authorization, db, login, pass, domain)
    {
    }

    protected override void SetUriModule()
    {
        _uriModule = "/apigateway/se/ws/wf_ws.php";
    }



    


    /// <summary>
    /// Este método cria uma nova instância de processo de Workflow
    /// </summary>
    /// <param name="ProcessID">ID do processo</param>
    /// <param name="WorkflowTitle">Titulo da instância</param>
    /// <param name="UserID">Matrícula do usuário iniciador da instância</param>
    /// <returns>newWorkflowResponse, objeto com os campos Status, Code, Detail, RecordKey e RecordID. Se Code = 1 entao RecordID conterá o ID da intância gerada. Se Code != 1, uma SoftExpertException é gerada</returns>
    /// <exception cref="SoftExpertException"></exception>
    public string newWorkflow(string ProcessID, string WorkflowTitle, string? UserID = null)
    {
        //BUG: dependendo dos caracteres do WorkflowTitle, a instancia não pode ser criada.
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

        var se_response = SendRequestSOAP("newWorkflow", body);
        return se_response.SelectToken("RecordID").ToString();
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
    public void editEntityRecord(string WorkflowID, string EntityID, Dictionary<string, string> EntityAttributeList = null, Dictionary<string, Dictionary<string, string>> RelationshipList = null, Dictionary<string, Anexo> EntityAttributeFileList = null)
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
                </soapenv:Envelope>";

        SendRequestSOAP("editEntityRecord", body);
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
    public void executeActivity(string WorkflowID, string ActivityID, int ActionSequence, string? UserID = null, int? ActivityOrder = null)
    {
        try
        {
            executeActivityWorkflowTry(WorkflowID, ActivityID, ActionSequence, UserID, ActivityOrder);
            return;
        }
        catch (SoftExpertException errorWF)
        {
            if(errorWF.Code == -20){
                throw;
            }
            try
            {
                executeActivityProblemTry(WorkflowID, ActivityID, ActionSequence, UserID, ActivityOrder);
                return;
            }
            catch (SoftExpertException errorPB)
            {
                if(errorPB.Code == -20){
                    throw;
                }
                try
                {
                    executeActivityIncidentTry(WorkflowID, ActivityID, ActionSequence, UserID, ActivityOrder);
                    return;
                }
                catch (SoftExpertException errorIn)
                {
                    if(errorIn.Code == -20){
                        throw;
                    }
                }
            }
            throw;
        }
        catch (System.Exception){
            throw;
        }
    }

    private void executeActivityWorkflowTry(string WorkflowID, string ActivityID, int ActionSequence, string? UserID = null, int? ActivityOrder = null)
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
                </soapenv:Envelope>";
        
        SendRequestSOAP("executeActivity", body);
    }

    private void executeActivityProblemTry(string WorkflowID, string ActivityID, int ActionSequence, string? UserID = null, int? ActivityOrder = null)
    {
        string body = $@"
                <soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:urn='urn:workflow'>
                   <soapenv:Header/>
                   <soapenv:Body>
                      <urn:executeActivity>
                         <!--You may enter the following 5 items in any order-->
                         <urn:ProblemID>{WorkflowID}</urn:ProblemID>
                         <urn:ActivityID>{ActivityID}</urn:ActivityID>
                         <urn:ActionSequence>{ActionSequence}</urn:ActionSequence>
                         <!--Optional:-->
                         <urn:UserID>{UserID}</urn:UserID>
                         <!--Optional:-->
                         <urn:ActivityOrder>{ActivityOrder}</urn:ActivityOrder>
                      </urn:executeActivity>
                   </soapenv:Body>
                </soapenv:Envelope>";
        
        SendRequestSOAP("executeActivity", body, "/apigateway/se/ws/pb_ws.php");
    }

    private void executeActivityIncidentTry(string WorkflowID, string ActivityID, int ActionSequence, string? UserID = null, int? ActivityOrder = null)
    {
        string body = $@"
                <soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:urn='urn:workflow'>
                   <soapenv:Header/>
                   <soapenv:Body>
                      <urn:executeActivity>
                         <!--You may enter the following 5 items in any order-->
                         <urn:IncidentID>{WorkflowID}</urn:IncidentID>
                         <urn:ActivityID>{ActivityID}</urn:ActivityID>
                         <urn:ActionSequence>{ActionSequence}</urn:ActionSequence>
                         <!--Optional:-->
                         <urn:UserID>{UserID}</urn:UserID>
                         <!--Optional:-->
                         <urn:ActivityOrder>{ActivityOrder}</urn:ActivityOrder>
                      </urn:executeActivity>
                   </soapenv:Body>
                </soapenv:Envelope>";
        
        SendRequestSOAP("executeActivity", body, "/apigateway/se/ws/in_ws.php");
    }

    /// <summary>
    /// Este método anexa um arquivo no menu de anexo do lado esquerdo de uma instancia
    /// </summary>
    /// <param name="WorkflowID">ID da instancia</param>
    /// <param name="ActivityID">ID da atividade a ser executada</param>
    /// <param name="File">Arquivo a ser anexado</param>
    /// <returns></returns>
    public int newAttachment(string WorkflowID, string ActivityID, Anexo File, string UserID = null)
    {
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
                </soapenv:Envelope>";

        var se_response = SendRequestSOAP("newAttachment", body);
        return Int32.Parse(se_response.SelectToken("RecordKey").ToString());
    }








    /// <summary>
    /// Este método permite você criar itens de uma grid de um formulário principal
    /// </summary>
    /// <param name="ProcessID">ID do processo</param>
    /// <param name="WorkflowTitle">Titulo da instância</param>
    /// <param name="UserID">Matrícula do usuário iniciador da instância</param>
    /// <returns>newWorkflowResponse, objeto com os campos Status, Code, Detail, RecordKey e RecordID. Se Code = 1 entao RecordID conterá o ID da intância gerada. Se Code != 1, uma SoftExpertException é gerada</returns>
    /// <exception cref="SoftExpertException"></exception>
    public void newChildEntityRecord(string WorkflowID, string MainEntityID, string ChildRelationshipID, Dictionary<string, string> EntityAttributeList = null, Dictionary<string, Dictionary<string, string>> RelationshipList = null, Dictionary<string, Anexo> EntityAttributeFileList = null)
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
                            </soapenv:Envelope>";

        SendRequestSOAP("newChildEntityRecord", body);
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
        //BUG: ao passa uma atividade para a função listAttachmentFromInstance, o SQL não traz resultados. Usar sem informar a atividade.
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
                        from {db_name}.wfprocess p
                        JOIN {db_name}.WFSTRUCT A ON A.IDPROCESS = P.IDOBJECT
                        JOIN {db_name}.WFPROCATTACHMENT ATAASSOC ON A.IDOBJECT = ATAASSOC.IDSTRUCT
                        JOIN {db_name}.ADATTACHMENT ANEXO ON ATAASSOC.CDATTACHMENT = ANEXO.CDATTACHMENT
                        join {db_name}.ADATTACHFILE a on ANEXO.CDATTACHMENT = a.CDATTACHMENT
                        join {db_name}.GNCOMPFILECONTCOPY c on a.CDCOMPLEXFILECONT = c.CDCOMPLEXFILECONT
                        join {db_name}.gnfile g on c.CDCOMPLEXFILECONT = g.CDCOMPLEXFILECONT
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
                anexo.nmuserupd = arquivo["NMUSERUPD"].ToString();
                var contentZip = (byte[])stream;
                var content = Utils.Zip.UnzipFile(contentZip);
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



    /// <summary>
    /// Traz todos os itens de uma grid de uma dada instancia
    /// </summary>
    /// <param name="WorkflowID">ID da instancia</param>
    /// <param name="MainEntityID">ID da entidade/tabela principal do formulário do processo</param>
    /// <param name="ChildEntityID">ID da entidade/tabela da grid</param>
    /// <param name="ChildOID">OID da grid. Pode ser obtida ao inspecionar uma grid dentro do formulário principal. Formato: OIDFBCX6LIPRTHT4H2</param>
    /// <returns></returns>
    public List<dynamic> listGridItems(string WorkflowID, string MainEntityID, string ChildEntityID, string ChildOID) {
        string sql = $@"SELECT grid.*
                        FROM {db_name}.wfprocess p
                        --
                        JOIN {db_name}.GNASSOCFORMREG GNF on p.cdassocreg = GNF.cdassoc
                        JOIN {db_name}.DYN{MainEntityID} formulario on formulario.oid = GNF.OIDENTITYREG
                        JOIN {db_name}.DYN{ChildEntityID} grid ON grid.{ChildOID} = formulario.oid
                        --
                        WHERE p.idprocess = :WorkflowID";

        Dictionary<string, dynamic> parametros = new Dictionary<string, dynamic>();
        parametros.Add(":WorkflowID", WorkflowID);

        DataTable list = null;
        try
        {
            list = db.Query(sql, parametros);

            return list.AsEnumerable()
            .Select(row =>
            {
                dynamic obj = new ExpandoObject();
                var objDictionary = (IDictionary<string, object>)obj;
                foreach (var column in list.Columns.Cast<DataColumn>())
                {
                    objDictionary[column.ColumnName.ToLower()] = row[column];
                }
                return obj;
            })
            .ToList();
        }
        catch (Exception erro)
        {
            throw new Exception($"Falha ao buscar os arquivo no bando de dados. Erro: {erro.Message}");
        }

    }




    /// <summary>
    /// Marca um anexo como sicronizado, para trabalhos de envio de anexos a outros sistemas
    /// </summary>
    /// <param name="anexo"></param>
    /// <returns></returns>
    public int setAttachmentSynced(int cdAttachment)
    {
        string sql = $@"UPDATE {db_name}.ADATTACHMENT SET NMUSERUPD  = NMUSERUPD||'-synced' WHERE CDATTACHMENT = :cdAttachment";

        Dictionary<string, dynamic> parametros = new Dictionary<string, dynamic>();
        parametros.Add(":cdAttachment", cdAttachment);

        return db.Execute(sql, parametros);
    }








    /// <summary>
    /// Obter os dados de uma instancia da tabela wfprocess
    /// </summary>
    /// <param name="WorkflowID"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public dynamic getWorkFlowData(string WorkflowID)
    {
        string sql = $@"SELECT p.*
                        FROM {db_name}.wfprocess p
                        WHERE p.idprocess = :WorkflowID";

        Dictionary<string, dynamic> parametros = new Dictionary<string, dynamic>();
        parametros.Add(":WorkflowID", WorkflowID);

        DataTable list = null;
        try
        {
            list = db.Query(sql, parametros);

            return list.AsEnumerable()
            .Select(row =>
            {
                dynamic obj = new ExpandoObject();
                var objDictionary = (IDictionary<string, object>)obj;
                foreach (var column in list.Columns.Cast<DataColumn>())
                {
                    objDictionary[column.ColumnName.ToLower()] = row[column];
                }
                return obj;
            })
            .FirstOrDefault();


        }
        catch (Exception erro)
        {
            throw new Exception($"Falha ao buscar os arquivo no bando de dados. Erro: {erro.Message}");
        }

    }






    /// <summary>
    /// Obtem os dados de um formulario de uma instancia. Necessário passar a instancia e o ID da entidade/tabela
    /// </summary>
    /// <param name="WorkflowID"></param>
    /// <param name="EntityID"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public dynamic getFormData(string WorkflowID, string EntityID)
    {
        string sql = $@"SELECT formulario.*
                        FROM {db_name}.wfprocess p
                        --
                        JOIN {db_name}.GNASSOCFORMREG GNF on p.cdassocreg = GNF.cdassoc
                        JOIN {db_name}.DYN{EntityID} formulario on formulario.oid = GNF.OIDENTITYREG
                        WHERE p.idprocess = :WorkflowID";

        Dictionary<string, dynamic> parametros = new Dictionary<string, dynamic>();
        parametros.Add(":WorkflowID", WorkflowID);


        DataTable list = db.Query(sql, parametros);
        if (list == null || list.Rows.Count == 0){
            throw new SoftExpertException($"A instancia de workflow '{WorkflowID}' não foi encontrada com a tabela '{EntityID}'");
        }

        return list.AsEnumerable()
        .Select(row =>
        {
            dynamic obj = new ExpandoObject();
            var objDictionary = (IDictionary<string, object>)obj;
            foreach (var column in list.Columns.Cast<DataColumn>())
            {
                objDictionary[column.ColumnName.ToLower()] = row[column];
            }
            return obj;
        })
        .FirstOrDefault();
    }





    /// <summary>
    /// Busca os itens de um selectbox de um formulario. Necessário passar a tabela do selectbox e o OID do registro dessa tabela
    /// </summary>
    /// <param name="oid"></param>
    /// <param name="EntityID"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public dynamic getFormSelectBox(string oid, string EntityID)
    {
        string sql = $@"SELECT * FROM {db_name}.DYN{EntityID} WHERE FGENABLED = 1 AND oid = :oid";

        Dictionary<string, dynamic> parametros = new Dictionary<string, dynamic>();
        parametros.Add(":oid", oid);


        DataTable list = db.Query(sql, parametros);

        return list.AsEnumerable()
        .Select(row =>
        {
            dynamic obj = new ExpandoObject();
            var objDictionary = (IDictionary<string, object>)obj;
            foreach (var column in list.Columns.Cast<DataColumn>())
            {
                objDictionary[column.ColumnName.ToLower()] = row[column];
            }
            return obj;
        })
        .FirstOrDefault();
    }


    public Anexo getFileFromOID(string oid){
        string sql = $@"select FLDATA, --possui o blod
                            NMNAME, --nome e extensão do arquivo
                            IDEXTENSION, --somente a extensão
                            NRSIZE -- tamanho do arquivo em bytes
                            from {db_name}.seblob
                            where oid = :OID";

        Dictionary<string, dynamic> parametros = new Dictionary<string, dynamic>();
        parametros.Add(":OID", oid);


        
        DataTable list = db.Query(sql, parametros);
        if (list == null || list.Rows.Count == 0){
            throw new SoftExpertException($"O oid '{oid}' não foi encontrado na tabela 'SEBLOB'");
        }

        return list.AsEnumerable()
            .Select(row =>
            {
                Anexo anexo = new Anexo();
                
                // Mapeamento dos campos para as propriedades da classe Anexo
                anexo.FileName = row["NMNAME"].ToString();
                anexo.Content = (byte[])row["FLDATA"];
                return anexo;
            })
            .FirstOrDefault();
    }



    /// <summary>
    /// Marca uma atividade como executada, mas não a executa de fato. Serve para desativar uma atividade.
    /// </summary>
    /// <param name="WorkflowID"></param>
    /// <param name="ActivityID"></param>
    /// <returns></returns>
    public int markActivityAsExecuted(string WorkflowID, string ActivityID)
    {
        string sql = $@"UPDATE {db_name}.WFSTRUCT SET FGSTATUS = 3 WHERE IDOBJECT = 
                        (SELECT A.IDOBJECT FROM {db_name}.wfprocess p JOIN {db_name}.wfstruct a on a.idprocess = p.idobject WHERE p.idprocess = :WorkflowID AND IDSTRUCT = :ActivityID)";

        Dictionary<string, dynamic> parametros = new Dictionary<string, dynamic>();
        parametros.Add(":WorkflowID", WorkflowID);
        parametros.Add(":ActivityID", ActivityID);

        int result = db.Execute(sql, parametros);
        if (result == 0) {
            return 0;
        }

        sql = $@"DELETE FROM {db_name}.wftask WHERE IDACTIVITY = 
                    (SELECT A.IDOBJECT FROM {db_name}.wfprocess p JOIN {db_name}.wfstruct a on a.idprocess = p.idobject WHERE p.idprocess = :WorkflowID AND IDSTRUCT = :ActivityID)";

        return db.Execute(sql, parametros);
    }




    /// <summary>
    /// Este método permite você EDITAR itens de uma grid de um formulário principal
    /// </summary>
    /// <param name="workflowID"></param>
    /// <param name="mainEntityID"></param>
    /// <param name="childRelationshipID"></param>
    /// <param name="childRecordOID"></param>
    /// <param name="formulario"></param>
    /// <returns></returns>
    public void editChildEntityRecord(string WorkflowID, string MainEntityID, string ChildRelationshipID, string childRecordOID, Dictionary<string, string> EntityAttributeList, Dictionary<string, Anexo>  EntityAttributeFileList = null)
    {
        string camposForm = Gerar_EntityAttributeList(EntityAttributeList);
        string anexos = Gerar_EntityAttributeFileList(EntityAttributeFileList);
        string body = $@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:urn=""urn:workflow"">
                                <soapenv:Header/>
                                <soapenv:Body>
                                    <urn:editChildEntityRecord>
                                        <!--You may enter the following 6 items in any order-->
                                        <urn:WorkflowID>{WorkflowID}</urn:WorkflowID>
                                        <urn:MainEntityID>{MainEntityID}</urn:MainEntityID>
                                        <urn:ChildRelationshipID>{ChildRelationshipID}</urn:ChildRelationshipID>
                                        <urn:ChildRecordOID>{childRecordOID}</urn:ChildRecordOID>

                                        <urn:EntityAttributeList>
                                            {camposForm}
                                        </urn:EntityAttributeList>


                                        <EntityAttributeFileList>
                                            {anexos}
                                        </EntityAttributeFileList>
                                        
                                    </urn:editChildEntityRecord>
                                </soapenv:Body>
                            </soapenv:Envelope>";
        
        SendRequestSOAP("editChildEntityRecord", body);
    }

    public Anexo getFileFromFormField(string WorkflowID, string MainEntityID, string FormField)
    {
        string sql = $@"SELECT SEBLOB.*
                            FROM {db_name}.wfprocess p
                            JOIN {db_name}.GNASSOCFORMREG GNF on p.cdassocreg = GNF.cdassoc
                            JOIN {db_name}.dyn{MainEntityID} formulario on formulario.oid = GNF.OIDENTITYREG
                            JOIN {db_name}.SEBLOB ON SEBLOB.OID = formulario.oid{FormField}
                            --
                            WHERE p.idprocess = :WorkflowID";

        Dictionary<string, dynamic> parametros = new Dictionary<string, dynamic>();
        parametros.Add(":WorkflowID", WorkflowID);


        
        DataTable list = db.Query(sql, parametros);
        if (list == null || list.Rows.Count == 0){
            throw new SoftExpertException($"Não foi encontrado na tabela 'SEBLOB' um OID do campo '{FormField}' da tabela '{MainEntityID}' ou o arquivo não foi anexado na instancia '{WorkflowID}'");
        }

        return list.AsEnumerable()
            .Select(row =>
            {
                Anexo anexo = new Anexo();
                
                // Mapeamento dos campos para as propriedades da classe Anexo
                anexo.FileName = row["NMNAME"].ToString();
                anexo.Content = (byte[])row["FLDATA"];
                return anexo;
            })
            .FirstOrDefault();
    }

    public int changeWorflowTitle(string workflowID, string title)
    {
        string sql = $@"UPDATE {db_name}.WFPROCESS SET NMPROCESS = :title WHERE IDPROCESS= :workflowID";

        Dictionary<string, dynamic> parametros = new Dictionary<string, dynamic>();
        parametros.Add(":title", title);
        parametros.Add(":workflowID", workflowID);

        return db.Execute(sql, parametros);
    }


    //TODO: Documentar e escrever testes sobre getWorflowStatus
    /// <summary>
    /// Este método verifica o Status de uma instância
    /// </summary>
    /// <param name="workflowID"></param>
    /// <returns>WFStatus</returns>
    public WFStatus getWorflowStatus(string WorkflowID){
        string sql = $@"SELECT fgstatus
                            FROM {db_name}.wfprocess p
                            WHERE p.idprocess = :WorkflowID";

        Dictionary<string, dynamic> parametros = new Dictionary<string, dynamic>();
        parametros.Add(":WorkflowID", WorkflowID);


        
        DataTable list = db.Query(sql, parametros);
        if (list == null || list.Rows.Count == 0){
            throw new SoftExpertException($"Não foi encontrado um workflow com o id '{WorkflowID}'");
        }

        int fgStatusValue = Convert.ToInt32(list.Rows[0]["fgstatus"]);

        if (Enum.IsDefined(typeof(WFStatus), fgStatusValue))
        {
            return (WFStatus)fgStatusValue;
        }
        else
        {
            throw new SoftExpertException($"Valor desconhecido para fgstatus: {fgStatusValue}");
        }
    }


    //TODO: Documentar e escrever testes sobre getWorflowActualActivities
    /// <summary>
    /// Este método verifica a lista de atividades habilitadas de uma instancia
    /// </summary>
    /// <param name="workflowID"></param>
    /// <returns>List<string></returns>
    public List<string> getActualActivities(string WorkflowID){
        string sql = $@"SELECT a.idstruct
                            FROM {db_name}.wfprocess p
                            --
                            JOIN {db_name}.wfstruct a on a.idprocess = p.idobject
                            JOIN {db_name}.wftask c on c.IDACTIVITY = a.idobject
                            --
                            WHERE p.idprocess = :WorkflowID";

        Dictionary<string, dynamic> parametros = new Dictionary<string, dynamic>();
        parametros.Add(":WorkflowID", WorkflowID);


        DataTable list = db.Query(sql, parametros);
        if (list == null || list.Rows.Count == 0){
            throw new SoftExpertException($"Não foi encontrado um workflow em andamento com o id '{WorkflowID}'. Verifique se ele realmente está em andamento");
        }

        return list.AsEnumerable()
               .Select(row => row["idstruct"].ToString())
               .ToList();

    }

    /// <summary>
    /// Cancela um fluxo de workflow, incidente ou problema
    /// </summary>
    /// <param name="workflowID">ID da instancia de workflow, incidente ou problema</param>
    /// <param name="explanation">Justificativa</param>
    /// <param name="userID">Matricula do usuario que está cancelando. Ele precisa ter permissão na segurança para cancelar</param>
    public void cancelWorkflow(string workflowID, string explanation, string userID = null)
    {
        try
        {
            cancelWorkflowTry(workflowID, explanation, userID);
            return;
        }
        catch (System.Exception errorWF)
        {
            try
            {
                cancelProblemTry(workflowID, explanation, userID);
                return;
            }
            catch (System.Exception)
            {
                 try
                {
                    cancelIncidentTry(workflowID, explanation, userID);
                    return;
                }
                catch (System.Exception)
                {
                    
                }
            }
            throw;
        }
    }


    private void cancelWorkflowTry(string workflowID, string explanation, string userID = null)
    {
        string body = $@"
                <soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:urn='urn:workflow'>
                   <soapenv:Header/>
                   <soapenv:Body>
                      <urn:cancelWorkflow>
                         <urn:WorkflowID>{workflowID}</urn:WorkflowID>
                         <urn:Explanation>{explanation}</urn:Explanation>
                         <urn:UserID>{userID}</urn:UserID>
                      </urn:cancelWorkflow>
                   </soapenv:Body>
                </soapenv:Envelope>";
        SendRequestSOAP("cancelWorkflow", body);
    }

    private void cancelProblemTry(string workflowID, string explanation, string userID = null)
    {
        string body = $@"
                <soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:urn='urn:workflow'>
                   <soapenv:Header/>
                   <soapenv:Body>
                      <urn:cancelProblem>
                         <urn:ProblemID>{workflowID}</urn:ProblemID>
                         <urn:Explanation>{explanation}</urn:Explanation>
                         <urn:UserID>{userID}</urn:UserID>
                      </urn:cancelProblem>
                   </soapenv:Body>
                </soapenv:Envelope>";
        
        SendRequestSOAP("cancelProblem", body, "/apigateway/se/ws/pb_ws.php");
    }

    private void cancelIncidentTry(string workflowID, string explanation, string userID)
    {
        string body = $@"
                <soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:urn='urn:workflow'>
                   <soapenv:Header/>
                   <soapenv:Body>
                      <urn:cancelIncident>
                         <urn:IncidentID>{workflowID}</urn:IncidentID>
                         <urn:Explanation>{explanation}</urn:Explanation>
                         <urn:UserID>{userID}</urn:UserID>
                      </urn:cancelIncident>
                   </soapenv:Body>
                </soapenv:Envelope>";
        
        SendRequestSOAP("cancelIncident", body, "/apigateway/se/ws/in_ws.php");
    }



    /// <summary>
    /// Adiciona um comentário no histório de uma instancia de WorkFlow
    /// </summary>
    /// <param name="workflowID">ID da instancia de workflow, incidente ou problema</param>
    /// <param name="explanation">Justificativa</param>
    /// <param name="userID">Matricula do usuario que está cancelando. Ele precisa ter permissão na segurança para cancelar</param>
    public void addHistoryComment(string workflowID, string comment, int cduser = 0)
    {
        string endpoint = "/apigateway/se/exp/chatbot/api/task/comment.php";
        try
        {
            Dictionary<string, dynamic> parametros = new Dictionary<string, dynamic>(){
                {"idInstance", workflowID},
                {"cdProduct", 39},
                {"text", comment.Replace("&", "e").Replace("#", "")},
            };
            if(cduser > 0){
                parametros.Add("cdUser", cduser);
            }
            string query = string.Join("&", parametros.Select(p => $"{p.Key}={p.Value}"));

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"{endpoint}?{query}");

            SendRequestRest(request);
            return;
        }
        catch (System.Exception errorWF)
        {
            throw;
        }
    }

    
    public void unlinkActivityFromUser(string workflowID, string ActivityID)
    {
        string body = $@"
                <soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:urn='urn:workflow'>
                <soapenv:Header/>
                <soapenv:Body>
                    <urn:unlinkActivityFromUser>
                        <urn:WorkflowID>{workflowID}</urn:WorkflowID>
                        <urn:ActivityID>{ActivityID}</urn:ActivityID>
                    </urn:unlinkActivityFromUser>
                </soapenv:Body>
                </soapenv:Envelope>";
        
        SendRequestSOAP("unlinkActivityFromUser", body);
    }



    public void reactivateWorkflow(string workflowID, string ActivityID, string explanation, string userID)
    {
        try
        {
            var obj = GetIDObjectToManageInstance(workflowID, ActivityID);
            if(obj == null){
                throw new Exception($"Não foi encontrada nenhuma instância de workflow com o ID '{workflowID}' e que possua a atividade '{ActivityID}'");
            }
            Dictionary<string, dynamic> parametros = new Dictionary<string, dynamic>(){
                {"oid", obj.p_idobject},
                {"action", 2},
            };
            string query = string.Join("&", parametros.Select(p => $"{p.Key}={p.Value}"));

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"/se/v16780/workflow/wf_management/management_action.php?{query}");

            string token = GetToken();
            request.Headers.Add("Cookie", $"se-authentication-token={token}");

            var payload = new Dictionary<string, string>
            {
                { "fgstatus", "1" },
                { "cditemreturn", obj.s_idobject },
                { "justify", explanation }
            };
            string jsonBody = JsonConvert.SerializeObject(payload);

            request.Content = new FormUrlEncodedContent(payload);


            HttpResponseMessage  response = restClient.SendAsync(request).Result;
            if(!response.IsSuccessStatusCode){
                throw new Exception("Houve um problema ao reativar a instancia");
            }

            string responseBody = response.Content.ReadAsStringAsync().Result;
            if(responseBody.Contains("softexpert/login")){
                throw new Exception("Houve um erro ao gerar/obter op token para reativar a instancia");
            }

            return;
        }
        catch (System.Exception errorWF)
        {
            throw;
        }
        

    }


    private dynamic GetIDObjectToManageInstance(string workflowID, string ActivityID){
        string sql = $@"select p.idprocess, s.IDSTRUCT, s.NMSTRUCT, s.IDOBJECT as s_IDOBJECT, s.DTENABLED, NRORDER, p.IDOBJECT as p_IDOBJECT, P.FGSTATUS
                            from {db_name}.WFPROCESS p
                            LEFT join {db_name}.WFSTRUCT s on p.IDOBJECT = s.IDPROCESS
                            where p.IDPROCESS = :workflowID and s.IDSTRUCT = :ActivityID
                            and s.DTENABLED is not null
                            order by s.DTENABLED DESC, s.TMENABLED DESC";

        Dictionary<string, dynamic> parametros = new Dictionary<string, dynamic>();
        parametros.Add(":workflowID", workflowID);
        parametros.Add(":ActivityID", ActivityID);


        DataTable list = db.Query(sql, parametros);

        if (list.Rows.Count > 0)
        {
            var row = list.Rows[0];
            return new 
            {
                s_idobject = row["s_IDOBJECT"].ToString(),
                p_idobject = row["p_IDOBJECT"].ToString()
            };
        }
        else
        {
            return null; // Ou retorne um objeto anônimo com valores padrão, se preferir
        }
    }    
}


