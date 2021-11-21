using Newtonsoft.Json.Linq;

namespace TodoAPI.Models.GraphQl
{
    public class GraphQlquery
    {
        public string OperationName { get; set; }
        public string NamedQuery { get; set; }
        public string Query { get; set; }
        public JObject Variables { get; set; }
    }
}
