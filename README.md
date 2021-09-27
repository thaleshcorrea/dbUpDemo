# DbUpDemo - Atualização do banco
Projeto criado com intuito de validar o funcionamento de uma nova ferramenta para manipulação de scripts no banco de dados, o DbUp.

## O que foi validado
### DbUp atualização
O código abaixo mostra como o dbUp pega os scripts e atualiza o banco de dados
```c#
var connectionString = "Server=localhost;Database=dbUpDemo;Trusted_Connection=True;";

EnsureDatabase.For.SqlDatabase(connectionString);

var upgradeEngineBuilder = DeployChanges.To
    .SqlDatabase(connectionString)
    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
    .WithTransactionPerScript()
    .LogToConsole();

var upgrader = upgradeEngineBuilder.Build();

var result = upgrader.PerformUpgrade();

// Display the result
if (result.Successful)
{
    Environment.ExitCode = (int)ExitCode.Success;

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Update successful!");
}
else
{
    Environment.ExitCode = (int)ExitCode.Error;

    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(result.Error);
    Console.WriteLine("Update failed, check logs for more information.");
}
```
Todos os logs são exibidos em console, então precisava de uma forma de pegar esses logs e passar para um textbox ou outra coisa visual no sistema, então surgiu uma forma de redirecionar os logs que são exibidos no console para o app windows

### Logs
Abaixo está como ficou o código para execução do programa de atualização e exibiçao dos logs

#### Execuçao do software de atualização sem exibição da tela preta do console
```c#
using (var process = new Process())
{
    process.StartInfo.FileName = "VendaFacil.Update.exe";
    process.StartInfo.Arguments = _connectionString;
    process.StartInfo.UseShellExecute = false;
    process.StartInfo.RedirectStandardOutput = true;

    process.OutputDataReceived += SaidaAtualizacao;
    process.Start();

    process.BeginOutputReadLine();
}
```

#### Exibição dos logs de forma assincrona
```c#
delegate void SafeTextChange();
private void AtualizarTexto(string texto)
{
    if (tbLog.InvokeRequired)
    {
        SafeTextChange textChange = new SafeTextChange(() => AtualizarTexto(texto));
        Invoke(textChange);
    }
    else
    {
        tbLog.Text += $"{texto}\n";
    }
}

private void SaidaAtualizacao(object sendingProcess,
    DataReceivedEventArgs outLine)
{
    if (!string.IsNullOrEmpty(outLine.Data))
    {
          AtualizarTexto(outLine.Data);
    }
}
```
