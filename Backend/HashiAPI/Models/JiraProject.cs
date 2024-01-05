namespace HashiAPI_1.Models {

    public class JiraProject {
        public string Id {get; set;}
        public string Key {get; set;}
        public string Name {get; set;}
        public bool IsPrivate {get; set;}

        public JiraProject() {
            Id = "";
            Key = "";
            Name = "";
            IsPrivate = false;
        }

        override public string ToString() {
            return $"{Name} | {Key} | {Id}";
        }

    }
}