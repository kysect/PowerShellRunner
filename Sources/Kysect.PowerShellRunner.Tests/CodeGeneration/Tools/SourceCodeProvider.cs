namespace Kysect.PowerShellRunner.Tests.CodeGeneration.Tools;

// TODO: inline it inside tests
public static class SourceCodeProvider
{
    public static string ClassWithAttributes()
    {
        return @"public class Nouns
{
    public string Cats = ""Qqwe"";
}

[Cmdlet(Verbs.Get, Nouns.Cats, DefaultParameterSetName = Parameters.AllParameterSets)]
public class GetCats { }
";
    }

    public static string EnumWithMembers()
    {
        return @"public enum Enum1
{
    Value2 = 2,
    Value3
}";
    }

    public static string ClassWithProperties()
    {
        return @"public class Class1
{
    public int Value1 { get; set; }
    private int Value2 { get; set; }
}";
    }

    public static string ClassWithPublicOnlyProperties()
    {
        return @"public class Class1
{
    public int Value1 { get; set; }
}";
    }
}