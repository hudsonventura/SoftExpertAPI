# SoftExpertAPI
SoftExpertAPI é uma biblioteca que possui um conjunto de classes para abstrair a comunicação SOAP ou REST com a API do SoftExpert SESuite.<br>
Esta biblioteca não está completa e será desenvolvida conforme necessidades e pedidos.
<br>
Direitos reservados a https://www.softexpert.com/<br>
<br>
Documentação original: https://documentation.softexpert.com/en/integration/index.html
<br>
<br>
Se você quer falar comigo, por qualquer proposito, me mande um email. hudsonventura@outlook.comf

<br>
<br>

Há exemplos funcionais no diretório `Examples`

### Importação do namespace ...
```C#
using SoftExpert;
using SoftExpert.Workflow;
```
<br>
<br>

### Criar uma instancia da API de workflow para uso
```C#
string authorization = "Basic base64encode(DOMINIO\USUARIO:SENHA)"; //deve ser codificado em base64
string url = "https://se.dominio.com.br";
SoftExpertWorkflowApi wfAPI = new SoftExpertWorkflowApi(url, authorization);
```

<br>
<br>

### Usando a API - Criando uma instancia de workflow

```C#
string ProcessID = "CCF";                       //identificador do processo
string WorkflowTitle = "Teste de integração"; ; //titulo da instancia a ser criado
string UserID = "00000000000";                  //matricula do usuario

newWorkflowResponse responseNewWF;
try
{
    responseNewWF = wfAPI.newWorkflow(ProcessID, WorkflowTitle, UserID);
}
catch (Exception erro)
{
    Console.WriteLine($"Não foi possivel criar o workflow. Erro: {erro.Message}");
    return;
}
string WorkflowID = responseNewWF.RecordID;
int codigoNewWorkFlow = responseNewWF.Code;
SoftExpert.SoftExpertResponse.STATUS sucessoNewWorkFlow = responseNewWF.Status;
string detalhesNewWorkflow = responseNewWF.Detail;
```

<br>
<br>


### Usando a API - Editando dados do formulário

```C#
Dictionary<string, string> formulario = new Dictionary<string, string>();
formulario.Add("possuiendereco", "1"); //id do campo do formulário e valor (em string)
formulario.Add("ramal", "N/A");

Dictionary<string, Dictionary<string, string>> relacionamentos = new Dictionary<string, Dictionary<string, string>>();
relacionamentos.Add("tipocliente", //idrelacionamento
    new Dictionary<string, string>() {
            //{ "campodoformdorelacionamento", "valor" },
            { "tipo", "PESSOA JURIDICA (CNPJ)" },
    }
);

string EntityID = "SOLCLIENTEFORNE";

//em caso de adicionar arquivos no formulário
string filePath = "120.png";
string FileName = "logo.png";                       //Nome do arquivo com a extensão
byte[] FileContent = File.ReadAllBytes(filePath);   //Binário do arquivo
Dictionary<string, Anexo> arquivos = new Dictionary<string, Anexo>();
arquivos.Add("al5termoassinad", new Anexo() { FileName = FileName, Content = FileContent });


editEntityRecordResponse entityResponse;
try
{
    entityResponse = wfAPI.editEntityRecord(
        WorkflowID, EntityID, formulario, //campos obrigatórios
        relacionamentos, arquivos); //campos opcionais
}
catch (Exception erro)
{
    Console.WriteLine($"Não foi possivel editar o formulário. Erro: {erro.Message}");
    return;
}
int sucessoEntity = entityResponse.Code;
string detalhesEntity = entityResponse.Detail;
```

<br>
<br>

### Usando a API - Editando dados da grid do formulário

```C#
//Os campos são os mesmo da função editEntityRecord, adicionando o `ChildRelationshipID` que é o ID do relacionamento


newChildEntityRecordResponse entityResponse;
try
{
    entityResponse = wfAPI.newChildEntityRecord(WorkflowID, EntityID, ChildRelationshipID,  formulario, //campos obrigatórios
                                                                            relacionamentos, arquivos); //campos opcionais
}
catch (Exception erro)
{
    Console.WriteLine($"Não foi possivel editar o formulário. Erro: {erro.Message}");
    return;
}
int sucessoEntity = entityResponse.Code;
string detalhesEntity = entityResponse.Detail;
```

<br>
<br>



### Usando a API - Execução de atividade

```C#
string ActivityID = "ATIV-SOLCCF";      //ID da atividade do fluxograma
int ActionSequence = 3;                 //Sequence da ação da atividade. Veja na lista de ações da atividade

executeActivityResponse executeResponse;
try
{
    executeResponse = wfAPI.executeActivity(WorkflowID, ActivityID, ActionSequence, UserID);
}
catch (Exception erro)
{
    Console.WriteLine($"Não foi possivel executar a atividade. Erro: {erro.Message}");
    return;
}
var houveSucesso = executeResponse.Code;
var detalhes = executeResponse.Detail;
```

<br>
<br>

### Usando a API - Anexar arquivo (menu do lado esquerdo de uma instancia)

```C#
string filePath = "120.png";                                           
byte[] FileContent = File.ReadAllBytes(filePath);   //Binário do arquivo
string ActivityID = "ATIV-SOLCCF";                  //ID da atividade em que o arquivo será anexado
string WorkflowID = "CCF202323106";                 //ID da instancia
Anexo Arquivo = new Anexo() {                       //pode-se passar também uma List<Anexo>
    FileName = "logo.png",                          //Nome do arquivo com a extensão
    Content = FileContent                           //Binário do arquivo
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
```

<br>
<br>