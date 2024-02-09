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

            
            if (xmljson.Contains("<head><title>400 Bad Request</title></head>"))
            {
                var msg = "400 Bad Request. Check your URL";
                parsed.RecordKey = null;
                parsed.RecordID = null;
                parsed.Status = STATUS.FAILURE;
                parsed.Code = 0;
                parsed.Detail = msg;
                throw new SoftExpertException(msg, parsed);
            }

            //in success case
            JToken response = parsed.PreParse(xmljson);
            parsed.RecordKey = response.SelectToken("RecordKey").ToString();
            parsed.RecordID = response.SelectToken("RecordID").ToString();

            
            return parsed;
        }


    }
}
