namespace SoftExpert.Workflow;

public class newChildEntityRecordResponse : SoftExpertResponse
{
    internal static newChildEntityRecordResponse Parse(string xmljson)
    {
        newChildEntityRecordResponse parsed = new newChildEntityRecordResponse();
        parsed.PreParse(xmljson);
        return parsed;
    }
}