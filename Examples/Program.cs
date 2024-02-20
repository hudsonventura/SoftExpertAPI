using Examples;
using Microsoft.Extensions.Configuration;



IConfiguration appsettings = new ConfigurationBuilder()
	.SetBasePath(Directory.GetCurrentDirectory())
	.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
	.Build();

WorkflowExamples wf = new WorkflowExamples(appsettings);
wf.Execute(WorkflowExamples.Teste.NewWorkflow);

AdminExamples admin = new AdminExamples(appsettings);
admin.Execute();