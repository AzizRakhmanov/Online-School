using EnvDTE;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.OnlineSchoolCrm>("onlineschoolcrm");

builder.Build().Run();
