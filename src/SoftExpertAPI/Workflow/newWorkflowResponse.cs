using Newtonsoft.Json.Linq;

namespace SoftExpert.Workflow
{
    public class newWorkflowResponse : SoftExpertResponse
    {

        public string RecordKey { get; private set; }
        public string RecordID { get; private set; }

        public static newWorkflowResponse Parse(string xmljson)
        {
            newWorkflowResponse parsed = new newWorkflowResponse();
            try
            {
                JToken response = parsed.PreParse(xmljson);
                parsed.RecordKey = response.SelectToken("RecordKey").ToString();
                parsed.RecordID = response.SelectToken("RecordID").ToString();
            }
            catch (System.Exception error)
            {
                parsed.Detail = xmljson;
            }
            
            return parsed;
        }


    }
}
