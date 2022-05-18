﻿

namespace SoftExpert.Workflow
{
    public class editEntityRecordResponse : SoftExpertResponse
    {
        public new string Status { get; private set; }


        public static editEntityRecordResponse Parse(string xmljson)
        {
            editEntityRecordResponse parsed = new editEntityRecordResponse();
            parsed.PreParse(xmljson);
            return parsed;
        }
    }
}
