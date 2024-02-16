using Examples;
using Microsoft.Extensions.Configuration;

//TODO: dependendo dos caracteres do WorkflowTitle, a instancia não pode ser criada.
//TODO: anexar arquivo no form
//BUG: ao passa uma atividade para a função listAttachmentFromInstance, o SQL não traz resultados. Usar sem informar a atividade.
//TODO: Criar o cancelamento do Workflow


IConfiguration appsettings = new ConfigurationBuilder()
	.SetBasePath(Directory.GetCurrentDirectory())
	.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
	.Build();

WorkflowExamples wf = new WorkflowExamples(appsettings);
wf.Execute(WorkflowExamples.Teste.NewWorkflow);

AdminExamples admin = new AdminExamples(appsettings);
admin.Execute();