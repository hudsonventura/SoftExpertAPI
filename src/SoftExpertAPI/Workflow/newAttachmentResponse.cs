using System;

namespace SoftExpert.Workflow;

public class newAttachmentResponse : SoftExpertResponse
{
    internal static newAttachmentResponse Parse(string xmljson)
    {
        newAttachmentResponse parsed = new newAttachmentResponse();
        parsed.PreParse(xmljson);
        return parsed;
    }
}