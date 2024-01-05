using Newtonsoft.Json;

namespace HashiAPI_1.Models {

    public class WrikeProject {

        public string Title {get; set;}
        public string Id {get; set;}

        public WrikeProject() {
            Title = "";
            Id = "";
        }

        override public string ToString() {
            return $"{Title} | {Id}";
        }

    }
}