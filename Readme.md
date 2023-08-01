# PowerShellRunner

PowerShellRunner - это библиотека для типизированной работы с PowerShell.

Пример типизированного описания командлета в коде:

```csharp
public class ConvertToSecureStringCmdlet : IPowerShellCmdlet<SecureString>
{
    public string CmdletName => "ConvertTo-SecureString";

    public IPowerShellCmdletParameter<string> String { get; }
    public IPowerShellCmdletParameter<SwitchParameter> AsPlainText { get; }
    public IPowerShellCmdletParameter<SwitchParameter> Force { get; }
}
```

Пример формирования комнады:

```csharp
ConvertToSecureStringCmdlet cmdlet =
    new ConvertToSecureStringCmdlet()
        .Set(c => c.String, "Value")
        .Set(c => c.AsPlainText, true)
        .Set(c => c.Force, false);

PowerShellQuery query = cmdlet.BuildFromCmdlet();

// query.Query: ConvertTo-SecureString -String "Value" -AsPlainText
```

Пример выполнения комнады с сохранением в переменную:

```csharp
PowerShellVariable<SecureString> result =
    powerShellAccessor
        .SelectCmdlet(cmdlet)
        .ExecuteAndSetTo("$converted_string");
```

При выполнении командлета автоматически резолвится тип результата / тип переменной в которую будет записан результат. В дальнейшем переменную можно передавать в другие командлеты:

```csharp
public class SomeOtherCmdlet : IPowerShellCmdlet<string>
{
    public string CmdletName => "Some-Other";

    public IPowerShellCmdletParameter<SecureString> String { get; }
}

PowerShellVariable<SecureString> convertedString =
    powerShellAccessor
        .SelectCmdlet(cmdlet)
        .ExecuteAndSetTo("$converted_string");

SomeOtherCmdlet otherCmdlet =
    new SomeOtherCmdlet()
        .Set(c => c.String, convertedString);

IReadOnlyCollection<string> result =
    powerShellAccessor
        .SelectCmdlet(otherCmdlet)
        .Execute();
```

