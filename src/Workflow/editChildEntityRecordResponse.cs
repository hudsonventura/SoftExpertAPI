namespace SoftExpert.Workflow;

public class editChildEntityRecordResponse : SoftExpertResponse
{
    internal static editChildEntityRecordResponse Parse(string xmljson)
    {
        editChildEntityRecordResponse parsed = new editChildEntityRecordResponse();
        parsed.PreParse(xmljson);
        return parsed;
    }
}