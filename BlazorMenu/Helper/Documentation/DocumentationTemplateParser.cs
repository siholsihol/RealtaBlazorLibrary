namespace BlazorMenu.Helper.Documentation
{
    internal sealed class DocumentationTemplateParser
    {
        internal static string ParseTemplate(string pcProgramId, string pcProgramName)
        {
            var lcFileExtension = ".htm";
            var lcModule = pcProgramId.Substring(0, 2);
            var lcProgramType = pcProgramId.Substring(2, 1);
            var lcProgram = string.Join(" - ", pcProgramId, pcProgramName).Replace(" ", "_") + lcFileExtension;

            return string.Join("/", lcModule, lcProgramType, lcProgram);
        }
    }
}
