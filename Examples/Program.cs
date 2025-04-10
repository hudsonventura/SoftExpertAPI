using Examples;
using Microsoft.Extensions.Configuration;



IConfiguration appsettings = new ConfigurationBuilder()
	.SetBasePath(Directory.GetCurrentDirectory())
	.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
	.Build();

var Args = Environment.GetCommandLineArgs();
switch (Args[1])
{
	case "WorkflowExamples":
		WorkflowExamples wf = new WorkflowExamples(appsettings);
		wf.Execute(WorkflowExamples.Teste.delegateWorkflow);
		break;

	case "AdminExamples":
		AdminExamples admin = new AdminExamples(appsettings);
		admin.Execute();
		break;
		
	case "GenericExample":
		GenericExample gen = new GenericExample(appsettings);
		gen.Execute();
		break;

	default:
		Console.WriteLine("Invalid argument. Use 1 for WorkflowExamples or 2 for AdminExamples.");
		return;
}

