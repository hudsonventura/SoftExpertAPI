
namespace SoftExpert.Workflow
{
    public class executeActivityResponse : SoftExpertResponse
    {
        public new string Status { get; private set; }


        public static executeActivityResponse Parse(string xmljson)
        {
            executeActivityResponse parsed = new executeActivityResponse();
            parsed.PreParse(xmljson);
            return parsed;
        }
    }
}
