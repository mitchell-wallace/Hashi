namespace HashiAPI_1.Models {

    public class JiraUser {
        public string accountId {get; set;}
        public string accountType {get; set;}
        public string emailAddress {get; set;}
        public string displayName {get; set;}
        public bool active {get; set;}

        public JiraUser() {
            accountId = "";
            accountType = "";
            emailAddress = "";
            displayName = "";
            active = true;
        }

        override public string ToString() {
            return $"{displayName} | {emailAddress}";
        }

    }

}