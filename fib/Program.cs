
using System.CommandLine;
using System.ComponentModel;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;
//create the rootCommand
var RootCommand=new RootCommand("Root command for File bundler cli");
//create the command and its options
var boundleCommand = new Command("bundle");
RootCommand.AddCommand(boundleCommand);
var languageOption = new Option<string>("--language", "languages list");
boundleCommand.AddOption(languageOption);
languageOption.IsRequired = false;
var outputOption = new Option<FileInfo>("--output", "languages list");
boundleCommand.AddOption(outputOption);
outputOption.IsRequired= false;
var noteOption = new Option<bool>("--note", "");
boundleCommand.AddOption(noteOption);
noteOption.IsRequired= false;
var sortOption = new Option<bool>("--sort", "");
boundleCommand.AddOption(sortOption);
sortOption.IsRequired= false;
var removeEmptyLinesOption = new Option<bool>("--remove-empty-lines", "");
boundleCommand.AddOption(removeEmptyLinesOption);
removeEmptyLinesOption.IsRequired= false;
var authorOption = new Option<string>("--author", "");
boundleCommand.AddOption(authorOption);
authorOption.IsRequired= false;
//languages arr
string[] allLanuages = { ".css", ".java", ".html", ".js" ,".txt",};
static bool isGoodLanguage(string[] languages,string lan)
{
    foreach (var l in languages)
    {
        if (l == lan)
        {
            return true;
        }
    }
    return false;
}
//the main function
boundleCommand.SetHandler((language, output, note, sort, removeEmptyLines, author) =>
{
    string resFilePath = "";
    string res = "";
    if (author != null)
    {
        res += "//"+author+"\n";
    }
    if (!output.FullName.Contains("\\")) {
        resFilePath = Environment.CurrentDirectory + "\\" + output.FullName + ".txt";
    }
    else
        resFilePath=output.FullName;
    string[] filesArray = Directory.GetFiles(Environment.CurrentDirectory);
    if (sort == false)
    {
        filesArray.OrderBy(f => Path.GetExtension(f)).ToArray();
    }
    else
    {
        filesArray.OrderBy(f=>Path.GetFileName(f)).ToArray();
    }
    //create the languages array
    string[] languages = language.Split("_");  
    foreach (var fd in filesArray)
    {
        //adding the string in the files
        string thislan = fd.Substring(fd.LastIndexOf('.'));
        if ((language == "all" && isGoodLanguage(allLanuages, thislan)) ||
        isGoodLanguage(languages, thislan))
        {
            //adding a note if wanted
            if (note)
            {
                res += fd+"\n";
            }
            res+=  File.ReadAllText(fd);
        }
    }
    //removes empty lines if wanted
    if (removeEmptyLines == true)
    {
        string res1 = "";
        using (StringReader sr = new StringReader(res))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (!String.IsNullOrWhiteSpace(line))
                    res1 += line+"\n";
            }
        }
        res = res1;
    }
    File.WriteAllText(resFilePath, res);

}, languageOption, outputOption, noteOption, sortOption, removeEmptyLinesOption, authorOption);
var createRsp = new Command("create-rsp");
RootCommand.AddCommand(createRsp);
createRsp.SetHandler(() =>
{
    Console.WriteLine("input yout project path or name:");
    var folderPath=Console.ReadLine();
    Console.WriteLine("Insert the list of languages of the files you want to package, insert '_' between language");
    var languages = Console.ReadLine();
    Console.WriteLine("Enter the new file path or name ");
    var output = Console.ReadLine();
    Console.WriteLine("Enter true if you want to get a comment at the beginning of each file");
    var note = Console.ReadLine();
    Console.WriteLine("Enter true if you want to sort the files by type");
    var sort = Console.ReadLine();
    Console.WriteLine("Enter true if you want to delete empty lines from the file");
    var removeEmptyLines = Console.ReadLine();
    Console.WriteLine("Enter the creator's name if you want it to be written at the top of the exported file ");
    var author = Console.ReadLine();
    string res =  "bundle --language " + languages + " --output " + output + " --note " + note + " --sort " + sort + " --remove-empty-lines " +
    removeEmptyLines + " --author " + author;
    File.WriteAllText(Environment.CurrentDirectory + "\\run.rsp", res);
});
RootCommand.InvokeAsync(args);

