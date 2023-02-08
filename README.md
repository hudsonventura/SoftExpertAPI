# SoftExpertAPI
SoftExpertAPI é uma biblioteca que possui um conjunto de classes para abstrair a comunicação SOAP com a API do SoftExpert SESuite.<br>
Esta biblioteca não está completa e será desenvolvida conforme necessidades e pedidos.
<br>
Direitos reservados a https://www.softexpert.com/<br>
<br>
Documentação original: https://documentation.softexpert.com/en/integration/index.html
<br>
<br>
Se você quer falar comigo, por qualquer proposito, me mande um email. hudsonventura@outlook.com
<br>
<br>
### Importação do namespace ...
```C#
using SoftExpert;
using SoftExpert.Workflow;
```
<br>
<br>

### Criar uma instancia da API de workflow para uso
```C#
string Authorization = "Basic base64encode(DOMINIO\USUARIO:SENHA)"; //deve ser codificado em base64
string seURL = "https://se.dominio.com.br";
SoftExpertWorkflowApi wf = new SoftExpertWorkflowApi(seURL, Authorization);
```

<br>
<br>

### Usando a API - Criando uma instancia de workflow

```C#
string ProcessID = "ID do processo";
string WorkflowTitle = "Título da instancia";
string UserID = "Matricula do solicitante";
var newWorkflowResponse = wf.newWorkflow(ProcessID, WorkflowTitle, UserID);

//Retorno: newWorkflowResponse
var instancia = newWorkflowResponse.RecordID;
var houveSucesso = newWorkflowResponse.Code;
var detalhes = newWorkflowResponse.Detail;
```

<br>
<br>


### Usando a API - Editando dados do formulário

```C#
string WorkflowID = "ID da instancia";
string EntityID = "ID da tabela (entidade)";

Dictionary<string, string> formulario = new Dictionary<string, string>();
formulario.Add("campodoform", "valor a ser inserido");
formulario.Add("possuiendereco", "1");
formulario.Add("ramal", "N/A");

Dictionary<string, Dictionary<string, string>> relacionamentos = new Dictionary<string, Dictionary<string, string>>();
        relacionamentos.Add("idrelacionamento",
            new Dictionary<string, string>() {
                { "campodoformdorelacionamento", "valor" },
                { "tipo", "PESSOA JURIDICA (CNPJ)" },
            }
        );

var editEntityRecordResponse = wf.editEntityRecord(WorkflowID, EntityID, formulario, relacionamentos);

//Retorno: editEntityRecordResponse
var houveSucesso = editEntityRecordResponse.Code;
var detalhes = editEntityRecordResponse.Detail;
```

<br>
<br>


### Usando a API - Execução de atividade

```C#
string WorkflowID = "ID da instancia";
string ActivityID = "ID da atividade do fluxograma";
string ActionSequence = "Sequence da ação da atividade. Veja na lista de ações da atividade";
string UserID = "Matricula do executor";


var executeActivityResponse = wf.executeActivity(WorkflowID, ActivityID, ActionSequence, UserID);

//Retorno: executeActivityResponse
var houveSucesso = executeActivityResponse.Code;
var detalhes = executeActivityResponse.Detail;
```

<br>
<br>